using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace passiveEnglishVocab
{
    internal class ConfigWrapper
    {
        string config_path = "D:\\Bastian Wiesner\\Own_Programs\\passiveEnglishVocab\\config\\config.json";

        public ConfigWrapper() { }

        public Configuration getConfiguration()
        {
            string filePath = ";";
            if (File.Exists(config_path))
            {
                string jsonString = File.ReadAllText(config_path);
                Configuration configuration = JsonSerializer.Deserialize<Configuration>(jsonString);
                if (configuration is not null)
                {
                    return configuration;
                }
                else
                {
                    throw new Exception("Config couldn't be loaded!");
                }
            }
            else
            {
                throw new Exception("No Config File available at " + config_path);
            }
        }

        public void setConfiguration(Configuration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration), "Configuration cannot be null.");
            }

            string jsonString = JsonSerializer.Serialize(configuration, new JsonSerializerOptions
            {
                WriteIndented = true // Better Formatting
            });

            File.WriteAllText(config_path, jsonString);
        }

    }
}
