using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Data.SqlServerCe;
using ProjektSoftwareverteilung2013.Controller;
using ProjektSoftwareverteilung2013.Models;
using ProjektSoftwareverteilung2013.Datenbanken;

namespace ProjektSoftwareverteilung2013.Controller
{
    class DataConverter
    {
        private SqlCeConnection Connection = null;

        public DataConverter()
        {
            LocalDB oDB = new LocalDB();

            this.Connection = oDB.Connection;
        }

        public List<ClientInfoModel> GetClientInfoModels()
        {
            List<ClientInfoModel> oResult = new List<ClientInfoModel>();
            DataTable oData = new DataTable();

            SqlCeDataAdapter oDataAdapter = new SqlCeDataAdapter("SELECT * FROM Client", Connection);
            oDataAdapter.Fill(oData);

            if (oData.Rows.Count > 0)
            {
                foreach (DataRow oRow in oData.Rows)
                {
                    ClientInfoModel oClient = new ClientInfoModel();

                    oClient.admin = Convert.ToBoolean(oRow["Client_Administrator"]);
                    oClient.group = Convert.ToInt32(oRow["Client_Gruppe"]);
                    oClient.ID = Convert.ToInt32(oRow["Client_ID"]);
                    oClient.arc = Convert.ToString(oRow["Client_Arc"]);

                    oResult.Add(oClient);
                }
            }
            return oResult;
        }

        public List<GroupInfoModel> GetGroupInfoModel()
        {
            List<GroupInfoModel> oResult = new List<GroupInfoModel>();
            DataTable oData = new DataTable();

            SqlCeDataAdapter oDataAdapter = new SqlCeDataAdapter("SELECT * FROM Gruppe", Connection);
            oDataAdapter.Fill(oData);

            if (oData.Rows.Count > 0)
            {
                foreach (DataRow oRow in oData.Rows)
                {
                    GroupInfoModel oGroup = new GroupInfoModel();

                    oGroup.ID = Convert.ToInt32(oRow["Gruppe_ID"]);
                    oGroup.Name = oRow["Gruppe_Name"].ToString();

                    oResult.Add(oGroup);
                }
            }

            return oResult;
        }

        public List<PackageInfoModel> GetPackageInfoModel()
        {
            List<PackageInfoModel> oResult = new List<PackageInfoModel>();
            DataTable oData = new DataTable();

            SqlCeDataAdapter oDataAdapter = new SqlCeDataAdapter("SELECT * FROM Softwarepaket", Connection);
            oDataAdapter.Fill(oData);
            
            if (oData.Rows.Count > 0)
            {
                foreach (DataRow oRow in oData.Rows)
                {
                    PackageInfoModel oPackage = new PackageInfoModel();

                    oPackage.ID = Convert.ToInt32(oRow["Softwarepaket_ID"]);
                    oPackage.Name = oRow["Softwarepaket_Name"].ToString();
                    oPackage.size = Convert.ToInt32(oRow["Softwarepaket_Groesse"]);
                    oPackage.arc = oRow["Softwarepaket_Arc"].ToString();

                    oResult.Add(oPackage);
                }
            }
            
            return oResult;
        }
    }
}
