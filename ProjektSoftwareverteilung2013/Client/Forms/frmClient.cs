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
using System.Diagnostics;

namespace Client
{
    public partial class frmClient : Form
    {
        public frmClient()
        {
            InitializeComponent();
            //ConnectToServer();
            UpdateTimer.Start();
        }
        
        private void ConnectToServer()
        {
            ClientInfoModel InfoModel = new ClientInfoModel();
            InfoModel.arc = GetArchitecture();
            InfoModel.macAddress = GetMacAddress();
            InfoModel.admin = GetAdminBool();

            sendRequest Request = new sendRequest(Properties.Settings.Default.ServerIP);      
            if (Request.sendUpdateRequest(InfoModel, Properties.Settings.Default.SavePath))
            {
               //Get All Zip Files, Unpack to new underfolder, Move Zip File to Another underfolder, Install Each File in First Underfolder
            }
        }

        private void UnpackZip(string ZipPath)
        {

        }

        private void InstallProgramms(string ProgPath)
        {
            Process CurrentProcess = new Process();
            CurrentProcess.StartInfo.FileName = ProgPath;
            CurrentProcess.StartInfo.Arguments = "/i \"" + ProgPath + "\"/qn";
            CurrentProcess.Start();
        }

        private void DeinstallProgramms(string ProgPath)
        {
            Process CurrentProcess = new Process();
            CurrentProcess.StartInfo.FileName = "";
            CurrentProcess.StartInfo.Arguments = "/x \"" + ProgPath + "\"/qn";
            CurrentProcess.Start();
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

        private void schließenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IconContextMenuStrip.Close();
        }

        private void einstellungenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.frmSetting Settings = new Forms.frmSetting();
            Settings.ShowDialog();
        }

        private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            string CurrentTime = DateTime.Now.TimeOfDay.ToString().Substring(0, 5);
            string Compare = Properties.Settings.Default.RequestTime;
            if (CurrentTime == Compare)
            {
                ConnectToServer();
            }
        }

    }
}
