using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SVApi.Models
{
    public enum ResultType
    {
        ClientInfo,
        GroupInfo,
        SoftwarePackagesInfo,
        GroupClients,
        GrupePackages,
        ClientPackages,
        addClient,
        addGroup,
        addPackage,
        delDatabaeClient,
        delDatabaseGroup,
        delDatabaseSoftwarePackage,
        sendPackage,
        readPackage,
        defaultInfo,
    };

    public class StandardResultModel
    {
        public bool successful { get; set; }
        public string message { get; set; }
        public ResultType type { get; set; }
        public object result { get; set; }
    }
}
