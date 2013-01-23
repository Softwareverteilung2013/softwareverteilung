﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SVApi.Models
{
    class ClientInfoModel
    {
        public int ID { get; set; }
        public string macAddress { get; set; }
        public int group { get; set; }
        public string arc { get; set; }
        public bool admin { get; set; }
    }
}