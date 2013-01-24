using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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

        public bool CheckSoftwareClient(ClientInfoModel oClient, PackageInfoModel oPackage)
        {
            //Prüfen welcher Client, welche Software hat.

            return true;
        }

        public bool gbAddGroup(GroupInfoModel oGroup)
        {
            try
            {
                string sQry;
                SqlCeCommand SQLCmd = new SqlCeCommand();

                sQry = "INSERT INTO Gruppe(Gruppe_Name)" +
                       "VALUES ('" + oGroup.Name + "')";

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

        public bool gbAddClient(ClientInfoModel oClient)
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

        public bool gbAddPackage(PackageInfoModel oPackage)
        {
            try
            {
                string sQry;
                SqlCeCommand SQLCmd = new SqlCeCommand();
            }
            catch (Exception ex)
            {
                Diagnostics.WriteToEventLog(ex.Message, System.Diagnostics.EventLogEntryType.Error, 3105);
            }

            return true;
        }

        public bool gbUpdateClient(ClientInfoModel oClient)
        {
            return true;
        }

        public bool gbUpdateGroup(GroupInfoModel oGroup)
        {
            return true;
        }

        public bool gbUpdatePackage(PackageInfoModel oPackage)
        {
            return true;
        }

        public bool gbDeleteClient(ClientInfoModel oClient)
        {
            return true;
        }

        public bool gbDeleteGroup(GroupInfoModel oGroup)
        {
            return true;
        }

        public bool gbDeletePackage(PackageInfoModel oPackage)
        {
            return true;
        }
    }
}
