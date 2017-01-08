﻿using System.IO;
using System.Xml.Linq;

namespace iTool.DiscordBot.Prefrences
{
    internal static class SettingsFolders
    {
        internal static void Create()
        {


            if (!File.Exists(Settings.Static.SettingsFile))
            {
                XDocument XDoc = new XDocument(
                    new XDeclaration("1.0", "UTF-8", "yes"),
                        new XElement("iTool",
                            new XElement("General",
                                new XElement("Game"),
                                new XElement("AntiSwear", "True")
                            ),
                            new XElement("ApiKeys",
                                new XElement("SteamKey"),
                                new XElement("OpenWeatherMapKey"),
                                new XElement("DiscordToken")
                            )
                        )
                    );
                XDoc.Save(File.OpenWrite(Settings.Static.SettingsFile));
            }
        }
    }
}
