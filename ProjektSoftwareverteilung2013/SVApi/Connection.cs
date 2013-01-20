using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using ProjektSoftwareverteilung2013.Models;
using System.IO;

namespace SVApi
{
    public class Connection
    {
        private TcpClient Connection = null;
        private IPEndPoint ConnectionInformation = null;

        public Connection(string ipAddress, bool admin)
        {
            int port = 5555;
            IPAddress mIpAddress = IPAddress.Parse(ipAddress);
            ConnectionInformation = new IPEndPoint(mIpAddress,port);
            //Connection = new TcpClient(ConnectionInformation);
        }


        private string readStream()
        {
            StreamReader inStream = new StreamReader(Connection.GetStream());
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
            Connection.Close();
            Connection = null;

            return message;
        }

        private void sendStringStream(string message)
        {
            Connection = new TcpClient(ConnectionInformation);
            Stream outStream = Connection.GetStream();
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


    }
}
