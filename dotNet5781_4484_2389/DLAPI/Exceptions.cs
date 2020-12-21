﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    //DO Exception:
    public class BusStationNotFoundEx : Exception
    {
        public int Num { get; set; }
        public BusStationNotFoundEx(int num, string message) : base(message) => Num = num;
        public BusStationNotFoundEx(int num, string message, Exception inner) : base(message, inner) => Num = num;
        public override string ToString() => base.ToString() + $"Bus station number {Num} was not found";
    }
    public class BusNotFoundEx : Exception /////////
    {
        public int Num { get; set; }
        public BusNotFoundEx(int num, string message) : base(message) => Num = num;
        public BusNotFoundEx(int num, string message, Exception inner) : base(message, inner) => Num = num;
        public override string ToString() => base.ToString() + $" Bus number {Num} was not found";
    }
    public class LineNotFoundEx : Exception /////////
    {
        public int Num { get; set; }
        public LineNotFoundEx(int num, string message) : base(message) => Num = num;
        public LineNotFoundEx(int num, string message, Exception inner) : base(message, inner) => Num = num;
        public override string ToString() => base.ToString() + $" Line number {Num} was not found";
    }
}
