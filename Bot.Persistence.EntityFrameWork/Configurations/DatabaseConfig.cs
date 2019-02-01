using System.IO;
using Bot.Persistence.EntityFrameWork.Structs;
using Newtonsoft.Json;

namespace Bot.Persistence.EntityFrameWork.Configurations
{
    public class DatabaseConfig
    {
        private const string ConfigFolder = "Configs";
        private const string ConfigFile = "DatabaseConfig.json";

        public static DbConfig DbConfig;

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
