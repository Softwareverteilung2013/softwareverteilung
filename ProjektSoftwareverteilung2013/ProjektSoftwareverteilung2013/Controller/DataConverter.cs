using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Data.SqlServerCe;
using ProjektSoftwareverteilung2013.Controller;
using ProjektSoftwareverteilung2013.Models;

namespace ProjektSoftwareverteilung2013.Controller
{
    class DataConverter
    {
        public SqlCeConnection Connection { get; set; }

        public DataConverter()
        {
            string dbfile = Directory.GetCurrentDirectory().ToString();
            dbfile = dbfile.Substring(0, dbfile.Length - 9);
            dbfile += "Datenbanken\\SoftwareDB.sdf";

            Connection = new SqlCeConnection("Datasource=" + dbfile);
        }

        public List<ClientInfoModel> GetClientInfoModels()
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

                    oClient.admin = Convert.ToBoolean(oRow["Client_Administrator"]);
                    oClient.group = Convert.ToInt32(oRow["Client_Gruppe"]);
                    oClient.ID = Convert.ToInt32(oRow["Client_ID"]);
                    oClient.arc = Convert.ToString(oRow["Client_Arc"]);

                    oResult.Add(oClient);
                }
            }
            return oResult;
        }

        public List<GroupInfoModel> GetGroupInfoModel()
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
                    oGroup.PackageID = Convert.ToInt32(oRow["Gruppe_Softwarepaket_ID"]);

                    oResult.Add(oGroup);
                }
            }

            return oResult;
        }

        public List<PackageInfoModel> GetPackageInfoModel()
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
                    oPackage.arc = oRow["Software_Arc"].ToString();

                    oResult.Add(oPackage);
                }
            }
            
            return oResult;
        }

        public bool gbInsertGroup(GroupInfoModel oGroup)
        {
            try
            {
                string sQry;
                SqlCeCommand SQLCmd = new SqlCeCommand();

                sQry = "INSERT INTO Gruppe(Gruppe_Name, Softwarepaket_ID)" +
                       "VALUES ('" + oGroup.Name + "', " + oGroup.PackageID + ")";

                SQLCmd.CommandText = sQry;
                SQLCmd.Connection = Connection;
                SQLCmd.Connection.Open();
                SQLCmd.ExecuteNonQuery();
                SQLCmd.Connection.Close();

                return true;
            }
            catch (Exception ex)
            {
                Diagnostics.WriteToEventLog(ex.Message, System.Diagnostics.EventLogEntryType.Error, 3103);
                return false;
            }
        }

        public bool gbInsertClient(ClientInfoModel oClient)
        {
            try
            {
                string sQry;
                SqlCeCommand SQLCmd = new SqlCeCommand();
                int nAdministrator;

                if (oClient.admin) nAdministrator = -1;
                else nAdministrator = 0;

                sQry = "INSERT INTO Client(Client_MacAdresse, Client_Gruppe, Client_Administrator, Client_Arc)" +
                       "VALUES ('" + oClient.macAddress + "', " + oClient.group + ", " + nAdministrator + ", " + oClient.arc + ")";

                SQLCmd.CommandText = sQry;
                SQLCmd.Connection = Connection;
                SQLCmd.Connection.Open();
                SQLCmd.ExecuteNonQuery();
                SQLCmd.Connection.Close();

                return true;
            }
            catch (Exception ex)
            {
                Diagnostics.WriteToEventLog(ex.Message, System.Diagnostics.EventLogEntryType.Error, 3104);
                return false;
            }
        }
    }
}
