﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NiceHashMiner.Utils;

namespace NiceHashMiner
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] argv)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var commandLineArgs = new CommandLineParser(argv);

            bool ConfigExist = Config.ConfigFileExist();
            
            Config.InitializeConfig();

            if (Config.ConfigData.LogToFile)
                Logger.ConfigureWithFile();

            if (Config.ConfigData.DebugConsole)
                Helpers.AllocConsole();

            Helpers.ConsolePrint("NICEHASH", "Starting up NiceHashMiner v" + Application.ProductVersion);

            if (!ConfigExist && !commandLineArgs.IsLang)
            {
                Helpers.ConsolePrint("NICEHASH", "No config file found. Running NiceHash Miner for the first time. Choosing a default language.");
                Application.Run(new Form_ChooseLanguage());
            }

            // Init languages
            International.Initialize(Config.ConfigData.Language);

            if (commandLineArgs.IsLang) {
                Helpers.ConsolePrint("NICEHASH", "Language is overwritten by command line parameter (-lang).");
                International.Initialize(commandLineArgs.LangValue);
                Config.ConfigData.Language = commandLineArgs.LangValue;
            }
            
            Application.Run(new Form_Main(commandLineArgs.IsConfig));
        }

    }
}
