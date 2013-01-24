using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlServerCe;
using ProjektSoftwareverteilung2013.Models;
using ProjektSoftwareverteilung2013.Controller;

namespace ProjektSoftwareverteilung2013.Datenbanken
{
    class LocalDB
    {
        private SqlCeConnection Connection { get; set; }
        public DataConverter Converter { get; set; }

        public LocalDB()
        {
            string dbfile = Directory.GetCurrentDirectory().ToString();
            dbfile = dbfile.Substring(0, dbfile.Length - 9);
            dbfile += "Datenbanken\\SoftwareDB.sdf";

            Connection = new SqlCeConnection("Datasource=" + dbfile);

            Converter = new DataConverter(Connection);
        }

        private void openConnection()
        {
            Connection.Open();
        }

        private void closeConnection()
        {
            Connection.Close();
        }

        public List<PackageInfoModel> CheckSoftwareClient(ClientInfoModel oClient)
        {
            //Prüfen welcher Client, welche Software hat.

            try
            {
                List<PackageInfoModel> oResult = new List<PackageInfoModel>();
                List<PackageInfoModel> groupPackages = new List<PackageInfoModel>();
                List<PackageInfoModel> clientInstalledPackages = new List<PackageInfoModel>();
                string sQry;
                SqlCeCommand sqlCmd = new SqlCeCommand();
                DataTable oData = new DataTable();

                sQry = "SELECT * FROM Client WHERE Client_MacAdresse = '" + oClient.macAddress + "'";

                // DataTable mit Daten füllen
                SqlCeDataAdapter oDataAdapter = new SqlCeDataAdapter(sQry, Connection);
                oDataAdapter.Fill(oData);

                // Alle Informationen über die MacAdresse ermitteln
                oClient = new ClientInfoModel();
                foreach (DataRow oRow in oData.Rows)
                {
                    if (oData.Rows.Count == 1)
                    {
                        oClient.ID = Convert.ToInt32(oRow["Client_ID"]);
                        oClient.macAddress = oRow["Client_MacAdresse"].ToString();
                        oClient.group = Convert.ToInt32(oRow["Client_Gruppe"]);
                        oClient.admin = Convert.ToBoolean(oRow["Client_Administrator"]);
                        oClient.arc = oRow["Client_Arc"].ToString();
                    }
                }

                // Alle Softwarepakete über die Gruppe ermitteln, die der Client haben soll.
                sQry = "    SELECT SP.Softwarepaket_ID, SP.Softwarepaket_Name " +
                       "           , SP.Softwarepaket_Groesse, SP.Softwarepaket_Arc " +
                       "      FROM Gruppe_Softwarepaket GS " + 
                       " LEFT JOIN Softwarepaket AS SP ON GS.Softwarepaket_ID = SP.Softwarepaket_ID " +
                       "     WHERE GS.Gruppe_ID = " + oClient.group;
                oData = new DataTable();
                oDataAdapter = new SqlCeDataAdapter(sQry, Connection);

                foreach (DataRow oRow in oData.Rows)
                {
                    PackageInfoModel oPackage = new PackageInfoModel();

                    oPackage.ID = Convert.ToInt32(oRow["Softwarepaket_ID"]);
                    oPackage.Name = oRow["Softwarepaket_Name"].ToString();
                    oPackage.size = Convert.ToInt32(oRow["Softwarepaket_Groesse"]);
                    oPackage.arc = oRow["Softwarepaket_Arc"].ToString();

                    groupPackages.Add(oPackage);
                }

                // Alle installierten Softwarepakete des Clients ermitteln
                sQry = "    SELECT SP.Softwarepaket_ID, SP.Softwarepaket_Name " +
                       "           , SP.Softwarepaket_Groesse, SP.Softwarepaket_Arc " +
                       "      FROM Client_Softwarepaket AS CP " +
                       " LEFT JOIN Softwarepaket AS SP ON CP.Softwarepaket_ID = SP.Softwarepaket_ID " +
                       "     WHERE CP.Client_ID = " + oClient.ID.ToString();
                oData = new DataTable();
                oDataAdapter = new SqlCeDataAdapter(sQry, Connection);

                foreach (DataRow oRow in oData.Rows)
                {
                    PackageInfoModel oPackage = new PackageInfoModel();

                    oPackage.ID = Convert.ToInt32(oRow["Softwarepaket_ID"]);
                    oPackage.Name = oRow["Softwarepaket_Name"].ToString();
                    oPackage.size = Convert.ToInt32(oRow["Softwarepaket_Groesse"]);
                    oPackage.arc = oRow["Softwarepaket_Arc"].ToString();

                    clientInstalledPackages.Add(oPackage);
                }

                // Listen abgleichen und in oResult speichern.
                for (int i = 0; i < clientInstalledPackages.Count; i++)
                {
                    for (int n = 0; n < groupPackages.Count; n++)
                    {
                        if (clientInstalledPackages[i].ID == groupPackages[n].ID)
                        {
                            continue;
                        }
                        oResult.Add(groupPackages[n]);
                    }
                }

                return oResult;
            }
            catch (Exception ex)
            {
                Diagnostics.WriteToEventLog(ex.Message, System.Diagnostics.EventLogEntryType.Error, 3102);
                return null;
            }
        }

        public GroupInfoModel gbAddGroup(GroupInfoModel oGroup)
        {
            try
            {
                string sQry;
                GroupInfoModel oResult = new GroupInfoModel();
                SqlCeCommand SQLCmd = new SqlCeCommand();

                if (oGroup.ID == -1)
                {
                    sQry = "INSERT INTO Gruppe(Gruppe_Name)" +
                           "VALUES ('" + oGroup.Name + "')";
                }
                else
                {
                    sQry = "UPDATE Gruppe SET Gruppe_Name = '" + oGroup.Name + "'";
                }

                SQLCmd.CommandText = sQry;
                SQLCmd.Connection = Connection;
                openConnection();
                SQLCmd.ExecuteNonQuery();
                closeConnection();

                // Bearbeiteten Datensatz ermitteln und im objekt speichern
                sQry = "SELECT * FROM Gruppe WHERE Gruppe_Name = '" + oGroup.Name.ToString() + "'";
                DataTable oData = new DataTable();
                SqlCeDataAdapter oDataAdapter = new SqlCeDataAdapter(sQry, Connection);

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
                Diagnostics.WriteToEventLog(ex.Message, System.Diagnostics.EventLogEntryType.Error, 3103);
                return null;
            }
        }

        public ClientInfoModel gbAddClient(ClientInfoModel oClient)
        {
            try
            {
                string sQry;
                ClientInfoModel oResult = new ClientInfoModel();
                SqlCeCommand SQLCmd = new SqlCeCommand();
                int nAdministrator;

                if (oClient.admin) nAdministrator = -1;
                else nAdministrator = 0;

                if (mbClientVorhanden(oClient.macAddress))
                {
                    sQry = " UPDATE Client SET " +
                           " Client_MacAdresse = '" + oClient.macAddress + "'" +
                           " , Client_Gruppe = " + oClient.group +
                           " , Client_Administrator = " + nAdministrator +
                           " , Client_Arc = '" + oClient.arc + "'";
                }
                else
                {
                    sQry = "INSERT INTO Client(Client_MacAdresse, Client_Gruppe, Client_Administrator, Client_Arc)" +
                           "VALUES ('" + oClient.macAddress + "', " + oClient.group + ", " + nAdministrator + ", " + oClient.arc + ")";
                }

                SQLCmd.CommandText = sQry;
                SQLCmd.Connection = Connection;
                openConnection();
                SQLCmd.ExecuteNonQuery();
                closeConnection();

                sQry = "SELECT * FROM Client WHERE Client_MacAdresse = '" + oClient.macAddress + "'";
                SqlCeDataAdapter oDataAdapter = new SqlCeDataAdapter(sQry, Connection);
                DataTable oData = new DataTable();

                oDataAdapter.Fill(oData);

                foreach (DataRow oRow in oData.Rows)
                {
                    if (oData.Rows.Count == 1)
                    {
                        oResult.ID = Convert.ToInt32(oRow["Client_ID"]);
                        oResult.admin = Convert.ToBoolean(oRow["Client_Administrator"]);
                        oResult.arc = oRow["Client_Arc"].ToString();
                        oResult.group = Convert.ToInt32(oRow["Client_Gruppe"]);
                        oResult.macAddress = oRow["Client_MacAdresse"].ToString();
                    }
                }

                return oResult;
            }
            catch (Exception ex)
            {
                Diagnostics.WriteToEventLog(ex.Message, System.Diagnostics.EventLogEntryType.Error, 3104);
                return null;
            }
        }

        public PackageInfoModel gbAddPackage(PackageInfoModel oPackage)
        {
            // Prüfen ob package schon angelegt ist.
            // GUID, Klartext, Groesse, arc
            try
            {
                string sQry;
                PackageInfoModel oResult = new PackageInfoModel();
                SqlCeCommand SQLCmd = new SqlCeCommand();

                return oResult;
            }
            catch (Exception ex)
            {
                Diagnostics.WriteToEventLog(ex.Message, System.Diagnostics.EventLogEntryType.Error, 3105);
                return null;
            }
        }

        public bool gbDeleteClient(ClientInfoModel oClient)
        {
            try
            {
                string sQry;

                sQry = "DELETE FROM Client WHERE Client_ID = " + oClient.ID;

                SqlCeCommand sqlCmd = new SqlCeCommand(sQry, Connection);
                openConnection();
                sqlCmd.ExecuteNonQuery();
                closeConnection();

                return true;
            }
            catch (Exception ex)
            {
                Diagnostics.WriteToEventLog(ex.Message, System.Diagnostics.EventLogEntryType.Error, 3107);
                return false;
            }
        }

        public bool gbDeleteGroup(GroupInfoModel oGroup)
        {
            try
            {
                string sQry;

                sQry = "DELETE FROM Gruppe WHERE Gruppe_ID = " + oGroup.ID;

                SqlCeCommand sqlCmd = new SqlCeCommand(sQry, Connection);
                openConnection();
                sqlCmd.ExecuteNonQuery();
                closeConnection();

                return true;
            }
            catch (Exception ex)
            {
                Diagnostics.WriteToEventLog(ex.Message, System.Diagnostics.EventLogEntryType.Error, 3107);
                return false;
            }
        }

        public bool gbDeletePackage(PackageInfoModel oPackage)
        {
            try
            {
                string sQry;

                sQry = "DELETE FROM Softwarepaket WHERE Softwarepaket_ID = " + oPackage.ID;

                SqlCeCommand sqlCmd = new SqlCeCommand(sQry, Connection);
                openConnection();
                sqlCmd.ExecuteNonQuery();
                closeConnection();

                return true;
            }
            catch (Exception ex)
            {
                Diagnostics.WriteToEventLog(ex.Message, System.Diagnostics.EventLogEntryType.Error, 3107);
                return false;
            }
        }

        private bool mbClientVorhanden(string macadress)
        {
            string sQry;
            DataTable oData = new DataTable();

            sQry = "SELECT * FROM Client WHERE Client_MacAdresse = '" + macadress + "'";
            SqlCeDataAdapter oDataAdapter = new SqlCeDataAdapter(sQry, Connection);
            oDataAdapter.Fill(oData);

            if (oData.Rows.Count > 0) return true;
            else return false;
        }
    }
}
