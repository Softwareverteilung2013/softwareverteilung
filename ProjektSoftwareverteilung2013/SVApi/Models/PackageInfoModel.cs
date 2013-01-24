using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SVApi.Models
{
    public class PackageInfoModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string showName  { get; set; }
        public int size { get; set; }
        public string arc { get; set; }
        public int group { get; set; }
    }
}
