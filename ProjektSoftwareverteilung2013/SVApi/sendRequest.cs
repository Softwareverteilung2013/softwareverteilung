using System.Collections.Generic;
using SVApi.Controller;
using SVApi.Models;

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

            if (resultInfo.successful && resultInfo.type == ResultType.defaultInfo)
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

            if (resultInfo.successful && resultInfo.type == ResultType.ClientInfo)
            {
                clientInfo = (List<ClientInfoModel>)resultInfo.result;
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

            resultInfo = mConnection.startConnection(mRequest);
            mConnection.closeConnection();

            if (resultInfo.successful && resultInfo.type == ResultType.GroupInfo)
            {
                groupInfo = (List<GroupInfoModel>)resultInfo.result;
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

            if (resultInfo.successful && resultInfo.type == ResultType.SoftwarePackagesInfo)
            {
                packageInfo = (List<PackageInfoModel>)resultInfo.result;
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

            if (resultInfo.successful && resultInfo.type == ResultType.GroupClients)
            {
                clientInfo = (List<ClientInfoModel>)resultInfo.result;
            }

            mRequest = null;
            return clientInfo;
        }

        public List<PackageInfoModel> getGroupPackages(ClientInfoModel client, GroupInfoModel group)
        {
            List<PackageInfoModel> clientInfo = null;
            StandardResultModel resultInfo = null;

            if (!client.admin)
            {
                return clientInfo;
            }

            mRequest = RequestController.creatGetGrupePackages(client,group);


            if (mRequest == null)
            {
                return clientInfo;
            }

            resultInfo = mConnection.startConnection(mRequest);
            mConnection.closeConnection();

            if (resultInfo.successful && resultInfo.type == ResultType.GrupePackages)
            {
                clientInfo = (List<PackageInfoModel>)resultInfo.result;
            }

            mRequest = null;
            return clientInfo;
        }

        public List<PackageInfoModel> getClientPackages(ClientInfoModel client, ClientInfoModel clientInfo)
        {
            List<PackageInfoModel> Info = null;
            StandardResultModel resultInfo = null;

            if (!client.admin)
            {
                return Info;
            }

            mRequest = RequestController.creatGetClientPackages(client, clientInfo);


            if (mRequest == null)
            {
                return Info;
            }

            resultInfo = mConnection.startConnection(mRequest);
            mConnection.closeConnection();

            if (resultInfo.successful && resultInfo.type == ResultType.GrupePackages)
            {
                Info = (List<PackageInfoModel>)resultInfo.result;
            }

            mRequest = null;
            return Info;
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

            ClientInfoModel resultClient = (ClientInfoModel)resultInfo.result;

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

            GroupInfoModel resultGroup = (GroupInfoModel)resultInfo.result;

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

            PackageInfoModel resultPackage = (PackageInfoModel)resultInfo.result;

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

            if (resultInfo.successful && resultInfo.type == ResultType.delDatabaseSoftwarePackage)
            {
                value = true;
            }

            mRequest = null;
            return value;
        }
    }
}
