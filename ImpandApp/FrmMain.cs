using Continuous;
using ImpandApp.Properties;
using OctivLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImpandApp
{
    public partial class FrmMain : Form
    {
        bool isStop = false;
        PowerModel selectedPowermodel = null;
        BindingList<PowerModel> freq_lst = new BindingList<PowerModel>();
        List<RFCData> sensor_lst = new List<RFCData>();
   
        int index_power = 1;
        public FrmMain()
        {
            InitializeComponent();        
        }

        protected override void OnClosing(CancelEventArgs e)
        {
         //   SensorService.Close();
        }

        protected async  override void OnLoad(EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            progressBar1.Visible = false;
            toggleSwitch1.CheckedChanged += ToggleSwitch1_CheckedChanged;
            comboBox1.SelectedIndexChanged += (s, d) => { selectedPowermodel = (PowerModel)(s as ComboBox).SelectedItem; };
            var port1 = Properties.Settings.Default.ModbusSerialPort;
            var port2 = Properties.Settings.Default.DMMSerialPort;
            var port3 = Properties.Settings.Default.DMMSerialPort2;
            var port4 = Properties.Settings.Default.DCSerialPort;
            var interval = Properties.Settings.Default.PowerInterval;
            InitInput();
            initListView();
            timer1.Enabled = false;
            timer1.Interval = Settings.Default.PowerInterval;
            timer1.Tick += Timer1_Tick;

            timer2.Enabled = false;
            timer2.Interval = Settings.Default.DcInterval;
            timer2.Tick += Timer2_Tick;
          
            SerialService.OnDmmDataArrial += SerialService_OnDmmDataArrial;
            SerialService8.OnDmmDataArrial += SerialService8_OnDmmDataArrial;
            ModbusService.Portname =  port1;
            SerialService.Portname =  port2;
            SerialService8.Portname =  port3;
            DCSerialSerivce.Portname = port4;
            await RunService();

        }

    

        private void Timer2_Tick(object sender, EventArgs e)
        {
           
            Addtolist();
        }

        private void Addtolist()
        {
            Dcdata dcdata = new Dcdata();
            dcdata.VoltageRead = CheckedRadio(groupBox4) + "V";
            switch (CheckedRadio(groupBox6))
            {
                case "1":
                    dcdata.VoltageSet = "P25V";
                    break;
                case "2":
                    dcdata.VoltageSet = "N25V";
                    break;
                case "3":
                    dcdata.VoltageSet = "P6V";
                    break;
            }
            Updatedclist(dcdata);
        }

        private void SerialService8_OnDmmDataArrial(object sender, EventArgs e)
        {
            try
            {

                DmmEventArgs receiveData = (DmmEventArgs)e;
                listBox2.Items.Add(receiveData.Voltage);
              
                if (listBox2.Items.Count > 15)
                    listBox2.Items.Clear();
             }
            catch (Exception ex)
            {

                LogService.LogMessage("SerialService_OnDmmDataArrial8: " + ex.Message);
            }

        }

        private async Task RunService()
        {
            var task = new Task(() => SensorService.Start(Displaysensordata));
            await Task.Run(() => ModbusService.Open());
            await Task.Run(() => SerialService.Connect());
            await Task.Run(() => SerialService8.Connect());
            await Task.Run(() => DCSerialSerivce.Connect());
            radioButton2.Checked = ModbusService.IsOpen();
            radioButton2.Text = ModbusService.IsOpen() ? "Connected" : "Disconnected";
            radioButton3.Checked = SerialService.IsOpen();
            radioButton3.Text = SerialService.IsOpen() ? "Connected" : "Disconnected";
            radioButton7.Checked = SerialService8.IsOpen();
            radioButton7.Text = SerialService8.IsOpen() ? "Connected" : "Disconnected";
            task.Start();
      
        }    
        private async void Timer1_Tick(object sender, EventArgs e)
        {
             if(index_power<freq_lst.Count-1)
            {
                PowerModel p = freq_lst[index_power];
                var b= await Task.Run(() => ModbusService.SetPower(p.Testingvalue));
                if (b)
                    index_power++;
                else
                    timer1.Enabled = false;
                comboBox1.SelectedIndex = index_power;

            }

        }

        private void initListView()
        {
         
            listView4.View = View.Details;
            listView4.Columns.Add("时间", 100, HorizontalAlignment.Center);
            listView4.Columns.Add("设置电压", -2, HorizontalAlignment.Center);
            listView4.Columns.Add("输出电压", -2, HorizontalAlignment.Center);
        }

     

        private void SerialService_OnDmmDataArrial(object sender, EventArgs e)
        {
            try
            {
                DmmEventArgs receiveData = (DmmEventArgs)e;
                listBox1.Items.Add(receiveData.Voltage);
                if (listBox1.Items.Count > 15)
                    listBox1.Items.Clear();
        
            }
         
            catch (Exception ex)
            {

                LogService.LogMessage("SerialService_OnDmmDataArrial: " + ex.Message);
            }


        }

        private void InitInput()
        {
            freq_lst.Clear();
            freq_lst.Add(new PowerModel() { Settingvalue = "20W",Testingvalue=15 });
            freq_lst.Add(new PowerModel() { Settingvalue = "50W", Testingvalue = 47 });
            freq_lst.Add(new PowerModel() { Settingvalue = "80W", Testingvalue = 79 });
            freq_lst.Add(new PowerModel() { Settingvalue = "100W", Testingvalue = 100 });
            freq_lst.Add(new PowerModel() { Settingvalue = "200W", Testingvalue = 201 });
            freq_lst.Add(new PowerModel() { Settingvalue = "300W", Testingvalue = 302 });
            freq_lst.Add(new PowerModel() { Settingvalue = "400W", Testingvalue = 401 });
            freq_lst.Add(new PowerModel() { Settingvalue = "500W", Testingvalue = 501 });
            freq_lst.Add(new PowerModel() { Settingvalue = "600W", Testingvalue = 599 });
            freq_lst.Add(new PowerModel() { Settingvalue = "700W", Testingvalue = 698 });
            freq_lst.Add(new PowerModel() { Settingvalue = "800W", Testingvalue = 797 });
            freq_lst.Add(new PowerModel() { Settingvalue = "1000W", Testingvalue = 995 });

            comboBox1.DataSource = freq_lst;
            comboBox1.DisplayMember = "Settingvalue";
            comboBox1.ValueMember = "Testingvalue";
        }

        private async void ToggleSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as JCS.ToggleSwitch).Checked)
                await OpenRF();
            else
                await CloseRF();
        }

        public async Task<bool> OpenRF()
        {
            if (comboBox1.SelectedItem != null)
            {
                int inputPower = (comboBox1.SelectedItem as PowerModel).Testingvalue;
                return await Task.Run(() => ModbusService.OpenRF(inputPower));
            }
            return false;
        }

        public async Task<bool> CloseRF()
        {
            return await Task.Run(() => ModbusService.CloseRF());
        }

   
       
       private void Displaysensordata(RFCData data)
        {
            if (data!=null)
            {
               

                    radioButton1.Checked = data.IsConnected;
                    label9.Text = data.Serialnumber;
                    label10.Text = data.Pfrequency.ToString() + " MHZ";
                    label11.Text = data.Phomonic.ToString() + " Channel";
                    textBox1.Text = data.Frequency;
                    textBox2.Text = data.Voltage;
                    textBox3.Text = data.PhaseV;
                    textBox4.Text = data.Current;
                    textBox5.Text = data.Phase;
                    Resetcombovalue(data);
                   if(!isStop)
                   Addtosensorlst(data);
              

            }
        }

        private void Addtosensorlst(RFCData data)
        {
           
                RFCData _data = new RFCData();
                _data.Serialnumber = data.Serialnumber;
                _data.Pfrequency = data.Pfrequency;
                _data.Phomonic = data.Phomonic;
                _data.Frequency = data.Frequency;
                _data.Voltage = data.Voltage;
                _data.PhaseV = data.PhaseV;
                _data.Current = data.Current;
                _data.Phase = data.Phase;
                _data.PowerSet = freq_lst[index_power].Settingvalue;
                _data.Footer6v = "0";
                _data.Footer8v = "0";
                if (listBox1.Items.Count > 0)
                    _data.Footer6v = listBox1.Items[listBox1.Items.Count - 1].ToString();
                if (listBox2.Items.Count > 0)
                    _data.Footer8v = listBox2.Items[listBox2.Items.Count - 1].ToString();
                sensor_lst.Add(_data);
           
        }




        private void Resetcombovalue(RFCData data)
        {
            if (comboBox3.Items.Count == 0)
            {
                if (!string.IsNullOrEmpty(data.Phomonic.ToString()))
                {
                    comboBox3.SelectedIndexChanged += (sender, k) =>
                    {
                        ComboBox cmb = (ComboBox)sender;
                    };
                    comboBox3.Items.Clear();
                    for (int i = 0; i < int.Parse(data.Phomonic.ToString()); i++)
                        comboBox3.Items.Add((i + 1).ToString());
                }
            }
        }

      
        private async  void button1_Click(object sender, EventArgs e)
        {
            try
            {
                isStop = false;
                progressBar1.Visible = false;
                timer1.Interval = Settings.Default.PowerInterval;
                timer2.Interval = Settings.Default.DcInterval;
                button1.Enabled = false;
                index_power = 1;
                await Task.Run(() => SerialService.Set_PC());
                await Task.Delay(100);
                await Task.Run(() => SerialService.Set_Session());
                await Task.Delay(100);
                await Task.Run(()=>SerialService8.Set_Session());
                await Task.Delay(100);
                var b = await Task.Run(() => ModbusService.SetPower(selectedPowermodel.Testingvalue));
                if (b)
                    timer1.Enabled = true;
                var task1 = new Task(() => SerialService.Read_Data());
                var task2 = new Task(() => SerialService8.Read_Data());
                task1.Start();
                task2.Start();
               
            }
            catch (OperationCanceledException)
            {

                
            }
          
      
        }

        private void ClearTextBox()
        {
            foreach (Control control in groupBox3.Controls)
                if (control is TextBox)
                    (control as TextBox).Text = "";
            foreach (Control control in groupBox2.Controls)
                if (control is TextBox)
                    (control as TextBox).Text = "";
            foreach (Control control in groupBox5.Controls)
                if (control is TextBox)
                    (control as TextBox).Text = "";
        }

        private  void button2_Click(object sender, EventArgs e)
        {
            progressBar1.Visible = false;
            button1.Enabled = true;
            timer1.Enabled = false;
            isStop = true;
            comboBox1.SelectedIndex = 0;
            toggleSwitch1.Checked = false;
            ClearTextBox();
            SerialService.Reset();
            SerialService8.Reset();
          }

        private async void button4_Click(object sender, EventArgs e)
        {
            isStop = false;
            timer2.Enabled = false;
            listView4.Items.Clear();
            if (string.IsNullOrEmpty(CheckedRadio(groupBox6)))
            {
                MessageBox.Show("Please select channel");
                return;
            }
            int delay_time = Properties.Settings.Default.DcInterval;
            for (int i = 0; i < 11; i++)
            {
              
                RadioButton rb = groupBox4.Controls.Find("radioButton_" + i, true).FirstOrDefault() as RadioButton;
                if (rb != null)
                    rb.Checked = true;
                bool b = await Task.Run(() => DCSerialSerivce.SetVoltage(i, int.Parse(rb.Tag.ToString())));
                if (b)
                    Addtolist();
                   
               await Task.Delay(delay_time);
            }


        }

        private void Updatelist(float reading)
        {
            ListViewItem itm;
          
            string[] arr = new string[3];
            arr[0] = DateTime.Now.ToString("mm:ss");
            arr[1] = CheckedRadio(groupBox4);
            arr[2] = reading.ToString();
            itm = new ListViewItem(arr);
            listView4.Items.Add(itm);
            if (listView4.Items.Count > 11) 
                listView4.Items.Clear();
           
        }

        private void Updatedclist(Dcdata dc)
        {
            if (!isStop)
            {
                ListViewItem itm;

                string[] arr = new string[3];
                arr[0] = DateTime.Now.ToString("mm:ss");
                arr[1] = dc.VoltageSet.ToString();
                arr[2] = dc.VoltageRead;
              
                itm = new ListViewItem(arr);
                listView4.Items.Add(itm);
                if (listView4.Items.Count > 11)
                    listView4.Items.Clear();
            }
        }
        
        private string CheckedRadio(GroupBox groupBox)
        {
           

            foreach(var control in groupBox.Controls)
            {
                 if ((control is RadioButton) && (control as RadioButton).Checked)
                  return (control as RadioButton).Tag.ToString();
             }

            return string.Empty;
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            isStop = false;
            timer2.Enabled = false;
            listView4.Items.Clear();
            radioButton_0.Checked = true;
            if (string.IsNullOrEmpty(CheckedRadio(groupBox6)))
            {
                MessageBox.Show("Please select channel");
                return;
            }
            int selectedChannel = int.Parse(CheckedRadio(groupBox6));
            if(await Task.Run(() => DCSerialSerivce.SetVoltage(0f,selectedChannel)))
            {
                DCSerialSerivce.GetVoltage();
                timer2.Enabled = true;
            }
        }
     

        private void 串口设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmSettings f = new FrmSettings();
            f.ShowDialog();
        }

        private async void 导入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                isStop = true;
                Thread.Sleep(500);
                if (tabControl1.SelectedIndex == 0)
                {
                    progressBar1.Visible = true;
                    Progress<ProgressReportModel> progress = new Progress<ProgressReportModel>();
                    progress.ProgressChanged += Progress_ProgressChanged;
                    string fileName = "传感器.xlsx";
                    string location = @"d:\reports";
                    string customExcelSavingPath = location + "\\" + fileName;
                    await ExcelExport.GenerateExcel(ConvertToDataTable(sensor_lst), customExcelSavingPath, progress);
                    MessageBox.Show("Report Created");
                   
                }

            }
            catch (Exception ex)
            {
                LogService.LogMessage("report:" + ex.Message);
            }
       
      
        }

        private void Progress_ProgressChanged(object sender, ProgressReportModel e)
        {
            progressBar1.Value = e.PercentageComplete;
        }

        DataTable ConvertToDataTable<T>(List<T> models)
        {
           DataTable dataTable = new DataTable(typeof(T).Name);
           PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in models)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                 dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
             

                //string path = @"d:\reports\传感器.xlsx";
                //File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                string fileName = "传感器.xlsx";
                string location = @"d:\reports";
                string customExcelSavingPath = location + "\\" + fileName;
                Process.Start(customExcelSavingPath);
            }
            catch (Exception ex)
            {

                LogService.LogMessage(ex.Message);
            }
        }

        private async void button6_Click_1(object sender, EventArgs e)
        {
            try
            {
                isStop = false;
                timer2.Enabled = false;
                listView4.Items.Clear();
                radioButton_4.Checked = true;
          
                if (string.IsNullOrEmpty(CheckedRadio(groupBox6)))
                {
                    MessageBox.Show("Please select channel");
                    return;
                }
                int selectedChannel =int.Parse(CheckedRadio(groupBox6));
                if (await Task.Run(() => DCSerialSerivce.SetVoltage(4f, selectedChannel)))
                {
                    DCSerialSerivce.GetVoltage();
                    timer2.Enabled = true;
                }
                   
                  
            }
            catch (Exception ex)
            {

                LogService.LogMessage(ex.Message);
            }
        }


        private void button7_Click(object sender, EventArgs e)
        {
            isStop = true;
            timer2.Enabled = false;
            ClearCheckedRadio();

        }

        private void ClearCheckedRadio()
        {
            foreach (var control in groupBox6.Controls)
                (control as RadioButton).Checked = false;
            foreach (var control in groupBox4.Controls)
                (control as RadioButton).Checked = false;
        }

        private void 间隔设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmTimeSetting f = new FrmTimeSetting();
            f.ShowDialog();
        }
    }
}
