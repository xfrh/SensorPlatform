using Code4Bugs.Utils.IO;
using Code4Bugs.Utils.IO.Modbus;
using OctivLibrary;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctivLibrary
{
   public class ModbusService
    {
        public static string Portname="COM4";
        static SerialPort sp = new SerialPort();

        public static bool Open()
        {
            try
            {
                if (!sp.IsOpen)
                {
                    sp.PortName = Portname;
                    sp.BaudRate = 9600;
                    sp.DataBits = 8;
                    sp.Parity = System.IO.Ports.Parity.None;
                    sp.StopBits = System.IO.Ports.StopBits.One;
                    //These timeouts are default and cannot be editted through the class at this point:
                    sp.ReadTimeout = 1000;
                    sp.WriteTimeout = 1000;
                    sp.Open();
                    return true;
                }
                return true;
            }
            catch (Exception ex)
            {

                LogService.LogMessage("open modi serial:" + ex.Message);
                return false;
            }
          
        }

        public static bool Close()
        {
            try
            {
                sp.Close();
                return true;
            }
            catch (Exception ex)
            {
               LogService.LogMessage("modbus close: " + ex.Message);
                return false;
            }
        }

        public static bool IsOpen()
        {
            return sp.IsOpen;
        }

        public static bool OpenRF(int frequency)
        {
            try
            {
                if (sp.IsOpen)
                {
                    var stream = new SerialStream(sp);
                    var responseBytes = stream.RequestFunc6(1, 4, frequency);
                    int address = responseBytes.ToResponseFunc6().DataAddress;
                    return address > 0;
                }
                return false;
                 
            }
            catch (Exception e)
            {

                if (e is DataCorruptedException)
                {
                   LogService.LogMessage("checksum is failed");
                }
                else if (e is EmptyResponsedException)
                {
                    LogService.LogMessage("request timeout");
                }
                else if (e is MissingDataException)
                {
                    LogService.LogMessage("Missing response bytes");
                }
                else
                {
                    LogService.LogMessage(e.Message);
                }
                return false;

            }
        }

        public static bool CloseRF()
        {
            try
            {
                if (sp.IsOpen)
                {
                    var stream = new SerialStream(sp);
                    var responseBytes = stream.RequestFunc6(1, 4, 0);
                    int address = responseBytes.ToResponseFunc6().DataAddress;
                    return address > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                LogService.LogMessage("modbus closerf:" + ex.Message);
                return false;
            }
        }

        public static bool SetPower(int value)
        {
            try
            {
                if (sp.IsOpen)
                {
                    var stream = new SerialStream(sp);
                    var responseBytes = stream.RequestFunc6(1, 2, value);
                    int address = responseBytes.ToResponseFunc6().DataAddress;
                    return address > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                LogService.LogMessage("modbus setpower:" + ex.Message);
                return false;
            }
        }
    }
    
    }
