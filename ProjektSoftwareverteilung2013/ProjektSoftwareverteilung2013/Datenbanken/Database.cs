using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ProjektSoftwareverteilung2013.Datenbanken
{
    class Database
    {
        private SqlConnection _Connection;
        private string _MSSQLServer;
        private string _MSSQLDatabase;
        private string _MSSQLUserID;
        private string _MSSQLPassword;

        public SqlConnection Connection
        {
            get { return _Connection; }
        }

        public string MSSQLServer
        {
            get { return _MSSQLServer; }
            set { _MSSQLServer = value; }
        }

        public string MSSQLDatabase
        {
            get { return _MSSQLDatabase; }
            set { _MSSQLDatabase = value; }
        }

        public string MSSQLUserID
        {
            get { return _MSSQLUserID; }
            set { _MSSQLUserID = value; }
        }

        public string MSSQLPassword
        {
            get { return _MSSQLPassword; }
            set { _MSSQLPassword = value; }
        }

        /// <summary>
        /// Stellt eine MSSQL-Datenbankverbindung her. Vier Parameter werden benötigt.
        /// </summary>
        /// <param name="Server">Name des Datenbankservers.</param>
        /// <param name="Database">Name der Datenbank.</param>
        /// <param name="UserID">Benutzername für die Anmeldung am SQL-Server.</param>
        /// <param name="Password">Passwort für den eingegebenen Benutzer.</param>
        public Database(string Server, string Database, string UserID, string Password)
        {
            this._MSSQLServer = Server;
            this._MSSQLDatabase = Database;
            this._MSSQLUserID = UserID;
            this._MSSQLPassword = Password;

            _Connection = new SqlConnection("Server=" + _MSSQLServer + ";Database=" + _MSSQLDatabase + ";User Id=" + _MSSQLUserID + ";Password=" + _MSSQLPassword + ";");
        }

        /// <summary>
        /// Erwartet wird eine Abfrage, welche nur einen Datensatz zurückgibt, um einen Wert für ein Feld abzufragen.
        /// </summary>
        /// <param name="sField">Nur ein Feld angeben.</param>
        /// <param name="sTable">Name der Tabelle.</param>
        /// <param name="sClause">Bedingung der Abfrage.</param>
        /// <returns>object</returns>
        public object goLookup(string sField, string sTable, string sClause)
        {
            object oResult;
            string sQry;
            SqlDataAdapter oDataAdapter;
            DataTable oData = new DataTable() ;

            sQry = " SELECT " + sField + " FROM " + sTable + " WHERE " + sClause;
            oDataAdapter = new SqlDataAdapter(sQry, _Connection);
            oDataAdapter.Fill(oData);

            if (oData.Rows.Count == 1)
            {
                if (oData.Rows[0][0].ToString().Length > 0)
                {
                    oResult = oData.Rows[0][0];
                    return oResult;
                }
            }

            return "";
        }

        /// <summary>
        /// Erstellt eine Abfrage, befüllt ein DataTable mit dem Resultat der Abfrage und gibt das DataTable zurück.
        /// </summary>
        /// <param name="sField">Felder, die selektiert werden sollen.</param>
        /// <param name="sTable">Tabelle, auf die zugegriffen werden soll.</param>
        /// <param name="sClause">OPTIONAL: Bedingung der Abfrage.</param>
        /// <returns>DataTable</returns>
        public DataTable GetTable(string sField, string sTable, string sClause)
        {
            string sQry;
            DataTable oResult = new DataTable();
            SqlDataAdapter oDataAdapter;

            if (sClause.Length > 0) sQry = " SELECT " + sField + " FROM " + sTable + " WHERE " + sClause;
            else sQry = " SELECT " + sField + " FROM " + sTable;

            oDataAdapter = new SqlDataAdapter(sQry, _Connection);
            oDataAdapter.Fill(oResult);

            return oResult;
        }

        /// <summary>
        /// Führt einen Insert-Befehl auf der bestehenden Datenbankverbindung aus.
        /// </summary>
        /// <param name="sFields">Einzufügende Felder kommasepariert angeben.</param>
        /// <param name="sTable">Tabelle, in welche der Datensatz angelegt werden soll.</param>
        /// <param name="sValues">Werte für die angegebenen Felder. ACHTUNG: Gleiche Reihenfolge, wie Felder.</param>
        public void cmdInsert(string sFields, string sTable, string sValues)
        {
            string sQry;
            SqlCommand sqlCmd = new SqlCommand();

            sQry = "INSERT INTO " + sTable + "(" + sFields + ")VALUES(" + sValues + ")";
            sqlCmd.Connection = _Connection;
            sqlCmd.CommandText = sQry;

            sqlCmd.Connection.Open();
            sqlCmd.ExecuteNonQuery();
            sqlCmd.Connection.Close();
        }

        /// <summary>
        /// Führt einen DELETE-Befehl aus.
        /// </summary>
        /// <param name="sTable">Tabelle, der zu löschenden Datensätze.</param>
        /// <param name="sClause">Bedingung für den DELETE-Befehl.</param>
        public void cmdDelete(string sTable, string sClause)
        {
            string sQry;
            SqlCommand sqlCmd = new SqlCommand();

            sQry = "DELETE FROM " + sTable + " WHERE " + sClause;
            sqlCmd.Connection = _Connection;
            sqlCmd.CommandText = sQry;

            sqlCmd.Connection.Open();
            sqlCmd.ExecuteNonQuery();
            sqlCmd.Connection.Close();
        }

        /// <summary>
        /// Führt einen UPDATE-Befehl aus.
        /// </summary>
        /// <param name="sTable">Zu ändernde Tabelle.</param>
        /// <param name="sField">Zu ändernde Felder.</param>
        /// <param name="sClause">Bedingung der Abfrage.</param>
        public void cmdUpdate(string sTable, string sField, string sClause)
        {
            string sQry;
            SqlCommand sqlCmd = new SqlCommand();

            sQry = "UPDATE " + sTable + " SET " + sField + " WHERE " + sClause;
            sqlCmd.Connection = _Connection;
            sqlCmd.CommandText = sQry;

            sqlCmd.Connection.Open();
            sqlCmd.ExecuteNonQuery();
            sqlCmd.Connection.Close();
        }
    }
}
