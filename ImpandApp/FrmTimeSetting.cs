using ImpandApp.Properties;
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
    public partial class FrmTimeSetting : Form
    {
        public FrmTimeSetting()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            textBox1.Text = Settings.Default.PowerInterval.ToString();
            textBox2.Text = Settings.Default.DcInterval.ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text)) return;
            if (string.IsNullOrEmpty(textBox2.Text)) return;

            var isNumeric = int.TryParse(textBox1.Text, out _);
            if (isNumeric)
                Settings.Default.PowerInterval = int.Parse(textBox1.Text);
            else
                MessageBox.Show("RF interval not set");
             isNumeric = int.TryParse(textBox2.Text, out _);
            if (isNumeric)
              Settings.Default.DcInterval = int.Parse(textBox2.Text);
            else
             MessageBox.Show("DF interval not set");
            Properties.Settings.Default.Save();
            MessageBox.Show("Interval Set!");
           this.Close();
           }
    }
}
