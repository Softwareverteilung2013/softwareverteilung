using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Data.SqlServerCe;
using ProjektSoftwareverteilung2013.Controller;
using ProjektSoftwareverteilung2013.Models;
using ProjektSoftwareverteilung2013.Datenbanken;

namespace ProjektSoftwareverteilung2013.Controller
{
    class DataConverter
    {
        private SqlCeConnection Connection = null;

        public DataConverter(SqlCeConnection oConn)
        {
            this.Connection = oConn;
        }

        public List<ClientInfoModel> GetClientInfoModels()
        {
            try
            {
                List<ClientInfoModel> oResult = new List<ClientInfoModel>();
                DataTable oData = new DataTable();

                SqlCeDataAdapter oDataAdapter = new SqlCeDataAdapter("SELECT * FROM Client", Connection);
                oDataAdapter.Fill(oData);

                if (oData.Rows.Count > 0)
                {
                    foreach (DataRow oRow in oData.Rows)
                    {
                        ClientInfoModel oClient = new ClientInfoModel();

                        oClient.macAddress = oRow["Client_MacAdresse"].ToString();
                        oClient.admin = Convert.ToBoolean(oRow["Client_Administrator"]);
                        oClient.group = Convert.ToInt32(oRow["Client_Gruppe"]);
                        oClient.ID = Convert.ToInt32(oRow["Client_ID"]);
                        oClient.arc = Convert.ToString(oRow["Client_Arc"]);
                        oClient.pcName = oRow["Client_PCName"].ToString();

                        oResult.Add(oClient);
                    }
                }
                return oResult;
            }
            catch (Exception ex)
            {
                Diagnostics.WriteToEventLog(ex.Message, System.Diagnostics.EventLogEntryType.Error, 3011);
                return null;
            }
        }

        public List<GroupInfoModel> GetGroupInfoModels()
        {
            try
            {
                List<GroupInfoModel> oResult = new List<GroupInfoModel>();
                DataTable oData = new DataTable();

                SqlCeDataAdapter oDataAdapter = new SqlCeDataAdapter("SELECT * FROM Gruppe", Connection);
                oDataAdapter.Fill(oData);

                if (oData.Rows.Count > 0)
                {
                    foreach (DataRow oRow in oData.Rows)
                    {
                        GroupInfoModel oGroup = new GroupInfoModel();

                        oGroup.ID = Convert.ToInt32(oRow["Gruppe_ID"]);
                        oGroup.Name = oRow["Gruppe_Name"].ToString();

                        oResult.Add(oGroup);
                    }
                }

                return oResult;
            }
            catch (Exception ex)
            {
                Diagnostics.WriteToEventLog(ex.Message, System.Diagnostics.EventLogEntryType.Error, 3012);
                return null;
            }
        }

        public List<PackageInfoModel> GetPackageInfoModels()
        {
            try
            {
                List<PackageInfoModel> oResult = new List<PackageInfoModel>();
                DataTable oData = new DataTable();

                SqlCeDataAdapter oDataAdapter = new SqlCeDataAdapter("SELECT * FROM Softwarepaket", Connection);
                oDataAdapter.Fill(oData);

                if (oData.Rows.Count > 0)
                {
                    foreach (DataRow oRow in oData.Rows)
                    {
                        PackageInfoModel oPackage = new PackageInfoModel();

                        oPackage.ID = Convert.ToInt32(oRow["Softwarepaket_ID"]);
                        oPackage.Name = oRow["Softwarepaket_Name"].ToString();
                        oPackage.size = Convert.ToInt32(oRow["Softwarepaket_Groesse"]);
                        oPackage.arc = oRow["Softwarepaket_Arc"].ToString();
                        oPackage.showName = oRow["Softwarepaket_ShowName"].ToString();

                        oResult.Add(oPackage);
                    }
                }

                return oResult;
            }
            catch (Exception ex)
            {
                Diagnostics.WriteToEventLog(ex.Message, System.Diagnostics.EventLogEntryType.Error, 3013);
                return null;
            }
        }

        public List<ClientInfoModel> GetGroupClients(GroupInfoModel oGroup)
        {
            try
            {
                List<ClientInfoModel> oResult = new List<ClientInfoModel>();
                DataTable oData = new DataTable();
                string sQry;

                sQry = "SELECT * FROM Client WHERE Client_Gruppe = " + oGroup.ID;
                SqlCeDataAdapter oDataAdapter = new SqlCeDataAdapter(sQry, Connection);

                oDataAdapter.Fill(oData);

                foreach (DataRow oRow in oData.Rows)
                {
                    if (!Convert.ToBoolean(oRow["Client_Administrator"]))
                    {
                        ClientInfoModel client = new ClientInfoModel();

                        client.ID = Convert.ToInt32(oRow["Client_ID"]);
                        client.macAddress = oRow["Client_MacAdresse"].ToString();
                        client.group = Convert.ToInt32(oRow["Client_Gruppe"]);
                        client.arc = oRow["Client_Arc"].ToString();
                        client.admin = Convert.ToBoolean(oRow["Client_Administrator"]);
                        client.pcName = oRow["Client_PCName"].ToString();

                        oResult.Add(client);
                    }
                }

                return oResult;
            }
            catch (Exception ex)
            {
                Diagnostics.WriteToEventLog(ex.Message, System.Diagnostics.EventLogEntryType.Error, 3014);
                return null;
            }
        }

        public List<PackageInfoModel> GetGroupPackages(GroupInfoModel oGroup)
        {
            try
            {
                List<PackageInfoModel> oResult = new List<PackageInfoModel>();
                DataTable oData = new DataTable();
                string sQry;

                sQry = "    SELECT S.Softwarepaket_ID, S.Softwarepaket_Name " +
                       "           , S.Softwarepaket_Groesse, S.Softwarepaket_Arc " +
                       "           , S.Softwarepaket_ShowName " +
                       "      FROM Softwarepaket AS S" +
                       " LEFT JOIN Gruppe_Softwarepakete AS GS ON S.Softwarepaket_ID = GS.Softwarepaket_ID " +
                       "     WHERE GS.Gruppe_ID = " + oGroup.ID;
                SqlCeDataAdapter oDataAdapter = new SqlCeDataAdapter(sQry, Connection);
                oDataAdapter.Fill(oData);

                foreach (DataRow oRow in oData.Rows)
                {
                    PackageInfoModel package = new PackageInfoModel();

                    package.ID = Convert.ToInt32(oRow["Softwarepaket_ID"]);
                    package.Name = oRow["Softwarepaket_Name"].ToString();
                    package.size = Convert.ToInt32(oRow["Softwarepaket_Groesse"]);
                    package.arc = oRow["Softwarepaket_Arc"].ToString();
                    package.showName = oRow["Softwarepaket_ShowName"].ToString();

                    oResult.Add(package);
                }

                return oResult;
            }
            catch (Exception ex)
            {
                Diagnostics.WriteToEventLog(ex.Message, System.Diagnostics.EventLogEntryType.Error, 3015);
                return null;
            }
        }

        public List<PackageInfoModel> GetClientPackages(ClientInfoModel oClient)
        {
            try
            {
                List<PackageInfoModel> oResult = new List<PackageInfoModel>();
                DataTable oData = new DataTable();
                string sQry;

                sQry = "    SELECT SP.Softwarepaket_ID, SP.Softwarepaket_Name " +
                       "           , SP.Softwarepaket_Groesse, SP.Softwarepaket_Arc " +
                       "           , SP.Softwarepaket_ShowName " +
                       "      FROM Client_Softwarepaket AS CP " +
                       " LEFT JOIN Softwarepaket AS SP ON CP.Softwarepaket_ID = SP.Softwarepaket_ID " +
                       "     WHERE Client_ID = " + oClient.ID;
                SqlCeDataAdapter oDataAdapter = new SqlCeDataAdapter(sQry, Connection);
                oDataAdapter.Fill(oData);

                foreach (DataRow oRow in oData.Rows)
                {
                    PackageInfoModel package = new PackageInfoModel();

                    package.ID = Convert.ToInt32(oRow["Softwarepaket_ID"]);
                    package.Name = oRow["Softwarepaket_Name"].ToString();
                    package.size = Convert.ToInt32(oRow["Softwarepaket_Groesse"]);
                    package.arc = oRow["Softwarepaket_Arc"].ToString();
                    package.showName = oRow["Softwarepaket_ShowName"].ToString();

                    oResult.Add(package);
                }

                return oResult;
            }
            catch (Exception ex)
            {
                Diagnostics.WriteToEventLog(ex.Message, System.Diagnostics.EventLogEntryType.Error, 3016);
                return null;
            }
        }

        public GroupInfoModel GetGroupByClient(ClientInfoModel oClient)
        {
            try
            {
                GroupInfoModel oResult = new GroupInfoModel();
                DataTable oData = new DataTable();
                string sQry;

                sQry = "SELECT * FROM Gruppe WHERE Gruppe_ID = " + oClient.group.ToString();
                SqlCeDataAdapter oDataAdapter = new SqlCeDataAdapter(sQry, Connection);

                oDataAdapter.Fill(oData);

                foreach (DataRow oRow in oData.Rows)
                {
                    if (oData.Rows.Count == 1)
                    {
                        oResult.ID = Convert.ToInt32(oRow["Gruppe_ID"]);
                        oResult.Name = oRow["Gruppe_Name"].ToString();
                    }
                }
                return oResult;
            }
            catch (Exception ex)
            {
                Diagnostics.WriteToEventLog(ex.Message, System.Diagnostics.EventLogEntryType.Error, 3017);
                return null;
            }
        }
    }
}
