using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjektSoftwareverteilung2013.Models
{
    enum RequestTyp
    {
        upDateRequest,
        getDatabaseGroups,
        getGroupClients,
        getGrupePackages,
        getDatabaseClients,
        getDatabaseSoftwarePackages,
        getClientPackages,
        addDatabaseClient,
        addDatabaseGroup,
        addDatabaseSoftwarePackage,
        delDatabaeClient,
        delDatabaseGroup,
        delDatabaseSoftwarePackage,
        sendSoftwarePackage,
    };

    class StandardRequestModel
    {
        public RequestTyp request { get; set; }
        public object requestData { get; set; }
        public ClientInfoModel Client { get; set; }
    }
}
