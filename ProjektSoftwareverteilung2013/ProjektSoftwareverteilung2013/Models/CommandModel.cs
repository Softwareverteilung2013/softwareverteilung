using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjektSoftwareverteilung2013.Models
{
    class CommandModel
    {
        private static string clients = "clients         Gibt alle in der Datenbank geschriebene Clients zurück\n";
        private static string groups = "groups          Gibt alle in der Datenbank geschriebene Gruppen zurück\n";
        private static string packages = "packages        Gibt alle in der Datenbank geschriebene Pakete zurück\n";
        private static string stop = "stop            Beendet alle laufende Threads und Verbindungen.\n";
        private static string delGroup = "delGroup        Löscht die übergebene Gruppe (Group Name)\n";
        private static string clear = "clear           Bereinigt die Konsole";
        private static string delPackage = "delPackage      Löscht das übergeben Paket (Package Name | Package ID)\n";
        private static string groupClients = "GroupClients    Gibt alle Clients der Grouppe zurück";

        public static string getCommands()
        {
            string commands = clients + groups + groupClients + packages + delGroup + delPackage + clear + stop;
            return commands;
        }

    }
}
