using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SVApi;
using SVApi.Models;
using System.Net.NetworkInformation;
using System.Security.Principal;

namespace Client
{
    public partial class frmClient : Form
    {
        public frmClient()
        {
            InitializeComponent();
            //ConnectToServer();
        }



        private void ConnectToServer()
        {
            ClientInfoModel InfoModel = new ClientInfoModel();
            InfoModel.arc = GetArchitecture();
            InfoModel.macAddress = GetMacAddress();
            InfoModel.admin = GetAdminBool();
            
            Connection Connection_ = new Connection(Properties.Settings.Default.ServerIP, InfoModel);
            //
        }

        private string GetMacAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            //for each j you can get the MAC
            PhysicalAddress address = nics[0].GetPhysicalAddress();
            StringBuilder strb = new StringBuilder();
            for (int e = 0; e < nics.Length; e++)
            {
                byte[] bytes = address.GetAddressBytes();
                for (int i = 0; i < bytes.Length; i++)
                {
                    // Display the physical address in hexadecimal.
                    strb.AppendLine(bytes[i].ToString("X2"));
                    // Insert a hyphen after each byte, unless we are at the end of the
                    // address.
                    if (i != bytes.Length - 1)
                    {
                        strb.AppendLine("-");
                    }
                }
            }

            return strb.ToString();
        }

        private bool GetAdminBool()
        {
            bool isAdmin;
            try
            {
                //get the currently logged in user
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (UnauthorizedAccessException ex)
            {
                isAdmin = false;
            }
            catch (Exception)
            {
                isAdmin = false;
            }
            return isAdmin;
        }

        private string GetArchitecture()
        {

            string architecture = System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
            string archWOW = System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432");
            if (archWOW != null && archWOW != "" && archWOW.Contains("64"))
                return "x64";
            if (architecture.Contains("86"))
                return "x86";
            if (architecture.Contains("64"))
                return "x64";

            if (architecture == null)
            {
                architecture = "";
            }
            else if (architecture == "")
	        {
            architecture = "Not Defined";
	        }
            return architecture;
            }

    }
}
