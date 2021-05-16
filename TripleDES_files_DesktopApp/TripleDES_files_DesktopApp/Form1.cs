using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TripleDES_files_DesktopApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog OD = new OpenFileDialog();
            OD.Filter = "All Files|*";
            OD.FileName = " ";
            if (OD.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = OD.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                TripleDES tDES = new TripleDES(textBox2.Text);
                tDES.EncryptFile(textBox1.Text);
                GC.Collect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                TripleDES tDES = new TripleDES(textBox2.Text);
                tDES.DecryptFile(textBox1.Text);
                GC.Collect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {

                TripleDES tDES = new TripleDES(textBox2.Text);
                textBox4.Text = tDES.EncryptText(textBox3.Text, textBox2.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                TripleDES tDES = new TripleDES(textBox2.Text);
                textBox5.Text = tDES.DecryptText(textBox4.Text, textBox2.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
