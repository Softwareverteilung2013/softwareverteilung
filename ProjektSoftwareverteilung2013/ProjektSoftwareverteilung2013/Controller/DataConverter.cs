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
                    ClientInfoModel client = new ClientInfoModel();

                    client.ID = Convert.ToInt32(oRow["Client_ID"]);
                    client.macAddress = oRow["Client_MacAdresse"].ToString();
                    client.group = Convert.ToInt32(oRow["Client_Gruppe"]);
                    client.arc = oRow["Client_Arc"].ToString();
                    client.admin = Convert.ToBoolean(oRow["Client_Administrator"]);
                    client.pcName = oRow["Client_PCName"].ToString();

                    oResult.Add(client);
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

                return oResult;
            }
            catch (Exception ex)
            {
                Diagnostics.WriteToEventLog(ex.Message, System.Diagnostics.EventLogEntryType.Error, 3015);
                return null;
            }
        }
    }
}
