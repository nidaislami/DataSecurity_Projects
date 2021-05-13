using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projekti1._1_TripleDes
{


    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtEncrypt.Text = Encrypt(txtInput.Text, txtKey.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
