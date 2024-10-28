using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using Windows.AI.MachineLearning;
using System.Windows;

namespace passiveEnglishVocab
{
    public class VocabularyParser
    {
        private List<VocabularyEntry> vocabEntries;

        public VocabularyParser()
        {
            string filePath = "";
            Configuration configuration = new ConfigWrapper().getConfiguration();
            LoadVocabulary(configuration.file_path);

        }

        private void LoadVocabulary(string filePath)
        {
            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                vocabEntries = JsonSerializer.Deserialize<List<VocabularyEntry>>(jsonString);
            }
            else
            {
                Console.WriteLine("File not found: " + filePath);
                vocabEntries = new List<VocabularyEntry>();
            }
        }

        public VocabularyEntry GetVocabularyById(int id)
        {
            return vocabEntries?.Find(entry => entry.id == id);
        }
    }
}
