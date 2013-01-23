using SVApi.Models;

namespace SVApi.Controller
{
    class RequestController
    {
        public static StandardRequestModel creatGetClientsRequest(ClientInfoModel client)
        {
            StandardRequestModel request = null;
            if (client == null)
            {
                return request;
            }
            request = new StandardRequestModel();
            request.Client = client;
            request.request = RequestTyp.getDatabaseClients;
            request.requestData = null;
            return request;
        }

        public static StandardRequestModel creatGetGroupsRequest(ClientInfoModel client)
        {
            StandardRequestModel request = null;
            if (client == null)
            {
                return request;
            }
            request = new StandardRequestModel();
            request.Client = client;
            request.request = RequestTyp.getDatabaseGroups;
            request.requestData = null;
            return request;
        }

        public static StandardRequestModel creatGetPackagesRequest(ClientInfoModel client)
        {
            StandardRequestModel request = null;
            if (client == null)
            {
                return request;
            }
            request = new StandardRequestModel();
            request.Client = client;
            request.request = RequestTyp.getDatabaseSoftwarePackages;
            request.requestData = null;
            return request;
        }

        public static StandardRequestModel creatAddClientRequest(ClientInfoModel client, ClientInfoModel addClient)
        {
            StandardRequestModel request = null;
            if (client == null)
            {
                return request;
            }
            request = new StandardRequestModel();
            request.Client = client;
            request.request = RequestTyp.addDatabaseClient;
            request.requestData = addClient;
            return request;
        }

        public static StandardRequestModel creatAddGroupRequest(ClientInfoModel client, GroupInfoModel addGroup)
        {
            StandardRequestModel request = null;
            if (client == null)
            {
                return request;
            }
            request = new StandardRequestModel();
            request.Client = client;
            request.request = RequestTyp.addDatabaseGroup;
            request.requestData = addGroup;
            return request;
        }

        public static StandardRequestModel creatAddPackageRequest(ClientInfoModel client, PackageInfoModel addPackage)
        {
            StandardRequestModel request = null;
            if (client == null)
            {
                return request;
            }
            request = new StandardRequestModel();
            request.Client = client;
            request.request = RequestTyp.addDatabaseSoftwarePackage;
            request.requestData = addPackage;
            return request;
        }

        public static StandardRequestModel creatDelClientRequest(ClientInfoModel client, ClientInfoModel delClient)
        {
            StandardRequestModel request = null;
            if (client == null)
            {
                return request;
            }
            request = new StandardRequestModel();
            request.Client = client;
            request.request = RequestTyp.delDatabaeClient;
            request.requestData = delClient;
            return request;
        }

        public static StandardRequestModel creatDelGroupRequest(ClientInfoModel client, GroupInfoModel delGroup)
        {
            StandardRequestModel request = null;
            if (client == null)
            {
                return request;
            }
            request = new StandardRequestModel();
            request.Client = client;
            request.request = RequestTyp.delDatabaseGroup;
            request.requestData = delGroup;
            return request;
        }

        public static StandardRequestModel creatDelPackageRequest(ClientInfoModel client, PackageInfoModel delPackage)
        {
            StandardRequestModel request = null;
            if (client == null)
            {
                return request;
            }
            request = new StandardRequestModel();
            request.Client = client;
            request.request = RequestTyp.delDatabaseSoftwarePackage;
            request.requestData = delPackage;
            return request;
        }

        public static StandardRequestModel creatSendSoftwarePackageRequest(ClientInfoModel client, GroupInfoModel group, PackageInfoModel package)
        {
            StandardRequestModel request = null;
            if (client == null)
            {
                return request;
            }

            string info = group.Name + "," + package.Name;
            request = new StandardRequestModel();
            request.Client = client;
            request.request = RequestTyp.sendSoftwarePackage;
            request.requestData = info;
            return request;
        }

        public static StandardRequestModel creatUpdateRequest(ClientInfoModel client)
        {
            StandardRequestModel request = null;
            if (client == null)
            {
                return request;
            }
            request = new StandardRequestModel();
            request.Client = client;
            request.request = RequestTyp.upDateRequest;
            request.requestData = null;
            return request;
        }
    }
}
