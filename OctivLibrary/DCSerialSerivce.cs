using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OctivLibrary
{
  public class DCSerialSerivce
    {

        public static string Portname;
        static SerialPort serial = new SerialPort();
       
        public static void Connect()
        {
        
            try
            {
                if (!serial.IsOpen)
                {
                    serial.PortName = Portname;
                    serial.DtrEnable = true;
                    serial.RtsEnable = true;
                    serial.Open();
                    serial.DataReceived += Serial_DataReceived;
                    serial.WriteLine("SYST:REM");
                    serial.WriteLine("*RST");
                    serial.WriteLine("*CLS");
                    serial.WriteLine("Output on");
                }

            }
            catch (Exception ex)
            {
                LogService.LogMessage(ex.Message);
              
            }

        }

        private static void Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(500);
        }

        public static void ShowOutput()
        {
            serial.WriteLine("Output on");
        }

        public static void CloseOutput()
        {
            serial.WriteLine("Output off");
        }

        public static void Close()
        {
              serial.Close();
        }

        public static bool SetVoltage(float voltage,int channel)
        {
            try
            {
              if (serial.IsOpen)
                {
                    switch (channel)
                    {
                        case 1:
                        case 4:
                            serial.WriteLine("INST:SEL P25V");
                            serial.WriteLine("Volt " + voltage.ToString("N2"));
                            return true;
                        case 2:
                        case 5:
                            serial.WriteLine("INST:SEL N25V");
                            serial.WriteLine("Volt " + voltage.ToString("N2"));
                            return true;
                        case 3:
                        default:
                            serial.WriteLine("INST:SEL P6V");
                            serial.WriteLine("Volt " + voltage.ToString("N2"));
                            return true;

                    }
               }
                return false;

            }
            catch (Exception ex)
            {
                LogService.LogMessage(ex.Message);
                return false;
               
            }
        }
        public static void GetVoltage()
        {
            if (serial.IsOpen)
            {
                serial.WriteLine("Meas:Volt?");

            }
        }

        //public static Dcdata GetData(float voltageSet)
        //{
        //    try
        //    {
        //        if (serial.IsOpen)
        //        {
        //            serial.ReadTimeout = 2000;
        //            serial.WriteLine("Meas:Volt?");
        //            float voltage = float.Parse(serial.ReadLine());
        //            serial.WriteLine("Meas:Curr?");
        //            float cur = float.Parse(serial.ReadLine());
        //            return new Dcdata() { CurrentRead = cur, VoltageRead = voltage, VoltageSet = voltageSet };
        //        }
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {

        //        return null;
        //    }
        //}

        //public static float GetCurrency()
        //{
        //    if (serial.IsOpen)
        //    {
        //        serial.WriteLine("Meas:Curr?");
        //        return float.Parse(serial.ReadLine());
        //    }
        //    return 0;
        //}

    }
}
