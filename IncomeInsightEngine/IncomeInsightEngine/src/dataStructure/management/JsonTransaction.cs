using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using dataStructure;
using System.Threading;
using System.Diagnostics;
using IncomeInsightEngine.Properties;

namespace IncomeInsightEngine.src.dataStructure.management
{
    /// <summary>
    /// Manages the encryption, decryption, storage, and retrieval of transaction data in a JSON format.
    /// </summary>
    /// <remarks>
    /// <para>The JsonTransaction class encapsulates functionality for managing a collection of transactions, including adding new transactions,
    /// saving collections of transactions, and reading transaction data from a storage file. It leverages the FileEncryptor class to ensure
    /// that transaction data is stored securely through encryption.</para>
    /// <para>The class is designed to work within the IncomeInsightEngine's data structure management, specifically focusing on the handling of
    /// transaction data in a JSON file format. It automates the process of file management by ensuring that necessary directories and files
    /// are created upon initialization and that data integrity is maintained through the use of the encryption and decryption processes.</para>
    /// <para>All operations that modify the transaction file, including adding and saving transactions, automatically handle encryption and decryption
    /// to ensure that data is only stored in an encrypted form, thus enhancing security.</para>
    /// </remarks>
    public class JsonTransaction
    {

        private string filePath;
        private FileEncryptor encryptor;

        /// <summary>
        /// Constructs a new instance of the JsonTransaction class, setting up the file path and ensuring the file and directory exist.
        /// </summary>
        /// <remarks>
        /// This constructor initializes the file path for storing transactions within the user's Application Data folder, specifically within a
        /// subdirectory for the IncomeInsightEngine application. It checks if the directory exists, and if not, creates it. Then, it initializes the
        /// filePath field with the path to the transactions.json file. An instance of the FileEncryptor class is created to manage file encryption
        /// and decryption. Finally, it calls the CreateFile method to ensure that the transactions file exists and is initialized correctly.
        /// </remarks>
        public JsonTransaction()
        {       
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string filePath = Path.Combine(appDataPath, "IncomeInsightEngine", "transactions.json");
          
            string directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            this.filePath = filePath;

            encryptor = new FileEncryptor();

            CreateFile();
        }

        /// <summary>
        /// Creates a new transaction file if it does not already exist, initializing it with an empty transactions array.
        /// </summary>
        /// <returns>Boolean value indicating the success of the operation. Returns true if the file was successfully created, otherwise false.</returns>
        /// <remarks>
        /// This method checks for the existence of the file at the specified filePath. If the file does not exist, it creates a new JSON file and initializes it
        /// with an empty transactions array. After creating and initializing the file, it encrypts the file by calling CloseTransactionFile. The method returns true
        /// if the file is created and false if the file already exists, indicating no action was taken.
        /// </remarks>
        /// <exception cref="IOException">Thrown when an I/O error occurs during file creation or writing.</exception>
        public bool CreateFile()
        {
            if (!File.Exists(filePath))
            {              
                File.WriteAllText(filePath, JsonConvert.SerializeObject(new { transactions = new List<Transaction>() }));
                CloseTransactionFile();                           
                return true;
            }           
            return false;
        }

        /// <summary>
        /// Opens the transaction file by decrypting its contents.
        /// </summary>
        /// <returns>Boolean value indicating the success of the operation. Returns true to signify the file was successfully decrypted.</returns>
        /// <remarks>
        /// This method calls the DecryptFile method on an instance of the FileEncryptor class to decrypt the transaction file's content before any operations are performed on it.
        /// The method's implementation indicates a success by returning true.
        /// </remarks>
        /// <exception cref="CryptographicException">Thrown if an error occurs during the file decryption process.</exception>
        /// <exception cref="IOException">Thrown when an I/O error occurs during file access.</exception>
        public bool OpenTransactionFile()
        {            
            encryptor.DecryptFile(filePath);          
            return true;
        }
        
        /// <summary>
        /// Closes the transaction file and encrypts its content.
        /// </summary>
        /// <returns>Boolean value indicating the success of the operation. Returns true to signify the file was successfully encrypted and closed.</returns>
        /// <remarks>
        /// This method encrypts the transaction file's content using the EncryptFile method of an instance of the FileEncryptor class. The return value
        /// of true indicates that the file has been successfully encrypted.
        /// </remarks>
        /// <exception cref="CryptographicException">Thrown if an error occurs during the file encryption process.</exception>
        /// <exception cref="IOException">Thrown when an I/O error occurs during file access.</exception>
        public bool CloseTransactionFile()
        {       
            encryptor.EncryptFile(filePath);      
            return true;
        }

        /// <summary>
        /// Adds a new Transaction object to the existing collection of transactions in the JSON file.
        /// </summary>
        /// <param name="transaction">The Transaction object to be added to the file.</param>
        /// <returns>Boolean value indicating success of the operation. Returns true if the transaction is successfully added.</returns>
        /// <remarks>
        /// This method opens the transaction file, reads the existing JSON data, and deserializes it into a dynamic object to access the transactions node.
        /// It then converts the transactions node to a JArray, adds the new Transaction object to this array, serializes the updated object back into JSON,
        /// writes this JSON string to the file, and closes the file. The method assumes that OpenTransactionFile and CloseTransactionFile manage the file
        /// resource securely and efficiently. It provides a way to dynamically add transactions to the data file without loading all transactions into memory,
        /// which can be beneficial for performance with large data sets.
        /// </remarks>
        /// <exception cref="JsonException">Thrown if an error occurs during serialization or deserialization.</exception>
        /// <exception cref="IOException">Thrown when an I/O error occurs while opening, reading, writing to, or closing the file.</exception>
        public bool AddTransaction(Transaction transaction)
        {
            OpenTransactionFile();
          
            var jsonData = File.ReadAllText(filePath);
            var jsonObj = JsonConvert.DeserializeObject<dynamic>(jsonData);
           
            var transactionArray = jsonObj.transactions as JArray;
            var newTransaction = JToken.FromObject(transaction);
            transactionArray.Add(newTransaction);
          
            File.WriteAllText(filePath, JsonConvert.SerializeObject(jsonObj, Formatting.Indented));

            CloseTransactionFile();
            return true;
        }

        /// <summary>
        /// Serializes a list of Transaction objects into JSON format and saves it to a file.
        /// </summary>
        /// <param name="transactions">The list of Transaction objects to be serialized and saved.</param>
        /// <returns>Boolean value indicating success of the operation. Returns true if data is successfully saved.</returns>
        /// <remarks>
        /// This method opens the transaction file for writing, serializes the provided list of Transaction objects into a JSON string with
        /// indented formatting for readability, writes this JSON string to the file, and then closes the file. The method encapsulates the transactions
        /// in an anonymous object with a 'transactions' property to structure the JSON data. It assumes that OpenTransactionFile and CloseTransactionFile
        /// manage the file resource securely and efficiently.
        /// </remarks>
        /// <exception cref="JsonException">Thrown if an error occurs during serialization.</exception>
        /// <exception cref="IOException">Thrown when an I/O error occurs while opening, writing to, or closing the file.</exception>
        public bool SaveData(List<Transaction> transactions)
        {
            OpenTransactionFile();
            var jsonData = JsonConvert.SerializeObject(new { transactions = transactions }, Formatting.Indented);
            File.WriteAllText(filePath, jsonData);
            CloseTransactionFile();
            return true;
        }

        /// <summary>
        /// Reads transaction data from a JSON file and deserializes it into a list of Transaction objects.
        /// </summary>
        /// <returns>A list of Transaction objects.</returns>
        /// <remarks>
        /// This method opens the transaction file, reads the JSON data, and deserializes it into a dynamic object first to access the transactions node.
        /// It then deserializes the content of the transactions node into a list of Transaction objects. After deserialization, it closes the transaction file.
        /// It is assumed that OpenTransactionFile and CloseTransactionFile manage the opening and closing of the file resource securely.
        /// </remarks>
        /// <exception cref="JsonException">Thrown when the JSON data cannot be deserialized into the expected format.</exception>
        /// <exception cref="IOException">Thrown when an I/O error occurs while opening, reading, or closing the file.</exception>
        public List<Transaction> ReadData()
        {
            OpenTransactionFile();
            var jsonData = File.ReadAllText(filePath);
            var jsonObj = JsonConvert.DeserializeObject<dynamic>(jsonData);
            var transactions = JsonConvert.DeserializeObject<List<Transaction>>(jsonObj.transactions.ToString());
            CloseTransactionFile();
            return transactions;
        }

        public void OpenFileWithDefaultProgram()
        {
            try
            {
                if (File.Exists(filePath))
                {
                    var startInfo = new ProcessStartInfo(filePath)
                    {
                        UseShellExecute = true
                    };

                    Process.Start(startInfo);
                }
                else
                {
                    Console.WriteLine(Strings.Thefiledoesnotexist);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(Strings.Error + ex.Message);
            }
        }
    }
}
