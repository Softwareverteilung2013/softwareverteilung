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

        public ServerThread(TcpClient connection)
        {
            tcpConnection = connection;

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
            setDatabaseClient(Client);

            //Verarbeitung des Request
            StandardResultModel result = getResult(request);

            sendStringStream(result);

        }

        private StandardRequestModel readStream()
        {
            StreamReader inStream = new StreamReader(tcpConnection.GetStream());
            StandardRequestModel request = null;
            string message = null, line = null;
            bool loop = true;

            while (loop)
            {
                try
                {
                    line = inStream.ReadLine();

                    if (!line.Equals("end"))
                    {
                        message += line;
                    }

                    loop = !line.Equals("end");

                }
                catch (Exception ex)
                {
                    loop = false;
                    Diagnostics.WriteToEventLog(ex.Message, EventLogEntryType.Error, 3000);
                }
            }

            request = JsonConvert.DeserializeObject<StandardRequestModel>(message);
            return request;
        }

        private void sendStringStream(StandardResultModel message)
        {
            Stream outStream = tcpConnection.GetStream();
            string result = JsonConvert.SerializeObject(message);
            string end = "end";
            try
            {
                Byte[] sendBytes = Encoding.ASCII.GetBytes(result);
                outStream.Write(sendBytes, 0, sendBytes.Length);

                sendBytes = Encoding.ASCII.GetBytes(end);
                outStream.Write(sendBytes, 0, sendBytes.Length);
            }
            catch (Exception ex)
            {
                Diagnostics.WriteToEventLog(ex.Message, EventLogEntryType.Error, 3000);
            }

            //this.stopConnection();
        }

        private void readFile()
        {
            Socket clientSock = tcpConnection.Client;
            byte[] file = new byte[1024 * 5000];
            //Speicherort
            string receivedPath = "";

            int receivedBytesLen = clientSock.Receive(file);
            int fileNameLen = BitConverter.ToInt32(file, 0);
            string fileName = Encoding.ASCII.GetString(file, 4, fileNameLen);

            Console.WriteLine("Client:{0} connected & File {1} started received.", clientSock.RemoteEndPoint, fileName);

            BinaryWriter bWrite = new BinaryWriter(File.Open(receivedPath + fileName, FileMode.Append));
            bWrite.Write(file, 4 + fileNameLen, receivedBytesLen - 4 - fileNameLen);

            bWrite.Close();

            Console.WriteLine("File: {0} received & saved at path: {1}", fileName, receivedPath);

        }

        private void sendFile()
        {
            byte[] fileName = Encoding.ASCII.GetBytes("");
            byte[] fileData = File.ReadAllBytes("");
            byte[] fileNameLen = BitConverter.GetBytes(fileData.Length);
            byte[] file = new byte[4 + fileName.Length + fileNameLen.Length];

            fileNameLen.CopyTo(file, 0);
            fileName.CopyTo(file, 4);
            fileData.CopyTo(file, 4 + fileName.Length);

            try
            {
                //using (Stream stream = tcpConnection.GetStream())
                //{
                //    stream.Write(file, 0, file.Length);
                //    stream.Close();
                //}

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
                case RequestTyp.getDatabaseClients:
                    break;
                case RequestTyp.getDatabaseGroups:
                    break;
                case RequestTyp.getDatabaseSoftwarePackages:
                    break;
                case RequestTyp.sendSoftwarePackage:
                    if (!request.Client.admin)
                    {
                        break;
                    }
                    this.readFile();
                    //Zum Gruppen Ordner Hizufügen
                    result.successful = true;
                    result.message = "FilenName:";
                    result.type = ResultType.defaultInfo;
                    break;
                case RequestTyp.addDatabaseClient:

                    if (!request.Client.admin)
                    {
                        break;
                    }
                    ClientInfoModel client = (ClientInfoModel)request.requestData;
                    success = setDatabaseClient(client);
                    result.successful = success;
                    result.message = "";
                    result.result = null;
                    result.type = ResultType.addClient;

                    break;
                case RequestTyp.addDatabaseGroup:
                    break;
                case RequestTyp.addDatabaseSoftwarePackage:
                    break;
                case RequestTyp.upDateRequest:
                    break;
                case RequestTyp.delDatabaeClient:
                    break;
                case RequestTyp.delDatabaseGroup:
                    break;
                case RequestTyp.delDatabaseSoftwarePackage:
                    break;
                default:
                    break;
            }
            return result;
        }

        private bool setDatabaseClient(ClientInfoModel client)
        {
            Console.WriteLine("Anmeldung Client:" + client.macAddress + " Gruppe:" + client.group + " Admin:" + client.admin);
            return false;
        }

        public void stopConnection()
        {
            tcpConnection.Close();
        }

    }
}
