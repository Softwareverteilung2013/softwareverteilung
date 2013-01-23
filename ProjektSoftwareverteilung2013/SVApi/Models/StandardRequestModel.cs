﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SVApi.Models
{
    enum RequestTyp
    {
        upDateRequest,
        getDatabaseGroups,
        getDatabaseClients,
        getDatabaseSoftwarePackages,
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