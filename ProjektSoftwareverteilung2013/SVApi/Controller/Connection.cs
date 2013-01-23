﻿using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using SVApi.Models;
using System.IO;
using ProjektSoftwareverteilung2013.Controller;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace SVApi.Controller
{
    public class Connection
    {
        private TcpClient ServerConnection = null;
        private IPEndPoint ConnectionInformation = null;
        private Stream connectionStream = null;

        public Connection(string ipAddress)
        {
            int port = 5555;
            IPAddress mIpAddress = IPAddress.Parse(ipAddress);
            ConnectionInformation = new IPEndPoint(mIpAddress,port);
        }

        public StandardResultModel startConnection(StandardRequestModel request)
        {
            StandardResultModel result = null;
            string strRequest = null, strResult = null; 
            ServerConnection = new TcpClient(ConnectionInformation);

            if (!ServerConnection.Connected)
            {
                return result;
            }

            connectionStream = ServerConnection.GetStream();
            strRequest = JsonConvert.SerializeObject(request);
            sendStringStream(strRequest);

            strResult = readStream();
            result = JsonConvert.DeserializeObject<StandardResultModel>(strResult);

            return result;
        }

        public void closeConnection()
        {
            ServerConnection.Close();
        }

        private string readStream()
        {

            string message = null;
            byte[] readingBytes = new byte[1024 * 10];
            int len;
            if (!connectionStream.CanRead)
            {
                return "";
            }
            while ((len = connectionStream.Read(readingBytes, 0, readingBytes.Length)) > 0)
            {
                break;
            }

            message = Encoding.ASCII.GetString(readingBytes);
            string newString = Regex.Replace(message, "(\\s+)", " ");

            return newString;
        }

        private void sendStringStream(string request)
        {
            if (!connectionStream.CanWrite)
            {
                return;
            }
            try
            {
                Byte[] sendBytes = Encoding.ASCII.GetBytes(request);
                connectionStream.Write(sendBytes, 0, sendBytes.Length);
            }
            catch (Exception)
            {
                //Diagnostics.WriteToEventLog(ex.Message, EventLogEntryType.Error, 3000);
            }

            //this.stopConnection();
        }

        public bool readFile(string savePath)
        {
            if (!ServerConnection.Connected)
            {
                return false;
            }

            string filePath = readFileStream(savePath);

            if (!File.Exists(filePath))
            {
                return false;
            }

            return true;
        }

        private string readFileStream(string savePath)
        {
            if (!connectionStream.CanRead)
            {
                return "";
            }

            Socket clientSock = ServerConnection.Client;
            byte[] file = new byte[1024 * 5000];
            //Speicherort
            string receivedPath = savePath;

            int receivedBytesLen = clientSock.Receive(file);
            int fileNameLen = BitConverter.ToInt32(file, 0);
            string fileName = Encoding.ASCII.GetString(file, 4, fileNameLen);

            BinaryWriter bWrite = new BinaryWriter(File.Open(receivedPath + fileName, FileMode.Append));
            bWrite.Write(file, 4 + fileNameLen, receivedBytesLen - 4 - fileNameLen);

            bWrite.Close();

            return receivedPath + fileName;
        }

        public bool sendFile(string pathToFile)
        {
            if (!File.Exists(pathToFile))
            {
                return false;
            }

            if (!ServerConnection.Connected)
            {
                return false;
            }
           
            if (!connectionStream.CanWrite)
            {
                return false;
            }
            sendFileToStream(pathToFile);

            return true;
        }


        private void sendFileToStream(string pathToFile)
        {
            string strName = Path.GetFileName(pathToFile);

            byte[] fileName = Encoding.ASCII.GetBytes(strName);
            byte[] fileData = File.ReadAllBytes(pathToFile);
            byte[] fileNameLen = BitConverter.GetBytes(fileData.Length);
            byte[] file = new byte[4 + fileName.Length + fileNameLen.Length];

            fileNameLen.CopyTo(file, 0);
            fileName.CopyTo(file, 4);
            fileData.CopyTo(file, 4 + fileName.Length);

            try
            {
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