using System;
using System.Collections.Generic;


namespace OctivLibrary
{
    public class DmmEventArgs : EventArgs
    {
        private string voltage;

        public string Voltage
        {
            get { return voltage; }
            set { voltage = value; }
        }

    }
}
