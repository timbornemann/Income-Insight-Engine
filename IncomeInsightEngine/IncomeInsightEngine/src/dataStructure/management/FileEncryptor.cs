using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace IncomeInsightEngine.src.dataStructure.management
{
    /// <summary>
    /// Provides methods for encrypting and decrypting files using AES (Advanced Encryption Standard) and DPAPI (Data Protection API).
    /// </summary>
    /// <remarks>
    /// <para>This class encapsulates the functionality to securely encrypt and decrypt files. It manages the encryption keys and initialization vectors (IVs),
    /// ensuring they are securely generated and stored using the DPAPI with the scope set to the current user. This means that only the user who encrypts the file
    /// can decrypt it, offering a layer of user-specific security.</para>
    /// <para>The class provides a straightforward interface with methods to initialize encryption keys (if they don't exist), encrypt files, decrypt files, and handle
    /// the secure storage and retrieval of keys and IVs. It is intended to be used in scenarios where file data needs to be protected at rest.</para>
    /// </remarks>
    public class FileEncryptor
    {
        private readonly string keyFilePath = "transaction999.bin";
        private readonly string ivFilePath = "transaction998.bin";
        
        /// <summary>
        /// Constructs a new FileEncryptor instance, setting up the file paths for the encryption key and IV, and initializing them.
        /// </summary>
        /// <remarks>
        /// The constructor creates file paths for the encryption key and IV within the user's Application Data folder,
        /// specifically within a subfolder for the IncomeInsightEngine application. It then calls InitializeEncryptionKeys
        /// to generate and store the encryption key and IV if they do not already exist. The filenames "transaction999.bin"
        /// and "transaction998.bin" are used for the key and IV, respectively.
        /// </remarks>
        public FileEncryptor()
        {

            string keyFilePathappDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            keyFilePath = Path.Combine(keyFilePathappDataPath, "IncomeInsightEngine", "transaction999.bin");

            string ivFilePathappDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            ivFilePath = Path.Combine(ivFilePathappDataPath, "IncomeInsightEngine", "transaction998.bin");

            InitializeEncryptionKeys();
        }
        
        /// <summary>
        /// Initializes the encryption keys and IV (Initialization Vector) if they do not already exist.
        /// </summary>
        /// <remarks>
        /// This method checks for the existence of the key and IV files at the specified paths. If either file does not exist,
        /// it creates a new instance of the Aes class, generates a new key and IV, and then encrypts and saves them using
        /// the ProtectAndSave method with DPAPI for the current user. This ensures that the key and IV are securely stored
        /// and are only accessible by the user who created them.
        /// </remarks>
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
        
        /// <summary>
        /// Encrypts the provided data using Data Protection API (DPAPI) and saves it to the specified file.
        /// </summary>
        /// <param name="data">The byte array containing the data to encrypt.</param>
        /// <param name="filePath">The path to the file where the encrypted data should be saved.</param>
        /// <remarks>
        /// This method uses the DPAPI's Protect method to encrypt the data for the current user. The scope of the data protection
        /// is set to the current user, ensuring that only the user who encrypted the data can decrypt it. The encrypted data
        /// is then written to the specified file.
        /// </remarks>
        private void ProtectAndSave(byte[] data, string filePath)
        {           
            byte[] protectedData = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
            File.WriteAllBytes(filePath, protectedData);
        }
        
        /// <summary>
        /// Loads encrypted data from a file and decrypts it using Data Protection API (DPAPI) for the current user.
        /// </summary>
        /// <param name="filePath">The path to the file containing the encrypted data.</param>
        /// <returns>A byte array containing the decrypted data.</returns>
        /// <remarks>
        /// This method reads the encrypted data from the specified file and uses the DPAPI's Unprotect method
        /// to decrypt the data. The scope of the data protection is set to the current user, meaning that only
        /// the user who encrypted the data can decrypt it.
        /// </remarks>
        private byte[] UnprotectAndLoad(string filePath)
        {           
            byte[] protectedData = File.ReadAllBytes(filePath);
            return ProtectedData.Unprotect(protectedData, null, DataProtectionScope.CurrentUser);
        }

        /// <summary>
        /// Encrypts the content of the file specified by the filePath using Advanced Encryption Standard (AES) algorithm.
        /// </summary>
        /// <param name="filePath">The path to the file that needs to be encrypted.</param>
        /// <remarks>
        /// This method reads the content from the file, encrypts it using a stored key and initialization vector (IV),
        /// and then writes the encrypted data back to the file. The AES encryption algorithm is used, and the key and IV
        /// are loaded using custom UnprotectAndLoad methods. It encapsulates the encryption process in a series of using statements
        /// to ensure that all resources are properly disposed of after the encryption is complete.
        /// </remarks>
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
        
        /// <summary>
        /// Decrypts the content of the file specified by the filePath using Advanced Encryption Standard (AES) algorithm.
        /// </summary>
        /// <param name="filePath">The path to the file that needs to be decrypted.</param>
        /// <remarks>
        /// This method performs decryption based on a stored key and initialization vector (IV). It reads the encrypted content from the file,
        /// decrypts it, and then writes the decrypted data back to the same file. It handles cryptographic exceptions specifically and
        /// general exceptions as well. If an exception occurs during decryption, it is caught and logged to the console.
        /// </remarks>
        /// <exception cref="CryptographicException">Thrown when there is an issue with the cryptographic operation, such as an incorrect key or IV.</exception>
        /// <exception cref="Exception">Thrown when an issue occurs during the reading or writing process of the file.</exception>
        public void DecryptFile(string filePath)
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
           
        }
    }
}
