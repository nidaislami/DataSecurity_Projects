using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.Security.Cryptography;
using System.Xml;

namespace TCP_Klienti
{
    public partial class Shpenzimet : Form
    {
       
        public static Socket server;
        static string receivedData;
        private bool Connected = false;
        public string tokeni;

        public Client client;

        private static RSACryptoServiceProvider rsaClient = new RSACryptoServiceProvider();
        private static RSACryptoServiceProvider rsaServer = new RSACryptoServiceProvider();


        private static string publicKeyServer;

        public Shpenzimet(Client c)
        {
            client = c;
            Connected = true;
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        private void SendDataToServer(string data)
        {
            server.Send(Encoding.ASCII.GetBytes(data));
        }

        private void SendRequestToSrv(string teksti)
        {
            try
            {
                client.SendDataToServer(teksti);
                //client.server.Send(Encoding.ASCII.GetBytes(teksti));
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message.ToString());
            }
        }
        protected bool ValidateField(TextBox textbox, string Errormsg, ErrorProvider errorProvider1)
        {

            bool bStatus = true;

            if (textbox.Text == "")
            {
                errorProvider1.SetError(textbox, Errormsg);
                bStatus = false;
            }
            else
            {
                errorProvider1.SetError(textbox, "");
            }
            return bStatus;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!Connected)
                MessageBox.Show("Nuk mund ti ruani te dhenat!", "Vërejtje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            bool bValidLlojifatures = ValidateField(txtLlojifatures, "Lloji fatures duhet të shënohet!", errorProvider1);
            bool bValidViti = ValidateField(txtViti, "Viti duhet të shënohet!", errorProvider1);
            bool bValidMuaji = ValidateField(txtMuaji, "Muaji duhet të shënohet!", errorProvider1);
            bool bValidVleraEuro = ValidateField(txtVleraEuro, "Vlera ne euro duhet të shënohet!", errorProvider1);
           
            if (!bValidLlojifatures || !bValidViti || !bValidMuaji || !bValidVleraEuro )
                MessageBox.Show("Të dhënat janë jo valide!", "Vërejtje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                try
                {
                   string strCommand = "insert into shpenzimet(id, fatura, viti, muaji, total) values (" + "'" + bValidLlojifatures + "' ,'" + bValidViti + "','" + bValidMuaji + "','" + bValidVleraEuro + "')";
                   
                   string SendCommand = "Shpenzimet :" + strCommand;

                    SendCommand = DESEncrypt(SendCommand);
                    SendRequestToSrv(SendCommand);
                    
                   //MySqlCommand sqlCommand = new MySqlCommand(strCommand, connection);
                   //validateResponse(sqlCommand.ExecuteNonQuery());
                   client.appendTxt("");
                   client.appendTxt(client.ReceiveDataFromServer());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Vërejtje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        public string validateResponse(int result)
        {
            if (result > 0) {
                MessageBox.Show("Te dhenat u ruajten me sukses!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return "Te dhenat u ruajten me sukses!";
            }
            else {
                MessageBox.Show("Ruajtja deshtoi!","Error", MessageBoxButtons.OK,MessageBoxIcon.Error);
                return "Ruajtja deshtoi!";
             }
        }

        string DESEncrypt(string data)
        {
            DESCryptoServiceProvider DESalg = new DESCryptoServiceProvider();

            byte[] EncryptedData = MyDES.EncryptTextToMemory(data, DESalg.Key, DESalg.IV);

            byte[] RSAKey = RSA.RSAEncrypt(DESalg.Key, rsaServer.ExportParameters(false), false);

            return Convert.ToBase64String(DESalg.IV) + "*" + Convert.ToBase64String(RSAKey) + "*" + Convert.ToBase64String(EncryptedData);


        }
    }
}
