using Continuous;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImpandApp
{
    public partial class Form1 : Form
    {
        private DataAccess da = new DataAccess();
       
        string sensor_serial = "";
        int sensor_handle = 0;
        int sensor_frequencie_count=0;
     
        public Form1()
        {
            InitializeComponent();
        }

        protected async override void OnLoad(EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            if (await IniteSensor())
            {
                timer1.Enabled = false;
                timer1.Interval = 1000;
                timer1.Tick += Timer1_Tick;
             }

            checkBox1.CheckedChanged += (s, o) =>
            {
                checkBox1.Text = checkBox1.Checked ? "RF ON" : "RF OFF";
                checkBox1.ForeColor = checkBox1.Checked ? Color.Red : Color.Gray;
            };

        }

       

        private async void Timer1_Tick(object sender, EventArgs e)
        {
              await da.octivGetData(sensor_handle, ShowSensorData);

        }

        private void ShowSensorData(OutData outData)
        {
            textBox5.Text = outData.Frequency.ToString();
            textBox6.Text = outData.PhaseV.ToString();
            textBox7.Text = outData.Voltage.ToString();
            textBox8.Text = outData.Current.ToString();
            textBox9.Text = outData.Phase.ToString();
           
        }

        public async Task<bool> IniteSensor()
        {
            try
            {
                  sensor_handle = await da.OpenPort();
                if (sensor_handle > 0)
                    MessageBox.Show("Sensor Connected!");
                else
                {
                    MessageBox.Show("no sensor connected");
                    return false;
                }
                sensor_serial = await da.octivGetSerial(sensor_handle);
                if (sensor_serial != null)
                    label19.Text = sensor_serial;

                sensor_frequencie_count = await da.octivGetFrequencyCount(sensor_handle);
                if (sensor_frequencie_count > 0)
                {
                    RawData rawdata = await da.octivGetFrequencyDetails(sensor_handle, sensor_frequencie_count);
                    if (rawdata != null)
                    {
                        textBox1.Text = rawdata.FreqVal.ToString();
                        textBox2.Text = rawdata.HarmCount.ToString();
                    }
                    comboBox1.Items.Clear();
                    comboBox1.Items.Add(sensor_frequencie_count.ToString());
                    comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
                }
                else
                {
                    MessageBox.Show("no sensor frequencies found");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private  void button3_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        

        private  void button1_Click(object sender, EventArgs e)
        {
            
             
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            await da.ClosePort(sensor_handle);
        }

        private async void button6_Click(object sender, EventArgs e)
        {
          
        }
    }
}
