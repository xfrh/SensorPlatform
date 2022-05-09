using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace OctivLibrary
{

    public class SerialService8
    {
        static byte[] star = { 0x23, 0x2a };
        static byte[] rd_answer = { 0x52, 0x44 };
        static byte[] return_answer = { 0X0D, 0X0A };
        public static string Portname;
        static SerialPort serial = new SerialPort();
        public static event EventHandler OnDmmDataArrial;

        public static void Connect()
        {
            try
            {
                if (!serial.IsOpen)
                {
                    serial.PortName = Portname;
                    serial.BaudRate = 9600;
                    serial.Handshake = System.IO.Ports.Handshake.None;
                    serial.Parity = Parity.None;
                    serial.DataBits = 8;
                    serial.StopBits = StopBits.One;
                    serial.ReadTimeout = 200;
                    serial.WriteTimeout = 50;
                    serial.Open();
                    serial.DataReceived += Serial_DataReceived;
                }
               
                Set_PC();

            }
            catch (Exception ex)
            {
                LogService.LogMessage(ex.Message);
            }

        }

        static void Set_PC()
        {
            if (serial.IsOpen)
            {
                byte[] bytestosend = { 0x23, 0x2a, 0x4F, 0x4E, 0x4C, 0X0D, 0X0A };
                serial.Write(bytestosend, 0, bytestosend.Length);
            }
        }

        public static bool IsOpen(){
            return serial.IsOpen;
        }

        public static void Set_Session()
        {
            if (serial.IsOpen)
            {
                byte[] bytestosend = { 0x23, 0x2a, 0x49, 0x4E, 0x53, 0x30, 0x30, 0x30, 0X0D, 0X0A };
                serial.Write(bytestosend, 0, bytestosend.Length);
            }
        }

        public static void Read_Data()
        {
            try
            {
                if (serial.IsOpen)
                {
                    while (true)
                    {
                        if (!serial.IsOpen) break;
                        byte[] bytestosend = { 0x23, 0x2a, 0x52, 0x44, 0x3F, 0X0D, 0X0A };
                        serial.Write(bytestosend, 0, bytestosend.Length);
                        Thread.Sleep(500);
                    }
                }
            }
            catch (Exception e)
            {

                LogService.LogMessage(e.Message);
            }


        }

        private static void Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {


            Thread.Sleep(500);
            string read = serial.ReadLine();
            if (read.Contains("RD") && read.Length > 18)
            {
                string v = read.Substring(3, 15);
                OnDmmDataArrial?.Invoke(null, new DmmEventArgs() { Voltage = v });

            }


        }



        public static void Reset()
        {
            if (serial.IsOpen)
            {
                byte[] bytestosend = { 0x23, 0x2a, 0x52, 0x53, 0x54, 0X0D, 0X0A };
                serial.Write(bytestosend, 0, bytestosend.Length);
            }
            /* serial.Close();
         serial.Dispose();
         serial = null;*/
        }

        public static bool Close()
        {
            try
            {
                serial.Close();
                return true;
            }
            catch (Exception ex)
            {
                LogService.LogMessage("modbus close: " + ex.Message);
                return false;
            }
        }
    }

   }

