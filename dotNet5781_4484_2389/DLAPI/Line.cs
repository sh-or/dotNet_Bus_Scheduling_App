﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class Line
    {
        public int Code { get; set; }
        public int BusLine { get; set; }
        public AreaEnum Area { get; set; }
        public int FirstStation { get; set; }
        public int LastStation { get; set; }
        public bool IsExist { get; set; } //flag for deleting

    }
}
