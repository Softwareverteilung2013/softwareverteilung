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

            listener = new TcpListener(ipAddress, 5555);
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
            string message = readStream();

            StandardRequestModel request = JsonConvert.DeserializeObject<StandardRequestModel>(message);
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

            message = JsonConvert.SerializeObject(result);
            sendStringStream(message);

            if (result == null)
            {
                stopConnection();
            }

        }

        private string readStream()
        {
            StreamReader inStream = new StreamReader(tcpConnection.GetStream());
            string message = null, line = null;
            bool loop = true;

            while (loop)
            {
                try
                {
                    line = inStream.ReadLine();
                    message += line;
                    loop = !line.Equals("");
                }
                catch (Exception)
                {
                    loop = false;
                }
            }
            return message;
        }

        private void sendStringStream(string message)
        {
            Stream outStream = tcpConnection.GetStream();
            string end = "end";
            try
            {
                Byte[] sendBytes = Encoding.ASCII.GetBytes(message);
                outStream.Write(sendBytes, 0, sendBytes.Length);

                sendBytes = Encoding.ASCII.GetBytes(end);
                outStream.Write(sendBytes, 0, sendBytes.Length);
            }
            catch (Exception)
            {
               
            }

            //this.stopConnection();
        }

        private void readFile()
        {
        }

        private void sendFile()
        {
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
                    break;
                case RequestTyp.setDatabaseClient:

                    if (request.Client.admin)
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
                case RequestTyp.setDatabaseGroup:
                    break;
                case RequestTyp.setDatabaseSoftwarePackage:
                    break;
                case RequestTyp.upDateRequest:
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
