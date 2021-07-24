using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using MySql.Data.MySqlClient;

namespace Server_TCP
{
    class ConnectionHandler
    {
        private Socket Klienti;
        private static int lidhjet = 0;
        private string eDhenaEardhur;
        private string eDhenaEardhurEnkriptuar;

        private PasswordHash pswhash = new PasswordHash();
        private static RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        private static string publicKey;
        private static RSAParameters privateKey = rsa.ExportParameters(true);

        private string ConnectionString = "Server=localhost;Database=siguri;Uid=root;Pwd=;SslMode=none;";
        private MySqlConnection connection;
        private bool Connected = false;

        public ConnectionHandler(Socket klienti)
        {
            publicKey = rsa.ToXmlString(false);
            this.Klienti = klienti;
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            try
            {
                connection = new MySqlConnection(ConnectionString);
                connection.Open();
                Connected = true;
                SendData("Database Connected");
            }
            catch (Exception ex)
            {
                SendData("Error while opening the database: siguri");
            }
        }

        public void HandleConnection()
        {
            try
            {
                lidhjet++;
                Console.WriteLine("The new client is accepted: {0} active connection", lidhjet);
                SendData(DefaultFunctions());


                while (true)
                {
                    ReceiveData();
                    Console.WriteLine("Encrypted client request:");
                    Console.WriteLine(eDhenaEardhurEnkriptuar);
                    Console.WriteLine("Decrypted client request:");
                    Console.WriteLine(eDhenaEardhur);


                    if (eDhenaEardhur.ToUpper().StartsWith("REGJISTRIMI") || eDhenaEardhur.ToUpper().StartsWith("AUTHENTIFIKIMI"))
                    {
                        Console.WriteLine(eDhenaEardhur);
                        int poz = eDhenaEardhur.IndexOf(" ");
                        string command = eDhenaEardhur.Substring(0, poz);
                        string sjson = eDhenaEardhur.Substring(poz + 1);

                        switch (command.ToUpper())
                        {

                            case "REGJISTRIMI":
                                SendData(Regjistrimi(sjson));
                                break;
                            case "AUTHENTIFIKIMI":
                                SendData(Authentifikimi(sjson));
                                break;
                            default:
                                SendData("The operation is not valid!");
                                break;
                        }
                    }

                }
                this.Klienti.Close();
                lidhjet--;
                Console.WriteLine("The client is disconnected: {0} lidhje aktive", lidhjet);
            }
            catch (Exception)
            {
                lidhjet--;
                Console.WriteLine("Klienti është shkëputur: {0} lidhje aktive", lidhjet);
            }
        }

      

        private void SendData(string data)
        {
            data = DESEncrypt(data);
            this.Klienti.Send(Encoding.ASCII.GetBytes(data));
        }

        private string ReceiveData()
        {
            byte[] data = new byte[4092];
            int recv = this.Klienti.Receive(data);
            string stringData = Encoding.ASCII.GetString(data, 0, recv);
            eDhenaEardhurEnkriptuar = stringData;

            byte[] RSAkey;
            byte[] EncryptedData;
            byte[] IV;
            byte[] key;
            string[] stringDataList = stringData.Split('*');
            IV = Convert.FromBase64String(stringDataList[0]);
            RSAkey = Convert.FromBase64String(stringDataList[1]);
            EncryptedData = Convert.FromBase64String(stringDataList[2]);
            key = RSA.RSADecrypt(RSAkey, privateKey, false);
            stringData = DES.DecryptTextFromMemory(EncryptedData, key, IV);

            eDhenaEardhur = stringData;
            return stringData;
        }

        private string DefaultFunctions()
        {
            //string allFunctions = "\nKomandat e mundshme \n(REGJISTRIMI, AUTHENTIFIKIMI)";
            string allFunctions = publicKey;
            return allFunctions;
        }

        //Metodat
        private string Regjistrimi(string sjson)
        {
            string return_value = "";
            bool UserExist = false;
            try
            {
                //Shfrytezuesi Shfrytezuesi = js.Deserialize<Shfrytezuesi>(sjson);
                Shfrytezuesi shfrytezuesi = JsonConvert.DeserializeObject<Shfrytezuesi>(sjson);
                string Hashpassword = pswhash.CreateHash(shfrytezuesi.PasswordHash);
                shfrytezuesi.PasswordHash = Hashpassword;

                string path = "Shfrytezuesi.json";
                if (File.Exists(path))
                {
                    sjson = File.ReadAllText(path);
                    if (String.IsNullOrEmpty(sjson))
                    {
                        List<Shfrytezuesi> lstShfrytezuesit = new List<Shfrytezuesi>();
                        lstShfrytezuesit.Add(shfrytezuesi);
                        File.WriteAllText(path, JsonConvert.SerializeObject(lstShfrytezuesit), Encoding.UTF8);
                    }
                    else
                    {
                        List<Shfrytezuesi> lstShfrytezuesit = JsonConvert.DeserializeObject<List<Shfrytezuesi>>(sjson);
                        foreach (Shfrytezuesi item in lstShfrytezuesit)
                        {
                            if (item.userId == shfrytezuesi.userId)
                            {
                                return_value = "ERROR - Ekziston shfrytezuesi me username " + shfrytezuesi.userId;
                                UserExist = true;
                                break;
                            }
                        }
                        if (!UserExist)
                        {
                            lstShfrytezuesit.Add(shfrytezuesi);
                            File.WriteAllText(path, JsonConvert.SerializeObject(lstShfrytezuesit), Encoding.UTF8);
                            return_value = "OK - Jeni regjistruar me sukses me username " + shfrytezuesi.userId;
                        }
                    }
                }
                else
                {
                    List<Shfrytezuesi> lstShfrytezuesit = new List<Shfrytezuesi>();
                    lstShfrytezuesit.Add(shfrytezuesi);
                    File.WriteAllText(path, JsonConvert.SerializeObject(lstShfrytezuesit), Encoding.UTF8);
                    return_value = "OK - Jeni regjistruar me sukses me username " + shfrytezuesi.userId;
                }
            }
            catch (Exception ex)
            {
                return_value = ex.Message;
            }

            return return_value;
        }
        private string RegjistrimiShpenzimeve(string sjson)
        {
            string return_value = "";
            bool UserExist = false;
            try
            {
                Shpenzimet shpenzimet = JsonConvert.DeserializeObject<Shpenzimet>(sjson);
          

                string path = "Shpenzimet.json";
                if (File.Exists(path))
                {
                    sjson = File.ReadAllText(path);
                    if (String.IsNullOrEmpty(sjson))
                    {
                        List<Shpenzimet> lstShpenzimett = new List<Shpenzimet>();
                        lstShpenzimett.Add(shpenzimet);
                        File.WriteAllText(path, JsonConvert.SerializeObject(lstShpenzimett), Encoding.UTF8);
                    }
                    else
                    {
                        List<Shpenzimet> lstShpenzimett = JsonConvert.DeserializeObject<List<Shpenzimet>>(sjson);
                        foreach (Shpenzimet item in lstShpenzimett)
                        {
                            if (item.userId == shpenzimet.userId)
                            {
                                return_value = "ERROR - Ekzistojne keto shpenzime " + shpenzimet.userId;
                                UserExist = true;
                                break;
                            }
                        }
                        
                    }
                }
                else
                {
                    List<Shpenzimet> lstShpenzimett = new List<Shpenzimet>();
                    lstShpenzimett.Add(shpenzimet);
                    File.WriteAllText(path, JsonConvert.SerializeObject(lstShpenzimett), Encoding.UTF8);
                    return_value = "OK - Jane regjistruar me sukses te dhenat " + shpenzimet.userId;
                }
            }
            catch (Exception ex)
            {
                return_value = ex.Message;
            }

            return return_value;
        }





        private string Authentifikimi(string sjson)
        {
            string return_value = "";

            Login login = JsonConvert.DeserializeObject<Login>(sjson);
            string password = login.PasswordHash;
            string UserId = login.userId;
            bool UserExist = false;
            string path = "Shfrytezuesi.json";
            if (File.Exists(path))
            {
                sjson = File.ReadAllText(path, Encoding.UTF8);
                List<Shfrytezuesi> lstShfrytezuesit = JsonConvert.DeserializeObject<List<Shfrytezuesi>>(sjson);

                foreach (var item in lstShfrytezuesit)
                {
                    if (item.userId == UserId)
                    {
                        UserExist = true;
                        Jwt jwt = new Jwt();
                        if (pswhash.ValidatePassword(password, item.PasswordHash))
                        {
                            return_value = "\nOK - Jeni autentifikuar me sukses \n" + "@" + jwt.createToken(item.userId, item.email, item.emriMbiemri, item.paga, item.profesioni);
                        }
                        else
                        {
                            return_value = "ERROR - Passwordi eshte gabim!";
                        }
                        break;
                    }
                }
                if (!UserExist)
                {
                    return_value = "ERROR - User-i " + login.userId + " nuk ekziston!";
                }
            }
            else
            {
                return_value = "ERROR - User-i " + login.userId + " nuk ekziston!";
            }

            return return_value;
        }
        string DESEncrypt(string data)
        {
            DESCryptoServiceProvider DESalg = new DESCryptoServiceProvider();
            byte[] EncryptedData = DES.EncryptTextToMemory(data, DESalg.Key, DESalg.IV);
            byte[] Key = DESalg.Key;
            return Convert.ToBase64String(DESalg.IV) + "*" + Convert.ToBase64String(Key) + "*" + Convert.ToBase64String(EncryptedData);
        }
    }
}
