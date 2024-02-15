using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using dataStructure;

namespace IncomeInsightEngine.src.dataStructure.management
{
    public class JsonTransaction
    {
        private string _filePath;

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
            _filePath = filePath;
        }

        // Method to create JSON file if it doesn't exist
        public bool CreateFile()
        {
            if (!File.Exists(_filePath))
            {
                // Create a new JSON file with an empty transactions array
                File.WriteAllText(_filePath, JsonConvert.SerializeObject(new { transactions = new List<Transaction>() }));
                Console.WriteLine("Datei erstellt: " + _filePath);
                return true;
            }
            Console.WriteLine("Datei existiert schon: " + _filePath);
            return false;
        }

        // Method to add a new transaction to the JSON file
        public bool AddTransaction(Transaction transaction)
        {
            // Read the existing file
            var jsonData = File.ReadAllText(_filePath);
            var jsonObj = JsonConvert.DeserializeObject<dynamic>(jsonData);

            // Add the new transaction
            var transactionArray = jsonObj.transactions as JArray;
            var newTransaction = JToken.FromObject(transaction);
            transactionArray.Add(newTransaction);

            // Write the updated json back to the file
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(jsonObj, Formatting.Indented));
            return true;
        }

        // Method to save data to the JSON file
        public bool SaveData(List<Transaction> transactions)
        {
            var jsonData = JsonConvert.SerializeObject(new { transactions = transactions }, Formatting.Indented);
            File.WriteAllText(_filePath, jsonData);
            return true;
        }

        // Method to read all data from the JSON file
        public List<Transaction> ReadData()
        {
            var jsonData = File.ReadAllText(_filePath);
            var jsonObj = JsonConvert.DeserializeObject<dynamic>(jsonData);
            var transactions = JsonConvert.DeserializeObject<List<Transaction>>(jsonObj.transactions.ToString());
            return transactions;
        }

        // Method to read a single transaction from a JsonElement
        public Transaction ReadSingleTransaction(JToken jsonElement)
        {
            return jsonElement.ToObject<Transaction>();
        }
    }
}
