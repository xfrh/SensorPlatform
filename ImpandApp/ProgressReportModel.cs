using OctivLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpandApp
{
    public  class ProgressReportModel
    {
        public int PercentageComplete { get; set; } = 0;
       
    }

    
    public class Acdata
    {
        public string  Time { get; set; }
        public int PowerInput { get; set; }
        public double Frequency { get; set; }
        public double Voltage { get; set; }
        public double Current { get; set; }
        public double Phrase { get; set; }
        public float Footer6V { get; set; }
        public float Footer8V { get; set; }

    }
    public class PowerModel
    {
        public string Settingvalue { get; set; }
        public int Testingvalue { get; set; }
    }

    public class DataModel
    {
        public string TestTime { get; set; }
        public PowerModel Power { get; set; }
        public RFCData Sensor { get; set; }
        public DMMData Dmm { get; set; }
     
    }

    public class ModibusModel
    {
       public byte address { get; set; }
       public ushort start { get; set; }
       public ushort registers { get; set; }
       public short[] values { get; set; }
    }
}
