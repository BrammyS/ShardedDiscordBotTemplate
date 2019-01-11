using System.IO;
using Bot.Models;
using Newtonsoft.Json;

namespace Bot.Configurations
{
    public static class ConfigData
    {
        private const string ConfigFolder = "Configs";
        private const string ConfigFile = "ConfigData.json";

        public static ConfigDataModel Data { get; }


        /// <summary>
        /// Loads all the <see cref="ConfigData"/> needed to start the bot.
        /// </summary>
        static ConfigData()
        {
            if (!Directory.Exists(ConfigFolder)) Directory.CreateDirectory(ConfigFolder);

            if (!File.Exists(ConfigFolder + "/" + ConfigFile))
            {
                Data = new ConfigDataModel();
                var json = JsonConvert.SerializeObject(Data, Formatting.Indented);
                File.WriteAllText(ConfigFolder + "/" + ConfigFile, json);
            }
            else
            {
                var json = File.ReadAllText(ConfigFolder + "/" + ConfigFile);
                Data = JsonConvert.DeserializeObject<ConfigDataModel>(json);
            }
        }
    }
}
