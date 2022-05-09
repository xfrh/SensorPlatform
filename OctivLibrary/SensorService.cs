using Continuous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OctivLibrary
{
   public class SensorService
    {
        private static  String m_sensor_serial_number = "";
        private static RFCData m_sensor_data = new RFCData();
        private static int sensor_handle = 0;
        public  delegate void Report(RFCData data);
        public static void Start(Report report) {

            try
            {
                #region SENSOR INIT


                while (sensor_handle <= 0)
                {
                    m_sensor_data.Serialnumber = m_sensor_serial_number;
                    sensor_handle = OctivInterface.octivOpen(m_sensor_serial_number);
                }
                if (sensor_handle > 0)
                    m_sensor_data.IsConnected = true;
                else
                    m_sensor_data.IsConnected = false;
                if (string.IsNullOrEmpty(m_sensor_data.Serialnumber))
                {
                    StringBuilder _sb = new StringBuilder();
                    OctivInterface.octivGetSerial(sensor_handle, _sb);
                    m_sensor_data.Serialnumber = _sb.ToString();
                    report(m_sensor_data);
                }
         

                #endregion

                #region pfrequencyValue
                if (m_sensor_data.Pfrequency == 0 && m_sensor_data.Phomonic == 0)
                {
                    double val = 0;
                    int harm = 0;
                    int coindex = OctivInterface.octivGetFrequencyCount(sensor_handle);
                    OctivInterface.octivGetFrequencyDetails(sensor_handle, coindex - 1, ref val, ref harm);
                    m_sensor_data.Pfrequency = val / 100000;
                    m_sensor_data.Phomonic = harm;
                    report(m_sensor_data);
                }

                #endregion


                //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~        
                try
                {
                    #region GET DATA

                    // Get data from sensor here.

                    int retDelay = 0;
                    double peak = 0;
                    int time_difference = 0;

                    int number_of_values = 150; // Allocated size of values for getData function

                    int result;

                    double size = 0.0; // Used to keep double size


                    //  octivGetData call is non-blocking
                    //  Loop until we get data, ie number_of_values > 0
                    while (true)
                    {

                        int tries = 100;
                        IntPtr valuePointer = Marshal.AllocHGlobal(Marshal.SizeOf(size) * 150);
                        int index = OctivInterface.octivGetFrequencyCount(sensor_handle);
                        result = OctivInterface.octivGetData(
                        sensor_handle,
                        valuePointer,
                        ref number_of_values,
                        ref time_difference,
                        ref peak,
                        ref retDelay,
                        ref tries);

                        double[] valueRes = new double[number_of_values];
                        Marshal.Copy(valuePointer, valueRes, 0, number_of_values);
                        Marshal.FreeHGlobal(valuePointer);

                        if (result < 0)
                        {
                            m_sensor_data.IsConnected = false;
                            report(m_sensor_data);

                        
                        }

                        if (number_of_values > 0)
                        {
                            m_sensor_data.Frequency = string.Format("{0:N4}", valueRes[0]);
                            m_sensor_data.Voltage = string.Format("{0:N4}", valueRes[1]);
                            m_sensor_data.PhaseV = string.Format("{0:N4}", valueRes[2]);
                            m_sensor_data.Current = string.Format("{0:N4}", valueRes[3]);
                            m_sensor_data.Phase = string.Format("{0:N4}", valueRes[4]);
                        
                            report(m_sensor_data);

                        }
                        else
                        {
                            // Random rnd = new Random();
                            m_sensor_data.Frequency = "0";//rnd.Next(1,13).ToString();
                            m_sensor_data.Voltage = "0";
                            m_sensor_data.PhaseV = "0";
                            m_sensor_data.Current = "0";
                            m_sensor_data.Phase = "0";

                            report(m_sensor_data);

                        }
                        Thread.Sleep(500);


                    }

                    #endregion
                }
                catch (Exception)
                {

                    throw;
                }
            
            }
            catch (Exception ex)
            {

                LogService.LogMessage("sensorserive getdata" + ex.Message); ;
            }

 
        }

        public static int Close()
        {
         
            return OctivInterface.octivClose(sensor_handle);
        }

        public static void InitSensor()
        {
            m_sensor_serial_number = "";
            m_sensor_data = new RFCData();
            sensor_handle = 0;
           
        }
    }

    }

