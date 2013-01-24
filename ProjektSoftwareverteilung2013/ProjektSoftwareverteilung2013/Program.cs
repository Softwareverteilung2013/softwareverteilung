using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjektSoftwareverteilung2013.Controller;
using ProjektSoftwareverteilung2013.Models;
using ProjektSoftwareverteilung2013.Datenbanken;

namespace ProjektSoftwareverteilung2013
{
    class Program
    {
        private static ServerConnection connection = null;
        static void Main(string[] args)
        {
            List<ClientInfoModel> olist = new List<ClientInfoModel>();
            LocalDB oDB = new LocalDB();

            Controller.Diagnostics.EventName = "Softwareverteilung2013";
            Controller.Diagnostics.WriteToEventLog("Server wird gestartet", System.Diagnostics.EventLogEntryType.Information, 1000);
            Console.WriteLine("starting...");

            connection = new ServerConnection();

            String cmd = "";
            while (!cmd.ToLower().Equals("stop"))
            {
                cmd = Console.ReadLine();
                if (!cmd.ToLower().Equals("stop"))
                    Console.WriteLine("Unbekannter Befehl: " + cmd);
            }

            connection.stopServer();
        }
    }
}
