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
            while ((len = connectionStream.Read(readingBytes, 0, readingBytes.Length)) > 0)
            {
                break;
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
                Diagnostics.WriteToEventLog(ex.Message, EventLogEntryType.Error, 3000);
            }

            //this.stopConnection();
        }

        private void readFile(GroupInfoModel group, PackageInfoModel package)
        {
            Socket clientSock = tcpConnection.Client;
            byte[] file = new byte[1024 * 5000];

            FileController fileController = new FileController();
            string receivedPath = fileController.getPathFromFile(group.Name, package.Name);

            int receivedBytesLen = clientSock.Receive(file);
            int fileNameLen = BitConverter.ToInt32(file, 0);
            string fileName = Encoding.ASCII.GetString(file, 4, fileNameLen);

            Console.WriteLine("Client:{0} connected & File {1} started received.", clientSock.RemoteEndPoint, fileName);

            BinaryWriter bWrite = new BinaryWriter(File.Open(receivedPath + fileName, FileMode.Append));
            bWrite.Write(file, 4 + fileNameLen, receivedBytesLen - 4 - fileNameLen);

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
                Diagnostics.WriteToEventLog(ex.Message, EventLogEntryType.Error, 3000);
            }

            stopConnection();

        }

        private StandardResultModel getResult(StandardRequestModel request)
        {
            StandardResultModel result = new StandardResultModel();
            bool success = false;
            result.successful = success;

            switch (request.request)
            {
                case RequestTyp.upDateRequest:
                    break;
                case RequestTyp.getDatabaseGroups:
                    break;
                case RequestTyp.getDatabaseClients:
                    break;
                case RequestTyp.getDatabaseSoftwarePackages:
                    break;
                case RequestTyp.addDatabaseClient:
                    break;
                case RequestTyp.addDatabaseGroup:
                    break;
                case RequestTyp.addDatabaseSoftwarePackage:
                    break;
                case RequestTyp.delDatabaeClient:
                    break;
                case RequestTyp.delDatabaseGroup:
                    break;
                case RequestTyp.delDatabaseSoftwarePackage:
                    break;
                case RequestTyp.sendSoftwarePackage:
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
