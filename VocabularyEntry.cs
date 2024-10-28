using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace passiveEnglishVocab
{
    public class VocabularyEntry
    {
        public required int id { get; set; }
        public required string word { get; set; }
        public required string translation { get; set; }
        public required string description { get; set; }
        public required List<string> synonyms { get; set; }
    }
    public class Configuration
    {
        public required string file_path { get; set; }
        public required int interval { get; set; }
        public required int learning_mode { get; set; }
    }
}
