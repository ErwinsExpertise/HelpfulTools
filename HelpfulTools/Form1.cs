using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HelpfulTools
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fqdn = urlBox.Text;
            outBox.Clear();
            try
            {
                if (radioButton1.Checked)
                {
                    outBox.Clear();
                    outBox.AppendText(Worker.hostResolve(fqdn));
                }else if(radioButton2.Checked) {
                    outBox.Clear();
                    outBox.AppendText(Worker.recordResolve(fqdn));
                }else if (radioButton3.Checked)
                {
                    outBox.Clear();
                    outBox.AppendText(Worker.sslCheck(fqdn));
                }

            }//end try
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    

        private void button2_Click(object sender, EventArgs e)
        {
            outBox.Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
