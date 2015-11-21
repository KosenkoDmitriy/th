using System;
using System.IO;

namespace Assets.Scripts
{
    class Logger
    {
        FileHandler file;
        public Logger()
        {
            file = new FileHandler();
        }

        public void GetLogFileVars()
        {
            if (Settings.logging == false)
            {
                return;
            }
            StreamReader dataReader;
            string dataDirectory = Directory.GetCurrentDirectory() + "\\TexasHoldEm.dat";
            if (File.Exists(dataDirectory) == true)
            {
                FileStream fds = new FileStream(Directory.GetCurrentDirectory() + "\\TexasHoldEm.dat", FileMode.Open);
                dataReader = new StreamReader(fds);
                string read = dataReader.ReadToEnd();
                string[] vars = read.Split(' ');
                try
                {
                    Settings.gameNumber = int.Parse(vars[0]);
                    Settings.creditsPlayed = double.Parse(vars[1]);
                    Settings.creditsWon = double.Parse(vars[2]);
                }
                catch
                {
                    file.EraseFile(Directory.GetCurrentDirectory() + "\\TexasHoldEm.log");
                    Settings.gameNumber = 1;
                    Settings.creditsPlayed = 0;
                    Settings.creditsWon = 0;
                }
                dataReader.Close();
                dataReader.Dispose();
            }
            else
            {
                file.EraseFile(Directory.GetCurrentDirectory() + "\\TexasHoldEm.log");
            }
        }
    }
}
