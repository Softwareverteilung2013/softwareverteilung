using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SVApi.Models;
using Newtonsoft.Json;

namespace SVApi.Controller
{
    public class ResultController
    {
        public StandardResultModel mResult = null;

        public ResultController(StandardResultModel result)
        {
            mResult = result;
        }

        public object getResult()
        {
            if (mResult == null)
            {
                return null;
            }

            object result = null;

            switch (mResult.type)
            {
                case ResultType.ClientInfo:
                    List<ClientInfoModel> clientList = null;
                    clientList =(List<ClientInfoModel>)mResult.result;
                    result = clientList;
                    break;
                case ResultType.GroupInfo:
                    List<GroupInfoModel> groupList = null;
                    groupList = (List<GroupInfoModel>)mResult.result;
                    result = groupList;
                    break;
                case ResultType.SoftwarePackagesInfo:
                    List<PackageInfoModel> packageList = null;
                    packageList = (List<PackageInfoModel>)mResult.result;
                    result = packageList;
                    break;
                case ResultType.addClient:
                   
                    if (mResult.successful)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                    break;
                case ResultType.addGroup:
                    
                    if (mResult.successful)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                    break;
                case ResultType.addPackage:
                    
                    if (mResult.successful)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                    break;
                case ResultType.delDatabaeClient:
                    
                    if (mResult.successful)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                    break;
                case ResultType.delDatabaseGroup:
                    
                    if (mResult.successful)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                    break;
                case ResultType.delDatabaseSoftwarePackage:
                    
                    if (mResult.successful)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                    break;
                case ResultType.sendPackage:
                    if (mResult.successful)
                    {
                        result = true;
                    }
                    break;
                case ResultType.defaultInfo:
                    if (!mResult.successful)
                    {
                        result = false;
                    }
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
