using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjektSoftwareverteilung2013.Models
{
    public enum ResultType
    {
        ClientInfo,
        GroupInfo,
        SoftwarePackagesInfo,
        sendPackage,
        addClient,
        addGroup,
        addPackage,
        defaultInfo,
    };

    class StandardResultModel
    {
        public bool successful { get; set; }
        public string message { get; set; }
        public ResultType type { get; set; }
        public object result { get; set; }
    }
}
