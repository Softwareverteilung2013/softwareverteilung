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
        getDatabaseClients,
        getDatabaseSoftwarePackages,
        setDatabaseClient,
        setDatabaseGroup,
        setDatabaseSoftwarePackage,
        sendSoftwarePackage,
    };

    class StandardRequestModel
    {
        public RequestTyp request { get; set; }
        public object requestData { get; set; }
        public ClientInfoModel Client { get; set; }
    }
}
