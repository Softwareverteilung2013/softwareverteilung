using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjektSoftwareverteilung2013
{
    class Program
    {
        static void Main(string[] args)
        {

            Controller.Diagnostics.EventName = "Softwareverteilung2013";
            Controller.Diagnostics.WriteToEventLog("Server wird gestartet", System.Diagnostics.EventLogEntryType.Information, 1000);
            Console.WriteLine("starting...");

            String cmd = "";
            while (!cmd.ToLower().Equals("stop"))
            {
                cmd = Console.ReadLine();
                if (!cmd.ToLower().Equals("stop"))
                    Console.WriteLine("Unbekannter Befehl: " + cmd);
            }
        }
    }
}
