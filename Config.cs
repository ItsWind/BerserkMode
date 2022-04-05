using System;
using System.Collections.Generic;
using System.IO;

namespace BerserkMode
{
    internal class Config
    {
        private readonly string configFileString =
            "Set to 1 to enable Berserk mode always being activated for the player.\n" +
            "alwaysBerserk=0\n\n" +

            "Set to 0 to disable support for companions going berserk.\n" +
            "minHPPerKill=5\n\n" +

            "MINIMUM HP players receives on kill during Berserk Mode\n" +
            "minHPPerKill=5\n\n" +

            "MAXIMUM HP players receives on kill during Berserk Mode\n" +
            "maxHPPerKill=20\n\n" +

            "The amount of seconds the player must wait after exiting Berserk Mode before they can start it again\n" +
            "cooldownInSeconds=120\n\n" +

            "The amount of seconds the player will be able to use Berserk Mode\n" +
            "berserkTimeInSeconds=30\n\n" +

            "Set to 1 to report in text how much HP you gained every time an enemy has been killed\n" +
            "reportPlayerHPGain=0\n\n" +

            "Set to 1 to enable HP on kill by default. This sets it on for *ALL* troops.\n" +
            "hpOnKillByDefault=0\n\n" +

            "MINIMUM HP *ALL* troops receive on kill if hpOnKillByDefault is set to 1.\n" +
            "minHPPerKillByDefault=0\n\n" +

            "MAXIMUM HP *ALL* troops receive on kill if hpOnKillByDefault is set to 1.\n" +
            "maxHPPerKillByDefault=0\n\n" +

            "Set to 1 to break through blocks while in Berserk Mode.\n" +
            "breakThroughBlocks=1\n\n" +

            "Set a damage multiplier while in Berserk Mode. This number is divided by 10.\n" +
            "*For example*: to set the damage to itself multiplied by 1.5, make this number 15. to set the damage multiplied by 2, make this number 20.\n" +
            "(this is done this way to keep it in line with being an integer inside of the code itself. why? because im lazy and dont feel like writing a lot of code to make it not like this)\n" +
            "berserkDamageMultiplier=15\n\n" +

            "Set a damage REDUCTION multiplier while in Berserk Mode. This number is divided by 10.\n" +
            "*For example*: to set the damage TAKEN to itself divided by 1.5, make this number 15. to set the damage taken divided by 2, make this number 20.\n" +
            "berserkDamageDivider=15\n\n";
        private Dictionary<string, int> configValues = new();

        private void CreateConfigFile(string filePath)
        {
            StreamWriter sw = new(filePath);
            sw.WriteLine(this.configFileString);
            sw.Close();
        }

        public Config(string modPath)
        {
            string configFilePath = modPath + "\\config.txt";

            if (!File.Exists(configFilePath))
            {
                CreateConfigFile(configFilePath);
            }

            StreamReader sr = new(configFilePath);
            string line;
            // Read and display lines from the file until the end of
            // the file is reached.
            while ((line = sr.ReadLine()) != null)
            {
                int indexOfEqualSign = line.IndexOf('=');
                if (indexOfEqualSign != -1)
                {
                    string key = line.Substring(0, indexOfEqualSign);
                    string value = line.Substring(indexOfEqualSign + 1);
                    configValues.Add(key, Convert.ToInt32(value));
                }
            }
            sr.Close();
        }
        public Dictionary<string, int> GetConfigValues()
        {
            return configValues;
        }
    }
}
