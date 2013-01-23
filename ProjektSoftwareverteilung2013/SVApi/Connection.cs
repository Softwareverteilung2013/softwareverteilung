using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using SVApi.Models;
using System.IO;
using ProjektSoftwareverteilung2013.Controller;
using System.Diagnostics;

namespace SVApi
{
    public class Connection
    {
        private TcpClient ServerConnection = null;
        private IPEndPoint ConnectionInformation = null;

        public Connection(string ipAddress, StandardRequestModel rquest)
        {
            int port = 5555;
            IPAddress mIpAddress = IPAddress.Parse(ipAddress);
            ConnectionInformation = new IPEndPoint(mIpAddress,port);
            ServerConnection = new TcpClient(ConnectionInformation);
        }


        private string readStream()
        {
            StreamReader inStream = new StreamReader(ServerConnection.GetStream());
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
                catch (Exception)
                {
                    loop = false;
                    //Diagnostics.WriteToEventLog(ex.Message, EventLogEntryType.Error, 3000);
                }
            }
            ServerConnection.Close();
            ServerConnection = null;

            return message;
        }

        private void sendStringStream(string message)
        {
            ServerConnection = new TcpClient(ConnectionInformation);
            Stream outStream = ServerConnection.GetStream();
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
                //Diagnostics.WriteToEventLog(ex.Message, EventLogEntryType.Error, 3000);
            }

            //this.stopConnection();
        }

        private void readFile()
        {
            Socket clientSock = ServerConnection.Client;
            byte[] file = new byte[1024 * 5000];
            //Speicherort
            string receivedPath = "";

            int receivedBytesLen = clientSock.Receive(file);
            int fileNameLen = BitConverter.ToInt32(file, 0);
            string fileName = Encoding.ASCII.GetString(file, 4, fileNameLen);

            BinaryWriter bWrite = new BinaryWriter(File.Open(receivedPath + fileName, FileMode.Append));
            bWrite.Write(file, 4 + fileNameLen, receivedBytesLen - 4 - fileNameLen);

            bWrite.Close();

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

                ServerConnection.Client.Send(file);
                Console.WriteLine("File: {0} has been sent.", fileName);
            }
            catch (Exception ex)
            {
                Diagnostics.WriteToEventLog(ex.Message, EventLogEntryType.Error, 3000);
            }

        }
    }
}
