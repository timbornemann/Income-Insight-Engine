using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace IncomeInsightEngine.src.dataStructure.management
{
    public class FileEncryptor
    {
        // Die Methoden benötigen einen Schlüssel und einen Initialisierungsvektor (IV).
        // Diese sollten sicher gespeichert und gehandhabt werden.
        private readonly byte[] Key = Encoding.UTF8.GetBytes("6cuamzua12kmp3xy"); // Sollte 16 Bytes (128 Bit) sein
        private readonly byte[] IV = Encoding.UTF8.GetBytes("5618395375629671"); // Sollte 16 Bytes (128 Bit) sein

        public void EncryptFile(string filePath, string dataToEncrypt)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                using (CryptoStream cryptoStream = new CryptoStream(fileStream, encryptor, CryptoStreamMode.Write))
                using (StreamWriter encryptWriter = new StreamWriter(cryptoStream))
                {
                    encryptWriter.Write(dataToEncrypt);
                }
            }
        }

        public string DecryptFile(string filePath)
        {

            string data = File.ReadAllText(filePath);
            try
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Key;
                    aesAlg.IV = IV;

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                    using (CryptoStream cryptoStream = new CryptoStream(fileStream, decryptor, CryptoStreamMode.Read))
                    using (StreamReader decryptReader = new StreamReader(cryptoStream))
                    {
                        return decryptReader.ReadToEnd();
                    }
                }
            }
            catch (CryptographicException ex)
            {
                
                Console.WriteLine("A cryptographic error occurred: " + ex.Message);
               
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            File.WriteAllText(filePath, data);
            return data; 
        }
    }
}
