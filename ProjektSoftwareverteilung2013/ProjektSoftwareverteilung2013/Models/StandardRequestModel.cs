using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjektSoftwareverteilung2013.Models
{
    enum RequestTyp
    {
        upDateRequest = 0,
        getDatabaseGroups = 1,
        getDatabaseClients = 2,
        getDatabaseSoftwarePackages = 3,
        getGroupClients = 4,
        getGrupePackages = 5,
        getClientPackages = 6,
        addDatabaseClient = 7,
        addDatabaseGroup = 8,
        addDatabaseSoftwarePackage = 9,
        delDatabaeClient = 10,
        delDatabaseGroup = 11,
        delDatabaseSoftwarePackage = 12,
        sendSoftwarePackage = 13,
    };

    class StandardRequestModel
    {
        public RequestTyp request { get; set; }
        public object requestData { get; set; }
        public ClientInfoModel Client { get; set; }
    }
}
