using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFinanceApp
{
    public struct MyDataStruct
    {
        public string Description { get; set; }
        public double Value { get; set; }
        public double CurPercent { get; set; }
        public double TargetPercent { get; set; }
        public double RebalanceValue { get; set; }
    }
    public  class MyDataRaw
    {
        public string Key { get; set; } // To display the dictionary key (optional)
        public string Description { get; set; }
        public double Value { get; set; }
        public double CurPercent { get; set; }
        public double TargetPercent { get; set; }
        public double RebalanceValue { get; set; }
    }
}
