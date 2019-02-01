using System.IO;
using Bot.Persistence.EntityFrameWork.Structs;
using Newtonsoft.Json;

namespace Bot.Persistence.EntityFrameWork.Configurations
{
    public class DatabaseConfig
    {
        /// <summary>The folder name where the <see cref="ConfigFile"/> is located.</summary>
        private const string ConfigFolder = "Configs";

        /// <summary>The file name of the config file.</summary>
        private const string ConfigFile = "DatabaseConfig.json";

        public static DbConfig DbConfig;

        /// <summary>
        /// Load the Database config file.
        /// </summary>
        static DatabaseConfig()
        {
            if (!Directory.Exists(ConfigFolder))
                Directory.CreateDirectory(ConfigFolder);

            if (!File.Exists(ConfigFolder + "/" + ConfigFile))
            {
                DbConfig = new DbConfig();
                var json = JsonConvert.SerializeObject(DbConfig, Formatting.Indented);
                File.WriteAllText(ConfigFolder + "/" + ConfigFile, json);
            }
            else
            {
                var json = File.ReadAllText(ConfigFolder + "/" + ConfigFile);
                DbConfig = JsonConvert.DeserializeObject<DbConfig>(json);
            }
        }
    }
}
