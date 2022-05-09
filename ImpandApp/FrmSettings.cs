using Continuous;
using OctivLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImpandApp
{
    public partial class FrmSettings : Form
    {
      
        public FrmSettings()
        {
            InitializeComponent();
        }

        private void BindPortList(ComboBox comboBox,List<string> portList)
        {
            comboBox.DataSource = portList;
            switch (comboBox.Name)
            {
                case "comboBox1":
                    var port1 = Properties.Settings.Default.ModbusSerialPort;
                    if (string.IsNullOrEmpty(port1))
                        comboBox.SelectedIndex = 0;
                    else
                        comboBox.SelectedIndex = comboBox.FindStringExact(port1);
                    break;
                case "comboBox2":
                    var port2 = Properties.Settings.Default.DMMSerialPort;
                    if (string.IsNullOrEmpty(port2))
                        comboBox.SelectedIndex = 0;
                    else
                        comboBox.SelectedIndex = comboBox.FindStringExact(port2);
                    break;
                case "comboBox3":
                    var port3 = Properties.Settings.Default.DMMSerialPort2;
                    if (string.IsNullOrEmpty(port3))
                        comboBox.SelectedIndex = 0;
                    else
                        comboBox.SelectedIndex = comboBox.FindStringExact(port3);
                    break;
                case "comboBox4":
                    var port4 = Properties.Settings.Default.DCSerialPort;
                    if (string.IsNullOrEmpty(port4))
                        comboBox.SelectedIndex = 0;
                    else
                        comboBox.SelectedIndex = comboBox.FindStringExact(port4);
                    break;
                default:
                    comboBox.SelectedIndex = 0;
                    break;

            }
        }

        protected override void OnLoad(EventArgs e)
        {
          
  
            BindPortList(comboBox1, Getserialport());
            BindPortList(comboBox2, Getserialport());
            BindPortList(comboBox3, Getserialport());
            BindPortList(comboBox4, Getserialport());
        }

        private List<string> Getserialport()
        {
            return SerialPort.GetPortNames().ToList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ModbusSerialPort = comboBox1.SelectedItem.ToString();
            Properties.Settings.Default.DMMSerialPort = comboBox2.SelectedItem.ToString();
            Properties.Settings.Default.DMMSerialPort2 = comboBox3.SelectedItem.ToString();
            Properties.Settings.Default.DCSerialPort = comboBox4.SelectedItem.ToString();
            Properties.Settings.Default.Save();
            MessageBox.Show("Ports Saved!");
        }
    }
}
