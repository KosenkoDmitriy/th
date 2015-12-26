using System;
using System.IO;

namespace Assets.Scripts.Orig
{
    class Logger
    {
        StreamWriter logWriter;
        StreamReader logReader;
        StreamWriter dataWriter;
        FileHandler file = new FileHandler();

        public void LogResults()
        {
            if (Settings.logging == false)
            {
                return;
            }
            double tp = 999;
            string writestring;
            FileStream fs = new FileStream(Settings.pathToAssetRes + "TexasHoldEm.log", FileMode.OpenOrCreate);
            logWriter = new StreamWriter(fs);
            //logReader = new StreamReader(fs);
            //string file = logReader.ReadToEnd();
            fs.Seek(0, SeekOrigin.End);

            if (Settings.creditsWon > 0)
            {
                tp = 1 / (Settings.creditsPlayed / Settings.creditsWon);
                //   //tp *= -1;
            }

            string CreditsPlayed = String.Format("{0:0.0}", Settings.creditsPlayed);
            string CreditsWon = String.Format("{0:0.0}", Settings.creditsWon);
            string GamePercentage = String.Format("{0:0%}", tp);

            writestring = "#" + Settings.gameNumber.ToString() + " CP= " + CreditsPlayed + " CW= " + CreditsWon + " GP = " + GamePercentage;
            try
            {
                //lblWinInfo.GetComponent<Text>().text += writestring + Environment.NewLine;// TODO: Where this displaying?
                logWriter.WriteLine(writestring);
            }
            catch
            {

            }
            Settings.gameNumber++;

            logWriter.Close();
            fs.Dispose();
            logWriter.Dispose();

            try
            {
                FileStream fds = new FileStream(Settings.pathToAssetRes + "TexasHoldEm.dat", FileMode.OpenOrCreate);
                dataWriter = new StreamWriter(fds);
                fds.Seek(0, SeekOrigin.Begin);
                dataWriter.WriteLine(Settings.gameNumber.ToString() + " " + Settings.creditsPlayed.ToString() + " " + Settings.creditsWon.ToString());

                dataWriter.Close();
                dataWriter.Dispose();
                fds.Dispose();
            }
            catch
            {

            }
        }

        public void GetLogFileVars()
        {
            if (Settings.logging == false)
            {
                return;
            }
            StreamReader dataReader;
            string dataDirectory = Settings.pathToAssetRes + "TexasHoldEm.dat";
            if (File.Exists(dataDirectory) == true)
            {
                FileStream fds = new FileStream(Settings.pathToAssetRes + "TexasHoldEm.dat", FileMode.Open);
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
                    file.EraseFile(Settings.pathToAssetRes + "TexasHoldEm.log");
                    Settings.gameNumber = 1;
                    Settings.creditsPlayed = 0;
                    Settings.creditsWon = 0;
                }
                dataReader.Close();
                dataReader.Dispose();
            }
            else
            {
                file.EraseFile(Settings.pathToAssetRes + "TexasHoldEm.log");
            }
        }
    }
}
