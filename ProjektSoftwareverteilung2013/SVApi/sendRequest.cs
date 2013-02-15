using System.Collections.Generic;
using SVApi.Controller;
using SVApi.Models;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SVApi
{
    public class sendRequest
    {
        private StandardRequestModel mRequest = null;
        private Connection mConnection = null;
        private string mIpAdress = null;

        public sendRequest(string ipAdress)
        {
            mIpAdress = ipAdress;
            mConnection = new Connection(mIpAdress);
        }

        public bool sendUpdateRequest(ClientInfoModel client, string savePath)
        {
            bool value = false;
            StandardResultModel resultInfo = null;

            if (client.admin)
            {
                return value;
            }

            mRequest = RequestController.creatUpdateRequest(client);

            if (mRequest == null)
            {
                return value;
            }

            resultInfo = mConnection.startConnection(mRequest);

            if (resultInfo == null)
            {
                return value;
            }
            

            if (resultInfo.successful && resultInfo.type == ResultType.sendPackage)
            {
                value = mConnection.readFile(savePath);
            }

            mConnection.closeConnection();

            mRequest = null;
            return value;
        }

        public bool sendFile(ClientInfoModel client, GroupInfoModel group, PackageInfoModel package, string filePath)
        {
            bool value = false;
            StandardResultModel resultInfo = null;

            if (!client.admin)
            {
                return value;
            }

            mRequest = RequestController.creatSendSoftwarePackageRequest(client, group, package);

            if (mRequest == null)
            {
                return false;
            }

            resultInfo = mConnection.startConnection(mRequest);

            if (resultInfo == null)
            {
                return false;
            }

            if (resultInfo.successful && resultInfo.type == ResultType.readPackage)
            {
                value = mConnection.sendFile(filePath);
            }

            mConnection.closeConnection();

            return false;
        }

        public List<ClientInfoModel> getDatabaseClients(ClientInfoModel client)
        {
            List<ClientInfoModel> clientInfo = null;
            StandardResultModel resultInfo = null;

            if (!client.admin)
            {
                return clientInfo;
            }

            mRequest = RequestController.creatGetClientsRequest(client);


            if (mRequest == null)
            {
                return clientInfo;
            }

            resultInfo = mConnection.startConnection(mRequest);
            mConnection.closeConnection();

            if (resultInfo == null)
            {
                return clientInfo;
            }

            if (resultInfo.successful && resultInfo.type == ResultType.ClientInfo)
            {
                clientInfo = new List<ClientInfoModel>();
                JArray array = (JArray)resultInfo.result;
                clientInfo = JsonConvert.DeserializeObject<List<ClientInfoModel>>(array.ToString());
            }

            mRequest = null;
            return clientInfo;
        }

        public List<GroupInfoModel> getDatabaseGroups(ClientInfoModel client)
        {
            List<GroupInfoModel> groupInfo = null;
            StandardResultModel resultInfo = null;
            

            if (!client.admin)
            {
                return groupInfo;
            }
            mRequest = RequestController.creatGetGroupsRequest(client);


            if (mRequest == null)
            {
                return groupInfo;
            }

            try
            {
                resultInfo = mConnection.startConnection(mRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            mConnection.closeConnection();

            if (resultInfo == null)
            {
                return null;
            }

            if (resultInfo.successful && resultInfo.type == ResultType.GroupInfo)
            {
                groupInfo = new List<GroupInfoModel>();
                 JArray array =(JArray)resultInfo.result;
                 groupInfo = JsonConvert.DeserializeObject<List<GroupInfoModel>>(array.ToString());
            }

            mRequest = null;
            return groupInfo;
        }

        public List<PackageInfoModel> getDatabasePackages(ClientInfoModel client)
        {
            List<PackageInfoModel> packageInfo = null;
            StandardResultModel resultInfo = null;

            if (!client.admin)
            {
                return packageInfo;
            }

            mRequest = RequestController.creatGetGroupsRequest(client);


            if (mRequest == null)
            {
                return packageInfo;
            }

            resultInfo = mConnection.startConnection(mRequest);
            mConnection.closeConnection();

            if (resultInfo == null)
            {
                return packageInfo;
            }

            if (resultInfo.successful && resultInfo.type == ResultType.SoftwarePackagesInfo)
            {
                packageInfo = new List<PackageInfoModel>();
                JArray array = (JArray)resultInfo.result;
                packageInfo = JsonConvert.DeserializeObject<List<PackageInfoModel>>(array.ToString());
            }

            mRequest = null;
            return packageInfo;
        }

        public List<ClientInfoModel> getGroupClients(ClientInfoModel client, GroupInfoModel group)
        {
            List<ClientInfoModel> clientInfo = null;
            StandardResultModel resultInfo = null;

            if (!client.admin)
            {
                return clientInfo;
            }

            mRequest = RequestController.creatGetGroupClients(client,group);


            if (mRequest == null)
            {
                return clientInfo;
            }

            resultInfo = mConnection.startConnection(mRequest);
            mConnection.closeConnection();

            if (resultInfo == null)
            {
                return clientInfo;
            }

            if (resultInfo.successful && resultInfo.type == ResultType.GroupClients)
            {
                clientInfo = new List<ClientInfoModel>();
                JArray array = (JArray)resultInfo.result;
                clientInfo = JsonConvert.DeserializeObject<List<ClientInfoModel>>(array.ToString());
            }

            mRequest = null;
            return clientInfo;
        }

        public List<PackageInfoModel> getGroupPackages(ClientInfoModel client, GroupInfoModel group)
        {
            List<PackageInfoModel> packageInfo = null;
            StandardResultModel resultInfo = null;

            if (!client.admin)
            {
                return packageInfo;
            }

            mRequest = RequestController.creatGetGrupePackages(client,group);


            if (mRequest == null)
            {
                return packageInfo;
            }

            resultInfo = mConnection.startConnection(mRequest);
            mConnection.closeConnection();

            if (resultInfo == null)
            {
                return packageInfo;
            }

            if (resultInfo.successful && resultInfo.type == ResultType.GrupePackages)
            {
                packageInfo = new List<PackageInfoModel>();
                JArray array = (JArray)resultInfo.result;
                if (array != null)
                {
                    packageInfo = JsonConvert.DeserializeObject<List<PackageInfoModel>>(array.ToString());
                }
            }

            mRequest = null;
            return packageInfo;
        }

        public List<PackageInfoModel> getClientPackages(ClientInfoModel client, ClientInfoModel clientInfo)
        {
            List<PackageInfoModel> packageInfo = null;
            StandardResultModel resultInfo = null;

            if (!client.admin)
            {
                return packageInfo;
            }

            mRequest = RequestController.creatGetClientPackages(client, clientInfo);


            if (mRequest == null)
            {
                return packageInfo;
            }

            resultInfo = mConnection.startConnection(mRequest);
            mConnection.closeConnection();

            if (resultInfo == null)
            {
                return packageInfo;
            }

            if (resultInfo.successful && resultInfo.type == ResultType.GrupePackages)
            {
                packageInfo = new List<PackageInfoModel>();
                JArray array = (JArray)resultInfo.result;
                packageInfo = JsonConvert.DeserializeObject<List<PackageInfoModel>>(array.ToString());
            }

            mRequest = null;
            return packageInfo;
        }

        public int addClientInfo(ClientInfoModel client, ClientInfoModel addClient)
        {
            int value = -1;
            StandardResultModel resultInfo = null;

            if (!client.admin)
            {
                return value;
            }

            mRequest = RequestController.creatAddClientRequest(client, addClient);

            if (mRequest == null)
            {
                return value;
            }

            resultInfo = mConnection.startConnection(mRequest);
            mConnection.closeConnection();

            if (resultInfo == null)
            {
                return value;
            }

            ClientInfoModel resultClient = JsonConvert.DeserializeObject<ClientInfoModel>(resultInfo.result.ToString());

            if (resultInfo.successful && resultInfo.type == ResultType.addClient)
            {
                value = resultClient.ID;
            }

            mRequest = null;
            return value;
        }

        public int addGroupInfo(ClientInfoModel client, GroupInfoModel addGroup)
        {
            int value = -1;
            StandardResultModel resultInfo = null;

            if (!client.admin)
            {
                return value;
            }

            mRequest = RequestController.creatAddGroupRequest(client, addGroup);

            if (mRequest == null)
            {
                return value;
            }

            resultInfo = mConnection.startConnection(mRequest);
            mConnection.closeConnection();

            if (resultInfo == null)
            {
                return value;
            }

            GroupInfoModel resultGroup = JsonConvert.DeserializeObject<GroupInfoModel>(resultInfo.result.ToString());

            if (resultInfo.successful && resultInfo.type == ResultType.addGroup)
            {
                value = resultGroup.ID;
            }

            mRequest = null;
            return value;
        }

        public int addPackageInfo(ClientInfoModel client, PackageInfoModel addPackage)
        {
            int value = -1;
            StandardResultModel resultInfo = null;

            if (!client.admin)
            {
                return value;
            }

            mRequest = RequestController.creatAddPackageRequest(client, addPackage);

            if (mRequest == null)
            {
                return value;
            }

            resultInfo = mConnection.startConnection(mRequest);
            mConnection.closeConnection();

            if (resultInfo == null)
            {
                return value;
            }

            if (resultInfo.result == null)
            {
                return value;
            }

            PackageInfoModel resultPackage = JsonConvert.DeserializeObject<PackageInfoModel>(resultInfo.result.ToString());

            if (resultInfo.successful && resultInfo.type == ResultType.addClient)
            {
                value = resultPackage.ID;
            }

            mRequest = null;
            return value;
        }

        public bool delClientInfo(ClientInfoModel client, ClientInfoModel delClient)
        {
            bool value = false;
            StandardResultModel resultInfo = null;

            if (!client.admin)
            {
                return value;
            }

            mRequest = RequestController.creatDelClientRequest(client, delClient);

            if (mRequest == null)
            {
                return value;
            }

            resultInfo = mConnection.startConnection(mRequest);
            mConnection.closeConnection();

            if (resultInfo == null)
            {
                return value;
            }

            if (resultInfo.successful && resultInfo.type == ResultType.delDatabaeClient)
            {
                value = true;
            }

            mRequest = null;
            return value;
        }

        public bool delGroupInfo(ClientInfoModel client, GroupInfoModel delGroup)
        {
            bool value = false;
            StandardResultModel resultInfo = null;

            if (!client.admin)
            {
                return value;
            }

            mRequest = RequestController.creatDelGroupRequest(client, delGroup);

            if (mRequest == null)
            {
                return value;
            }

            resultInfo = mConnection.startConnection(mRequest);
            mConnection.closeConnection();

            if (resultInfo == null)
            {
                return value;
            }

            if (resultInfo.successful && resultInfo.type == ResultType.delDatabaseGroup)
            {
                value = true;
            }

            mRequest = null;
            return value;
        }

        public bool delPackageInfo(ClientInfoModel client, PackageInfoModel delPackage)
        {
            bool value = false;
            StandardResultModel resultInfo = null;

            if (!client.admin)
            {
                return value;
            }

            mRequest = RequestController.creatDelPackageRequest(client, delPackage);

            if (mRequest == null)
            {
                return value;
            }

            resultInfo = mConnection.startConnection(mRequest);
            mConnection.closeConnection();

            if (resultInfo == null)
            {
                return value;
            }

            if (resultInfo.successful && resultInfo.type == ResultType.delDatabaseSoftwarePackage)
            {
                value = true;
            }

            mRequest = null;
            return value;
        }
    }
}
