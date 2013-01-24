using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;
using ProjektSoftwareverteilung2013.Models;
using System.Text.RegularExpressions;
using ProjektSoftwareverteilung2013.Datenbanken;

namespace ProjektSoftwareverteilung2013.Controller
{
    class ServerConnection
    {
        private TcpListener listener = null;
        private List<ServerThread> threads = new List<ServerThread>();
        private Thread th = null;

        public ServerConnection()
        {
            IPAddress ipAddress = getIpAddressFromString("127.0.0.1");
            int port = 5555;
            listener = new TcpListener(ipAddress, port);

            Console.WriteLine("Open listener with Port:" + port);

            listener.Start();
            th = new Thread(new ThreadStart(RunNewConnection));
            th.Start();
        }


        private IPAddress getIpAddressFromString(string ipAddress)
        {
            IPAddress mIpAdress = IPAddress.Parse(ipAddress);
            return mIpAdress;
        }

        private void RunNewConnection()
        {
            while (true)
            {
                TcpClient c = listener.AcceptTcpClient();
                threads.Add(new ServerThread(c));
            }
        }

        public void stopServer()
        {
            th.Abort();
            this.stopAllConnections();
            Console.WriteLine("Alle Verbindungen wurden beendet");
            listener.Stop();
        }

        private void stopAllConnections()
        {
            for (IEnumerator<ServerThread> e = threads.GetEnumerator(); e.MoveNext(); )
            {
                ServerThread st = (ServerThread)e.Current;
                st.stopConnection();
            }
        }
    }

    class ServerThread
    {
        private TcpClient tcpConnection = null;
        private Stream connectionStream = null;

        public ServerThread(TcpClient connection)
        {
            LocalDB dataBase = null;
            tcpConnection = connection;
            connectionStream = tcpConnection.GetStream();

            StandardRequestModel request = readStream();
            if (request.Equals(""))
            {
                stopConnection();
            }
            ClientInfoModel Client = request.Client;

            //Aufruf der Datenbank speichern des Clients
            if (Client == null)
            {
                stopConnection();
            }
            loginClient(Client);

            //Verarbeitung des Request
            StandardResultModel result = getResult(request);

            sendStringStream(result);

            if (result.type == ResultType.sendPackage)
            {
                List<PackageInfoModel> packageList = (List<PackageInfoModel>)result.result;
                GroupInfoModel group = null;
                dataBase = new LocalDB();

                if (packageList.Count != 0)
                {
                    ClientInfoModel resultClient = (ClientInfoModel)result.result;

                    if (resultClient != null)
                    {
                        sendFile(group, packageList[0]);
                        dataBase.ClientGetSoftware(resultClient, packageList[0]); 
                    }
                }
            }

            if (result.type == ResultType.readPackage)
            {
                string strInfo =(string)request.requestData;
                char[] delimiter1 = new char[] { ',' };

                string[] strArray = strInfo.Split(delimiter1, StringSplitOptions.None);

                if (strArray.Length == 2)
                {
                    readFile(strArray[0], strArray[1]);
                }
                
            }

            stopConnection();
        }

        private StandardRequestModel readStream()
        {
            StandardRequestModel request = null;
            string message = null;
            int len;

            byte[] readingBytes = new byte[1024 * 100];
            
            if (!connectionStream.CanRead)
            {
                return null;
            }

            try
            {
                while ((len = connectionStream.Read(readingBytes, 0, readingBytes.Length)) > 0)
                {
                    break;
                }
            }
            catch (Exception ex)
            {

                Diagnostics.WriteToEventLog(ex.Message, EventLogEntryType.Error, 3000);
            }
            

            message = Encoding.ASCII.GetString(readingBytes);
            string newString = message.Trim();
            Console.WriteLine(newString);

            request = JsonConvert.DeserializeObject<StandardRequestModel>(newString);
            return request;
        }

        private void sendStringStream(StandardResultModel message)
        {
            string result = JsonConvert.SerializeObject(message);
            if (!connectionStream.CanWrite)
            {
                return;
            }

            try
            {
                Byte[] sendBytes = Encoding.ASCII.GetBytes(result);
                connectionStream.Write(sendBytes, 0, sendBytes.Length);
            }
            catch (Exception ex)
            {
                Diagnostics.WriteToEventLog(ex.Message, EventLogEntryType.Error, 3001);
            }

            //this.stopConnection();
        }

        private void readFile(string strGroupName, string strPackageName)
        {
            Socket clientSock = tcpConnection.Client;
            byte[] file = new byte[1024 * 5000];

            FileController fileController = new FileController();
            string receivedPath = fileController.getPathFromFile(strGroupName, strPackageName);

            int receivedBytesLen = clientSock.Receive(file);
            int fileNameLen = BitConverter.ToInt32(file, 0);
            string fileName = Encoding.ASCII.GetString(file, 4, fileNameLen);

            Console.WriteLine("Client:{0} connected & File {1} started received.", clientSock.RemoteEndPoint, fileName);
            BinaryWriter bWrite = new BinaryWriter(File.Open(receivedPath + fileName, FileMode.Append));

            try
            {
                bWrite.Write(file, 4 + fileNameLen, receivedBytesLen - 4 - fileNameLen);
            }
            catch (Exception ex)
            {
                Diagnostics.WriteToEventLog(ex.Message, EventLogEntryType.Error, 3002);
            }
            
            bWrite.Close();

            Console.WriteLine("File: {0} received & saved at path: {1}", fileName, receivedPath);

        }

        private void sendFile(GroupInfoModel group, PackageInfoModel package)
        {
            FileController fileController = new FileController();
            string filePath = fileController.getPathFromFile(group.Name, package.Name);

            if (!File.Exists(filePath))
            {
                return;
            }

            byte[] fileName = Encoding.ASCII.GetBytes(package.Name);
            byte[] fileData = File.ReadAllBytes(filePath);
            byte[] fileNameLen = BitConverter.GetBytes(fileData.Length);
            byte[] file = new byte[4 + fileName.Length + fileNameLen.Length];

            fileNameLen.CopyTo(file, 0);
            fileName.CopyTo(file, 4);
            fileData.CopyTo(file, 4 + fileName.Length);

            try
            {
                tcpConnection.Client.Send(file);
                Console.WriteLine("File: {0} has been sent.", fileName);
            }
            catch (Exception ex)
            {
                Diagnostics.WriteToEventLog(ex.Message, EventLogEntryType.Error, 3003);
            }

            stopConnection();

        }

        private StandardResultModel getResult(StandardRequestModel request)
        {
            StandardResultModel result = new StandardResultModel();
            List<ClientInfoModel> clientList = null;
            List<GroupInfoModel> groupList = null;
            List<PackageInfoModel> packageList = null;

            ClientInfoModel client = null;
            GroupInfoModel group = null;
            PackageInfoModel package = null;

            LocalDB dataBase = new LocalDB();
            bool success = false;
            result.successful = success;

            switch (request.request)
            {
                case RequestTyp.upDateRequest:

                    packageList = dataBase.CheckSoftwareClient(request.Client);
                    result.successful = true;
                    if (packageList.Count == 0)
                    {
                        result.type = ResultType.defaultInfo;
                    }
                    else
                    {
                        result.type = ResultType.sendPackage;

                    }

                    result.result = packageList;
                    break;

                case RequestTyp.getDatabaseGroups:

                    groupList = dataBase.Converter.GetGroupInfoModels();

                    result.message = "";
                    result.result = groupList;
                    result.successful = true;
                    result.type = ResultType.GroupInfo;
                    break;

                case RequestTyp.getDatabaseClients:

                    clientList = dataBase.Converter.GetClientInfoModels();

                    result.message = "";
                    result.result = clientList;
                    result.successful = true;
                    result.type = ResultType.ClientInfo;
                    break;

                case RequestTyp.getDatabaseSoftwarePackages:

                    packageList = dataBase.Converter.GetPackageInfoModels();

                    result.message = "";
                    result.result = packageList;
                    result.successful = true;
                    result.type = ResultType.SoftwarePackagesInfo;
                    break;

                case RequestTyp.getGroupClients:

                    clientList = dataBase.Converter.GetGroupClients((GroupInfoModel)request.requestData);

                    result.message = "";
                    result.result = clientList;
                    result.successful = true;
                    result.type = ResultType.GroupClients;
                    break;

                case RequestTyp.getGrupePackages:

                    packageList = dataBase.Converter.GetGroupPackages((GroupInfoModel)request.requestData);

                    result.message = "";
                    result.result = packageList;
                    result.successful = true;
                    result.type = ResultType.GrupePackages;
                    break;

                case RequestTyp.getClientPackages:

                    packageList = dataBase.Converter.GetClientPackages((ClientInfoModel)request.requestData);

                    result.message = "";
                    result.result = packageList;
                    result.successful = true;
                    result.type = ResultType.ClientPackages;
                    break;

                case RequestTyp.addDatabaseClient:

                    client = dataBase.gbAddClient((ClientInfoModel)request.requestData);

                    result.message = "";
                    result.result = client;
                    result.successful = true;
                    result.type = ResultType.addClient;
                    break;
                case RequestTyp.addDatabaseGroup:

                    group = dataBase.gbAddGroup((GroupInfoModel)request.requestData);

                    result.message = "";
                    result.result = group;
                    result.successful = true;
                    result.type = ResultType.addGroup;
                    break;

                case RequestTyp.addDatabaseSoftwarePackage:

                    package = dataBase.gbAddPackage((PackageInfoModel)request.requestData);

                    result.message = "";
                    result.result = package;
                    result.successful = true;
                    result.type = ResultType.addPackage;
                    break;

                case RequestTyp.delDatabaeClient:

                    result.successful = dataBase.gbDeleteClient((ClientInfoModel)request.requestData);

                    result.message = "";
                    result.result = package;
                    result.type = ResultType.delDatabaeClient;
                    break;

                case RequestTyp.delDatabaseGroup:

                    result.successful = dataBase.gbDeleteGroup((GroupInfoModel)request.requestData);

                    result.message = "";
                    result.result = package;
                    result.type = ResultType.delDatabaseGroup;
                    break;
                case RequestTyp.delDatabaseSoftwarePackage:

                    result.successful = dataBase.gbDeletePackage((PackageInfoModel)request.requestData);

                    result.message = "";
                    result.result = package;
                    result.type = ResultType.delDatabaseSoftwarePackage;
                    break;
                case RequestTyp.sendSoftwarePackage:

                    clientList = dataBase.Converter.GetClientInfoModels();

                    for (int i = 0; i < clientList.Count; i++)
                    {
                        if (request.Client.macAddress == clientList[i].macAddress)
                        {
                            client = clientList[i];
                        }
                    }

                    result.message = "";
                    result.successful = true;
                    result.type = ResultType.readPackage;
                    result.result = client;
                    break;

                default:
                    break;
            }
            return result;
        }

        private void loginClient(ClientInfoModel client)
        {
            Console.WriteLine("Anmeldung Client:" + client.macAddress + " Gruppe:" + client.group + " Admin:" + client.admin);
        }

        public void stopConnection()
        {
            tcpConnection.Close();
        }

    }
}
