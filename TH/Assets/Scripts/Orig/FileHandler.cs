namespace Assets.Scripts
{
    class FileHandler
    {
        public void EraseFile(string iniFile)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(iniFile);
            if (System.IO.File.Exists(iniFile))
            {
                //fi.Delete(); // TODO: Assets/Scripts/FileHandler.cs(13,20): error CS1061: Type `System.IO.FileInfo' does not contain a definition for `Delete' and no extension method `Delete' of type `System.IO.FileInfo' could be found (are you missing a using directive or an assembly reference?)
            }
        }
    }
}
