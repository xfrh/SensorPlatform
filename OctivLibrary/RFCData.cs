using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctivLibrary
{
    public class RFCData
    {
        public string PowerSet { get; set; }
        public string Serialnumber { get; set; }
        public int Frequencycode { get; set; }
        public bool IsConnected { get; set; }
        public double Pfrequency { get; set; }
        public double Phomonic { get; set; }
        public string Frequency { get; set; }
        public string Voltage { get; set; }
        public string PhaseV { get; set; }
        public string Current { get; set; }
        public string Phase { get; set; }
        public string Footer6v { get; set; }
        public string Footer8v { get; set; }
    }

   
}