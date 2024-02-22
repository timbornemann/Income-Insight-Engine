using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace IncomeInsightEngine.src.dataStructure.management
{
    public class FileEncryptor
    {
        private readonly string keyFilePath = "encryptionKey.bin";
        private readonly string ivFilePath = "encryptionIV.bin";

        public FileEncryptor()
        {

            string keyFilePathappDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            keyFilePath = Path.Combine(keyFilePathappDataPath, "IncomeInsightEngine", "encryptionKey.bin");

            string ivFilePathappDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            ivFilePath = Path.Combine(ivFilePathappDataPath, "IncomeInsightEngine", "encryptionIV.bin");

            InitializeEncryptionKeys();
        }

        private void InitializeEncryptionKeys()
        {
            if (!File.Exists(keyFilePath) || !File.Exists(ivFilePath))
            {
                using (Aes aesAlg = Aes.Create())
                {
                    ProtectAndSave(aesAlg.Key, keyFilePath);
                    ProtectAndSave(aesAlg.IV, ivFilePath);
                }
            }
        }

        private void ProtectAndSave(byte[] data, string filePath)
        {
            // Verschlüsselt die Daten mit DPAPI und speichert sie in der Datei.
            byte[] protectedData = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
            File.WriteAllBytes(filePath, protectedData);
        }

        private byte[] UnprotectAndLoad(string filePath)
        {
            // Lädt die Daten aus der Datei und entschlüsselt sie mit DPAPI.
            byte[] protectedData = File.ReadAllBytes(filePath);
            return ProtectedData.Unprotect(protectedData, null, DataProtectionScope.CurrentUser);
        }

        public void EncryptFile(string filePath)
        {
            string dataToEncrypt = File.ReadAllText(filePath);
            byte[] key = UnprotectAndLoad(keyFilePath);
            byte[] IV = UnprotectAndLoad(ivFilePath);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
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
            byte[] key = UnprotectAndLoad(keyFilePath);
            byte[] IV = UnprotectAndLoad(ivFilePath);

            string decryptedData = File.ReadAllText(filePath);
           
            try
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = key;
                    aesAlg.IV = IV;

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                    using (CryptoStream cryptoStream = new CryptoStream(fileStream, decryptor, CryptoStreamMode.Read))
                    using (StreamReader decryptReader = new StreamReader(cryptoStream))
                    {
                        decryptedData = decryptReader.ReadToEnd();
                                          
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
            File.WriteAllText(filePath, decryptedData);
            return decryptedData;
        }
    }
}
