using System;
using System.IO;
using System.Security.Cryptography;





class TripleDES1
{
    public static void Main()
    {
        Console.WriteLine("Choose our option text/file.\n If you want to encrypt/decrypt a text type text, else type file: ");
        string data = Console.ReadLine();
        if (data == "text")
        {
            Console.WriteLine("\nEnter text that needs to be encrypted: ");
            string text = Console.ReadLine();


            Apply3DES(text);
            Console.ReadLine();
        }
        else
        {
            try
            {
                Console.WriteLine("Enter the name/path of the file to encrypt: ");
                string theFile = Console.ReadLine();

                byte[] key;
                byte[] iv;

                using (SymmetricAlgorithm alg = SymmetricAlgorithm.Create("3DES"))
                {
                    key = alg.Key;
                    iv = alg.IV;
                }

                EncryptFile(theFile, "encrypted" + theFile, (byte[])key.Clone(), (byte[])iv.Clone());

                DecryptFile("encrypted" + theFile, "decrypted" + theFile, key, iv);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }




    static void Apply3DES(string given)
    {
        try
        {

            using (TripleDESCryptoServiceProvider td = new TripleDESCryptoServiceProvider())
            {

                byte[] encrypted1 = Encrypt(given, td.Key, td.IV);
                Console.WriteLine("Encrypted input:" + System.Text.Encoding.UTF8.GetString(encrypted1));
                string decrypted = Decrypt(encrypted1, td.Key, td.IV);
                Console.WriteLine("Decrypted input: " + decrypted);
            }
        }
        catch (Exception exp)
        {
            Console.WriteLine(exp.Message);
        }
        Console.ReadKey();
    }
    static byte[] Encrypt(string plainText, byte[] Key, byte[] IV)
    {
        byte[] encrypted1;
        using (TripleDESCryptoServiceProvider td = new TripleDESCryptoServiceProvider())
        {

            ICryptoTransform encryptor = td.CreateEncryptor(Key, IV);
            using (MemoryStream ms = new MemoryStream())
            {

                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {

                    using (StreamWriter sw = new StreamWriter(cs))
                        sw.Write(plainText);
                    encrypted1 = ms.ToArray();
                }
            }
        }
        return encrypted1;
    }
    static string Decrypt(byte[] cipherText, byte[] Key, byte[] IV)
    {
        string plaintext = null;
        using (TripleDESCryptoServiceProvider td = new TripleDESCryptoServiceProvider())
        {
            ICryptoTransform decryptor = td.CreateDecryptor(Key, IV);
            using (MemoryStream ms = new MemoryStream(cipherText))
            {

                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader reader = new StreamReader(cs))
                        plaintext = reader.ReadToEnd();
                }
            }
        }
        return plaintext;
    }
  
    //files code part 

    static void EncryptFile(string srcFileName, string destFileName, byte[] key, byte[] iv)
    {

        Stream srcFile = new FileStream(srcFileName, FileMode.Open, FileAccess.Read);
        Stream destFile = new FileStream(destFileName, FileMode.Create, FileAccess.Write);

        using (SymmetricAlgorithm alg = SymmetricAlgorithm.Create("3DES"))
        {
            try
            {
                alg.Key = key;
                alg.IV = iv;


                CryptoStream cryptoStream = new CryptoStream(srcFile, alg.CreateEncryptor(), CryptoStreamMode.Read);

                int bufferLength;
                byte[] buffer = new byte[1024];

                do
                {
                    bufferLength = cryptoStream.Read(buffer, 0, 1024);
                    destFile.Write(buffer, 0, bufferLength);

                } while (bufferLength > 0);


                destFile.Flush();
                Array.Clear(key, 0, key.Length);
                Array.Clear(iv, 0, iv.Length);
                cryptoStream.Clear();
                srcFile.Close();
                destFile.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }


    static void DecryptFile(string srcFileName, string destFileName, byte[] key, byte[] iv)
    {
        try
        {
            Stream srcFile = new FileStream(srcFileName, FileMode.Open, FileAccess.Read);
            Stream destFile = new FileStream(destFileName, FileMode.Create, FileAccess.Write);



            using (SymmetricAlgorithm alg = SymmetricAlgorithm.Create("3DES"))
            {

                alg.Key = key;
                alg.IV = iv;

                CryptoStream cryptoStream = new CryptoStream(destFile, alg.CreateDecryptor(), CryptoStreamMode.Write);

                int bufferLength;
                byte[] buffer = new byte[1024];

                do
                {
                    bufferLength = srcFile.Read(buffer, 0, 1024);
                    cryptoStream.Write(buffer, 0, bufferLength);
                } while (bufferLength > 0);


                cryptoStream.FlushFinalBlock();

                Array.Clear(key, 0, key.Length);
                Array.Clear(iv, 0, iv.Length);
                cryptoStream.Clear();
                cryptoStream.Close();
                srcFile.Close();
                destFile.Close();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}






