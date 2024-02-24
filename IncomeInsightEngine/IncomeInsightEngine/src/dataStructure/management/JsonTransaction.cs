using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using dataStructure;
using System.Threading;

namespace IncomeInsightEngine.src.dataStructure.management
{
    public class JsonTransaction
    {

        private string filePath;
        private FileEncryptor encryptor;

        // Constructor without parameters that sets a default file path
        public JsonTransaction()
        {       
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string filePath = Path.Combine(appDataPath, "IncomeInsightEngine", "transactions.json");

            // Stellen Sie sicher, dass das Verzeichnis existiert
            string directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            this.filePath = filePath;

            encryptor = new FileEncryptor();

            CreateFile();
        }

        // Method to create JSON file if it doesn't exist
        public bool CreateFile()
        {
            if (!File.Exists(filePath))
            {
                // Create a new JSON file with an empty transactions array
                File.WriteAllText(filePath, JsonConvert.SerializeObject(new { transactions = new List<Transaction>() }));
                CloseTransactionFile();                           
                return true;
            }           
            return false;
        }

        public bool OpenTransactionFile()
        {            
             encryptor.DecryptFile(filePath);          
            return false;
        }

        public bool CloseTransactionFile()
        {       
            encryptor.EncryptFile(filePath);      
            return false;
        }

        // Method to add a new transaction to the JSON file
        public bool AddTransaction(Transaction transaction)
        {
            OpenTransactionFile();

            // Read the existing file
            var jsonData = File.ReadAllText(filePath);
            var jsonObj = JsonConvert.DeserializeObject<dynamic>(jsonData);

            // Add the new transaction
            var transactionArray = jsonObj.transactions as JArray;
            var newTransaction = JToken.FromObject(transaction);
            transactionArray.Add(newTransaction);

            // Write the updated json back to the file
            File.WriteAllText(filePath, JsonConvert.SerializeObject(jsonObj, Formatting.Indented));

            CloseTransactionFile();
            return true;
        }

        // Method to save data to the JSON file
        public bool SaveData(List<Transaction> transactions)
        {
            OpenTransactionFile();
            var jsonData = JsonConvert.SerializeObject(new { transactions = transactions }, Formatting.Indented);
            File.WriteAllText(filePath, jsonData);
            CloseTransactionFile();
            return true;
        }

        // Method to read all data from the JSON file
        public List<Transaction> ReadData()
        {
            OpenTransactionFile();
            var jsonData = File.ReadAllText(filePath);
            var jsonObj = JsonConvert.DeserializeObject<dynamic>(jsonData);
            var transactions = JsonConvert.DeserializeObject<List<Transaction>>(jsonObj.transactions.ToString());
            CloseTransactionFile();
            return transactions;
        }

    }
}
