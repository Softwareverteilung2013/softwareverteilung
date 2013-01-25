using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SVApi.Models
{
    public enum ResultType
    {
        ClientInfo = 0,
        GroupInfo = 1,
        SoftwarePackagesInfo = 2,
        GroupClients = 3,
        GrupePackages = 4,
        ClientPackages = 5,
        addClient = 6,
        addGroup = 7,
        addPackage = 8,
        delDatabaeClient = 9,
        delDatabaseGroup = 10,
        delDatabaseSoftwarePackage = 11,
        sendPackage = 12,
        readPackage = 13,
        defaultInfo = 14,
    };

    public class StandardResultModel
    {
        public bool successful { get; set; }
        public string message { get; set; }
        public ResultType type { get; set; }
        public object result { get; set; }
    }
}
