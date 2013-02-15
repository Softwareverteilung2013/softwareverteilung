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
            //IPAddress mIpAdress = IPAddress.Parse(ipAddress);
            IPAddress mIpAdress = IPAddress.Any;
            return mIpAdress;
        }

        private void RunNewConnection()
        {
            Console.WriteLine("Wartet auf Client");
            while (true)
            {
                TcpClient c = listener.AcceptTcpClient();
                Console.WriteLine("Client " + c.Client.RemoteEndPoint.ToString().Substring(0,c.Client.RemoteEndPoint.ToString().Length - 6) + " hat sich mit dem Server verbunden");
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
        FileController fileController = null;
        LocalDB dataBase = null;

        public ServerThread(TcpClient connection)
        {
            dataBase = new LocalDB();
            fileController = new FileController();
            tcpConnection = connection;
            connectionStream = tcpConnection.GetStream();

            StandardRequestModel request = readStream();
            if (request == null)
            {
                stopConnection();
                return;
            }
            ClientInfoModel Client = request.Client;

            if (Client == null)
            {
                stopConnection();
            }
            loginClient(request);

            //Verarbeitung des Request
            StandardResultModel result = getResult(request);

            sendStringStream(result);

            if (result.type == ResultType.sendPackage)
            {
                List<PackageInfoModel> packageList = (List<PackageInfoModel>)result.result;
                GroupInfoModel group = null;

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
            byte[] ClientData = new byte[1024 * 5000];

            FileController fileController = new FileController();
            string receivedPath = fileController.getPathFromGroup(strGroupName);

            int receivedBytesLen = clientSock.Receive(ClientData);
            int fileNameLen = BitConverter.ToInt32(ClientData, 0);
            string fileName = Encoding.ASCII.GetString(ClientData, 4, fileNameLen);

            Console.WriteLine("Client:{0} connected & File {1} started received.", clientSock.RemoteEndPoint, fileName);
            BinaryWriter bWrite = new BinaryWriter(File.Open(receivedPath + fileName, FileMode.Append));

            try
            {
                bWrite.Write(ClientData, 4 + fileNameLen, receivedBytesLen - 4 - fileNameLen);
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
            byte[] fileNameLen = BitConverter.GetBytes(fileName.Length);
            byte[] file = new byte[4 + fileName.Length + fileData.Length];

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
            StandardResultModel result = null;
            switch (request.request)
            {
                case RequestTyp.upDateRequest:
                    result = upDateRequest(request);
                    break;

                case RequestTyp.getDatabaseGroups:
                    result = getDatabaseGroups(request);
                    break;

                case RequestTyp.getDatabaseClients:
                    result = getDatabaseClients(request);
                    break;

                case RequestTyp.getDatabaseSoftwarePackages:
                    result = getDatabaseSoftwarePackages(request);
                    break;

                case RequestTyp.getGroupClients:
                    result = getGroupClients(request);
                    break;

                case RequestTyp.getGrupePackages:
                    result = getGrupePackages(request);
                    break;

                case RequestTyp.getClientPackages:
                    result = getClientPackages(request);
                    break;

                case RequestTyp.addDatabaseClient:
                    result = addDatabaseClient(request);
                    break;
                case RequestTyp.addDatabaseGroup:
                    result = addDatabaseGroup(request);
                    break;

                case RequestTyp.addDatabaseSoftwarePackage:
                    result = addDatabaseSoftwarePackage(request);
                    break;

                case RequestTyp.delDatabaeClient:
                    result = delDatabaeClient(request);
                    break;

                case RequestTyp.delDatabaseGroup:
                    result = delDatabaseGroup(request);
                    break;

                case RequestTyp.delDatabaseSoftwarePackage:
                    result = delDatabaseSoftwarePackage(request);
                    break;

                case RequestTyp.sendSoftwarePackage:
                    result = sendSoftwarePackage(request);
                    break;

                default:
                    break;
            }
            return result;
        }

        private StandardResultModel upDateRequest(StandardRequestModel request)
        {
            StandardResultModel result = new StandardResultModel();
            List<PackageInfoModel> packageList = null;

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

            return result;
        }

        private StandardResultModel getDatabaseGroups(StandardRequestModel request)
        {
            StandardResultModel result = new StandardResultModel();
            List<GroupInfoModel> groupList = null;

            groupList = dataBase.Converter.GetGroupInfoModels();

            result.message = "";
            result.result = groupList;
            result.successful = true;
            result.type = ResultType.GroupInfo;

            return result;
        }

        private StandardResultModel getDatabaseClients(StandardRequestModel request)
        {
            StandardResultModel result = new StandardResultModel();
            List<ClientInfoModel> clientList = null;

            clientList = dataBase.Converter.GetClientInfoModels();

            result.message = "";
            result.result = clientList;
            result.successful = true;
            result.type = ResultType.ClientInfo;

            return result;
        }

        private StandardResultModel getDatabaseSoftwarePackages(StandardRequestModel request)
        {
            StandardResultModel result = new StandardResultModel();
            List<PackageInfoModel> packageList = null;

            packageList = dataBase.Converter.GetPackageInfoModels();

            result.message = "";
            result.result = packageList;
            result.successful = true;
            result.type = ResultType.SoftwarePackagesInfo;

            return result;
        }

        private StandardResultModel getGroupClients(StandardRequestModel request)
        {
            StandardResultModel result = new StandardResultModel();
            GroupInfoModel groupResult = null;
            List<ClientInfoModel> clientList = null;

            groupResult = JsonConvert.DeserializeObject<GroupInfoModel>(request.requestData.ToString());
            clientList = dataBase.Converter.GetGroupClients(groupResult);

            result.message = "";
            result.result = clientList;
            result.successful = true;
            result.type = ResultType.GroupClients;

            return result;
        }

        private StandardResultModel getGrupePackages(StandardRequestModel request)
        {
            StandardResultModel result = new StandardResultModel();
            GroupInfoModel groupResult = null;
            List<PackageInfoModel> packageList = null;

            groupResult = JsonConvert.DeserializeObject<GroupInfoModel>(request.requestData.ToString());
            packageList = dataBase.Converter.GetGroupPackages(groupResult);

            result.message = "";
            result.result = packageList;
            result.successful = true;
            result.type = ResultType.GrupePackages;

            return result;
        }

        private StandardResultModel getClientPackages(StandardRequestModel request)
        {
            StandardResultModel result = new StandardResultModel();
            ClientInfoModel clientResult = null;
            List<PackageInfoModel> packageList = null;

            clientResult = JsonConvert.DeserializeObject<ClientInfoModel>(request.requestData.ToString());
            packageList = dataBase.Converter.GetClientPackages(clientResult);

            result.message = "";
            result.result = packageList;
            result.successful = true;
            result.type = ResultType.ClientPackages;

            return result;
        }

        private StandardResultModel addDatabaseClient(StandardRequestModel request)
        {
            StandardResultModel result = new StandardResultModel();
            ClientInfoModel clientResult = null;
            ClientInfoModel client = null;

            clientResult = JsonConvert.DeserializeObject<ClientInfoModel>(request.requestData.ToString());
            client = dataBase.gbAddClient(clientResult);

            result.message = "";
            result.result = client;
            result.successful = true;
            result.type = ResultType.addClient;

            return result;
        }

        private StandardResultModel addDatabaseGroup(StandardRequestModel request)
        {
            StandardResultModel result = new StandardResultModel();
            GroupInfoModel groupResult = null;
            GroupInfoModel group = null;

            groupResult = JsonConvert.DeserializeObject<GroupInfoModel>(request.requestData.ToString());
            group = dataBase.gbAddGroup(groupResult);

            result.message = "";
            result.result = group;
            result.successful = fileController.creatGroupOrder(group.Name);
            result.type = ResultType.addGroup;

            return result;
        }

        private StandardResultModel addDatabaseSoftwarePackage(StandardRequestModel request)
        {
            StandardResultModel result = new StandardResultModel();
            PackageInfoModel package = null;
            PackageInfoModel packageResult = null;

            packageResult = JsonConvert.DeserializeObject<PackageInfoModel>(request.requestData.ToString());
            package = dataBase.gbAddPackage(packageResult);

            result.message = "";
            result.result = package;
            result.successful = true;
            result.type = ResultType.addPackage;

            return result;
        }

        private StandardResultModel delDatabaeClient(StandardRequestModel request)
        {
            StandardResultModel result = new StandardResultModel();
            ClientInfoModel clientResult = null;


            clientResult = JsonConvert.DeserializeObject<ClientInfoModel>(request.requestData.ToString());
            result.successful = dataBase.gbDeleteClient(clientResult);

            result.message = "";
            result.result = clientResult;
            result.type = ResultType.delDatabaeClient;

            return result;
        }

        private StandardResultModel delDatabaseGroup(StandardRequestModel request)
        {
            StandardResultModel result = new StandardResultModel();
            GroupInfoModel groupResult = null;

            groupResult = JsonConvert.DeserializeObject<GroupInfoModel>(request.requestData.ToString());
            result.successful = dataBase.gbDeleteGroup(groupResult);

            fileController.delGroupOrder(groupResult.Name);

            result.message = "";
            result.result = groupResult;
            result.type = ResultType.delDatabaseGroup;

            return result;
        }

        private StandardResultModel delDatabaseSoftwarePackage(StandardRequestModel request)
        {
            StandardResultModel result = new StandardResultModel();
            PackageInfoModel packageResult = null;
            GroupInfoModel group = null;
            
            LocalDB dataBase = new LocalDB();

            packageResult = JsonConvert.DeserializeObject<PackageInfoModel>(request.requestData.ToString());
            group = dataBase.Converter.GetGroupByPackage(packageResult);
            result.successful = dataBase.gbDeletePackage(packageResult);

            fileController.delSoftwarePackage(fileController.getPathFromFile(group.Name, packageResult.Name));

            result.message = "";
            result.result = packageResult;
            result.type = ResultType.delDatabaseSoftwarePackage;

            return result;
        }

        private StandardResultModel sendSoftwarePackage(StandardRequestModel request)
        {
            StandardResultModel result = new StandardResultModel();
            //List<ClientInfoModel> clientList = null;
            ClientInfoModel client = null;
            LocalDB dataBase = new LocalDB();

            //clientList = dataBase.Converter.GetClientInfoModels();

            //if (client == null)
            //{
                result.message = "";
                result.successful = true;
                result.type = ResultType.readPackage;
                result.result = client;
                return result;
            //}

            //for (int i = 0; i < clientList.Count; i++)
            //{
            //    if (request.Client.macAddress == clientList[i].macAddress)
            //    {
            //        client = clientList[i];
            //    }
            //}

            //result.message = "";
            //result.successful = true;
            //result.type = ResultType.readPackage;
            //result.result = client;

            //return result;
        }

        private void loginClient(StandardRequestModel request)
        {
            ClientInfoModel client = request.Client;
            ClientInfoModel newClient = null;
            GroupInfoModel newGroup = null;
            LocalDB oDB = new LocalDB();
            newClient = oDB.gbAddClient(client);
            newGroup = oDB.Converter.GetGroupByClient(newClient);
            
            string[] lines = Regex.Split(client.macAddress, "\r\n");
            string macAddress = "";

            foreach (string line in lines)
            {
                macAddress += line;
            }

            if (newGroup == null)
            {
                Console.WriteLine("Anmeldung Client:" + macAddress + " " + " Gruppe:" + client.group + " " + " Admin:" + client.admin + "\n" + "Request:" + request.request);
            }
            else
            {
                Console.WriteLine("Anmeldung Client:" + macAddress + " " + " Gruppe:" + newGroup.Name + " " + " Admin:" + client.admin + "\n" + "Request:" + request.request);
            }
        }

        public void stopConnection()
        {
            tcpConnection.Close();
        }

    }
}
