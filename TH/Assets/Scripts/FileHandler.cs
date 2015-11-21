using System.IO;

namespace Assets.Scripts
{
    class FileHandler
    {
        public void EraseFile(string iniFile)
        {
            FileInfo fi = new FileInfo(iniFile);
            if (File.Exists(iniFile))
            {
                fi.Delete();
            }
        }
    }
}
