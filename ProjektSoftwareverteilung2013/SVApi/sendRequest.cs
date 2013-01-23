﻿using System.Collections.Generic;
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

        public bool addClientInfo(ClientInfoModel client, ClientInfoModel addClient)
        {
            bool value = false;
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

            if (resultInfo.successful && resultInfo.type == ResultType.addClient)
            {
                value = true;
            }

            mRequest = null;
            return value;
        }

        public bool addGroupInfo(ClientInfoModel client, GroupInfoModel addGroup)
        {
            bool value = false;
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

            if (resultInfo.successful && resultInfo.type == ResultType.addGroup)
            {
                value = true;
            }

            mRequest = null;
            return value;
        }

        public bool addPackageInfo(ClientInfoModel client, PackageInfoModel addPackage)
        {
            bool value = false;
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

            if (resultInfo.successful && resultInfo.type == ResultType.addClient)
            {
                value = true;
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