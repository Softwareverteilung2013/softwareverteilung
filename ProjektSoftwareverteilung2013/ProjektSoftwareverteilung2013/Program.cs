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
            //List<ClientInfoModel> olist = new List<ClientInfoModel>();
            LocalDB oDB = new LocalDB();
            FileController file = new FileController(oDB.Converter.GetGroupInfoModels());

            Controller.Diagnostics.EventName = "Softwareverteilung2013";
            Controller.Diagnostics.WriteToEventLog("Server wird gestartet", System.Diagnostics.EventLogEntryType.Information, 1000);
            Console.WriteLine("starting...");

            connection = new ServerConnection();

            String cmd = "";
            while (cmd.ToLower() != "stop")
            {
                cmd = Console.ReadLine();

                if (cmd.ToLower().Equals("help"))
                {
                    string commands = CommandModel.getCommands();
                    Console.WriteLine(commands);
                }
                else if (cmd.ToLower().Equals("clear"))
                {
                    Console.Clear();
                }
                else if (cmd.ToLower().Equals("delPackage"))
                {
                    Console.WriteLine("Noch nicht Implementiert");
                }
                else if (cmd.ToLower().Equals("delGroup"))
                {
                    Console.WriteLine("Noch nicht Implementiert");
                }
                else if (cmd.ToLower().Equals("packages"))
                {
                    Console.WriteLine("Noch nicht Implementiert");
                }
                else if (cmd.ToLower().Equals("groups"))
                {
                    List<GroupInfoModel> groups = oDB.Converter.GetGroupInfoModels();

                    string groupNames = "";

                    foreach (var group in groups)
                    {
                        groupNames = "ID:" + group.ID + " Name:" + group.Name;
                        Console.WriteLine(groupNames);
                    }
                }
                else if (cmd.ToLower().Equals("groupclients"))
                {
                    List<GroupInfoModel> groups = oDB.Converter.GetGroupInfoModels();

                    for (int i = 0; i < groups.Count; i++)
                    {
                        Console.WriteLine("ID:" + groups[i].ID + " Name:" + groups[i].Name);
                        Console.WriteLine();

                        List<ClientInfoModel> clients = oDB.Converter.GetGroupClients(groups[i]);

                        string groupNames = "";

                        foreach (var client in clients)
                        {
                            groupNames = "ID:" + client.ID + " pcName:" + client.pcName;
                            Console.WriteLine(groupNames);
                        }
                        Console.WriteLine();
                    }
                    
                }
                else if (cmd.ToLower().Equals("clients"))
                {
                    List<ClientInfoModel> oList = oDB.Converter.GetClientInfoModels();

                    for (int i = 0; i < oList.Count; i++)
                    {
                        Console.WriteLine(oList[i].pcName.ToString() + "   " + oList[i].group.ToString());
                    }
                }
                else if (!cmd.ToLower().Equals("stop"))
                    Console.WriteLine("Unbekannter Befehl: " + cmd);
            }

            connection.stopServer();
        }
    }
}
