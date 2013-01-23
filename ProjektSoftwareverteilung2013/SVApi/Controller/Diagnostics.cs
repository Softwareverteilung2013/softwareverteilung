using System;
using System.Diagnostics;
using System.IO;

namespace ProjektSoftwareverteilung2013.Controller
{
    public class Diagnostics
    {
        private static string eventName;
        public static String EventName
        {
            get
            {
                if (eventName == null)
                    eventName = "Softwareverteilung2013";

                return eventName;
            }
            set
            {
                eventName = value;
            }
        }

        public static void WriteToEventLog(string message, EventLogEntryType Type, int id)
        {
            try
            {
                var elog = CreateEventlog();
                elog.WriteEntry(message, Type, id);
            }
            catch (Exception)
            {
                //WriteToLogFile(message);
            }
        }

        private static EventLog CreateEventlog()
        {
            var elog = new EventLog();
            if (!EventLog.SourceExists(eventName))
            {
                EventLog.CreateEventSource(eventName, eventName);
            }
            elog.Source = eventName;
            return elog;
        }

        public static void WriteErrToEventLog(string message)
        {
            try
            {
                var elog = CreateEventlog();
                elog.WriteEntry(message, EventLogEntryType.Error, 007);
            }
            catch (Exception)
            {
                //WriteErrToLogFile(message);
            }
        }

        public static void WriteErrToEventLog(Exception ex)
        {
            WriteErrToEventLog(ex, eventName, 3000);
        }

        public static void WriteErrToEventLog(Exception ex, string eventName)
        {
            WriteErrToEventLog(ex, eventName, 3000);
        }

        public static void WriteErrToEventLog(Exception ex, int id)
        {
            //System.Windows.Forms.MessageBox.Show(ex.Message,"Achtung",System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            Console.WriteLine(ex.Message);
            WriteErrToEventLog(ex, eventName, id);
        }

        public static void WriteErrToEventLog(Exception ex, string eventName, int id)
        {
            string strex = "";
            try
            {
                strex = "Fehler bei: " + ex.Source + "\n";
                if (ex.InnerException != null)
                    strex += ex.Message + " inner: " + ex.InnerException.Message;
                else
                    strex += ex.Message;

                strex += "\nStackTrace:\n" + ex.StackTrace;
                strex += "\nTargetSite:\n" + ex.TargetSite.Name;

                var elog = CreateEventlog();

                elog.WriteEntry(strex, EventLogEntryType.Error, id);
            }
            catch (Exception)
            {
                //WriteErrToLogFile(strex);
            }
        }

        //internal static void WriteErrToLogFile(string message)
        //{
        //    string currentPath = Environment.CurrentDirectory;
        //    string dir = "";
        //    if (Directory.Exists(currentPath + AppConst.LogDir))
        //        dir = AppConst.LogDir;

        //    using (var sw = new StreamWriter(dir + "Error.log", true))
        //    {
        //        sw.WriteLine("Neuer Fehler am: " + DateTime.Now);
        //        sw.WriteLine("------------------------------------------------------------");
        //        sw.WriteLine(message);
        //    }
        //}

        //internal static void WriteToLogFile(string message)
        //{
        //    string dir = "";
        //    if (Directory.Exists(AppConst.LogDir))
        //        dir = AppConst.LogDir;

        //    using (var sw = new StreamWriter(dir + "Info.log", true))
        //    {
        //        sw.WriteLine("Neue Meldung am: " + DateTime.Now);
        //        sw.WriteLine("------------------------------------------------------------");
        //        sw.WriteLine(message);
        //    }
        //}
    }
}