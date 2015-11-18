using System.IO;

class Utils
{
    public Utils() { }
    public string GetIniString(string Section, string KeyName, string Default, out int noChars, string FileName)
    {
        StreamReader reader;
        string read;
        string compString;
        reader = File.OpenText(FileName);
        do
        {
            read = reader.ReadLine();
            if (read.StartsWith("[" + Section + "]") == true)
            {
                do
                {
                    read = reader.ReadLine();
                    compString = read.Split('=')[0];
                    compString = compString.TrimEnd(' ');
                    if (compString == KeyName)
                    {
                        compString = read.Split('=')[1];
                        compString = compString.Split(';')[0];//bye bye to comments
                        compString = compString.Trim();
                        reader.Close();
                        noChars = compString.Length;
                        return compString;
                    }

                } while (read.StartsWith("[") == false && reader.EndOfStream == false);
            }

        } while (reader.EndOfStream == false);
        if (Default == null)
        {
            Default = "";
        }
        read = Default; // if we made it here we didnt find the string;
        noChars = read.Length;
        reader.Close();
        return read;
    }

    public int GetIniInt(string Section, string KeyName, int Default, string FileName)
    {
        StreamReader reader;
        string read;
        string compString;
        reader = File.OpenText(FileName);
        do
        {
            read = reader.ReadLine();
            if (read.StartsWith("[" + Section + "]") == true)
            {
                do
                {
                    read = reader.ReadLine();
                    compString = read.Split('=')[0];
                    compString = compString.TrimEnd(' ');
                    if (compString == KeyName)
                    {
                        compString = read.Split('=')[1];
                        compString = compString.Split(';')[0];//bye bye to comments
                        compString = compString.Trim();
                        reader.Close();
                        return int.Parse(compString);
                    }

                } while (read.StartsWith("[") == false && reader.EndOfStream == false);
            }

        } while (reader.EndOfStream == false);
        if (Default == null)
        {
            Default = 0;
        }
        read = Default.ToString(); // if we made it here we didnt find the string;

        reader.Close();
        return int.Parse(read);
    }

    public bool GetIniBool(string Section, string KeyName, bool Default, string FileName)
    {
        StreamReader reader;
        string read;
        string compString;
        reader = File.OpenText(FileName);
        do
        {
            read = reader.ReadLine();
            if (read.StartsWith("[" + Section + "]") == true)
            {
                do
                {
                    read = reader.ReadLine();
                    compString = read.Split('=')[0];
                    compString = compString.TrimEnd(' ');
                    if (compString == KeyName)
                    {
                        compString = read.Split('=')[1];
                        compString = compString.Split(';')[0];//bye bye to comments
                        compString = compString.Trim();
                        if (compString.ToUpper() == "TRUE" || compString == "1")
                        {
                            reader.Close();
                            return true;
                        };
                    }

                } while (read.StartsWith("[") == false && reader.EndOfStream == false);
            }

        } while (reader.EndOfStream == false);
        if (Default == null)
        {
            Default = false;
        }
        //read = Default.ToString(); // if we made it here we didnt find the string;

        reader.Close();
        return Default;
    }

    public int[] GetINIIntArray(string Section, string KeyName, int minSize, string FileName)
    {
        int[] retArray;// = new int[100];
        int[] tempArray = new int[200];
        string[] strArray;
        StreamReader reader;
        string read;
        string compString;
        int ptr = 0;
        reader = File.OpenText(FileName);
        do
        {
            read = reader.ReadLine();
            if (read.StartsWith("[" + Section + "]") == true)
            {
                do
                {
                    read = reader.ReadLine();
                    compString = read.Split('=')[0];
                    compString = compString.TrimEnd(' ');
                    if (compString == KeyName)
                    {
                        compString = read.Split('=')[1];
                        compString = compString.Split(';')[0];//bye bye to comments
                        compString = compString.Trim();
                        strArray = compString.Split(',');//now go get the comma delimited strings
                        foreach (string i in strArray)
                        {
                            if (i.Contains("~"))
                            {
                                int start = int.Parse(i.Remove(i.IndexOf('~')));
                                int len = i.IndexOf('~') + 1;
                                int end = int.Parse(i.Remove(0, len));
                                for (int x = start; x <= end; x++)
                                {
                                    tempArray[ptr++] = x;
                                }
                            }
                            else
                            {
                                tempArray[ptr++] = int.Parse(i);
                            }
                        }
                        reader.Close();
                        if (ptr < minSize)
                        {
                            ptr = minSize;
                        }
                        retArray = new int[ptr];
                        for (int x = 0; x < ptr; x++)
                        {
                            retArray[x] = tempArray[x];
                        }
                        return retArray;
                    }

                } while (read.StartsWith("[") == false && reader.EndOfStream == false);
            }

        } while (reader.EndOfStream == false);


        reader.Close();
        return retArray = new int[minSize];

    }


    public double[] GetINIDoubleArray(string Section, string KeyName, int minSize, string FileName)
    {
        double[] retArray;// = new int[100];
        double[] tempArray = new double[200];
        string[] strArray;
        StreamReader reader;
        string read;
        string compString;
        int ptr = 0;
        reader = File.OpenText(FileName);
        do
        {
            read = reader.ReadLine();
            if (read.StartsWith("[" + Section + "]") == true)
            {
                do
                {
                    read = reader.ReadLine();
                    compString = read.Split('=')[0];
                    compString = compString.TrimEnd(' ');
                    if (compString == KeyName)
                    {
                        compString = read.Split('=')[1];
                        compString = compString.Split(';')[0];//bye bye to comments
                        compString = compString.Trim();
                        strArray = compString.Split(',');//now go get the comma delimited strings
                        foreach (string i in strArray)
                        {
                            if (i.Contains("~"))
                            {
                                int start = int.Parse(i.Remove(i.IndexOf('~')));
                                int len = i.IndexOf('~') + 1;
                                int end = int.Parse(i.Remove(0, len));
                                for (int x = start; x <= end; x++)
                                {
                                    tempArray[ptr++] = x;
                                }
                            }
                            else
                            {
                                tempArray[ptr++] = double.Parse(i);
                            }
                        }
                        reader.Close();
                        if (ptr < minSize)
                        {
                            ptr = minSize;
                        }
                        retArray = new double[ptr];

                        for (int x = 0; x < ptr; x++)
                        {
                            retArray[x] = tempArray[x];
                        }
                        return retArray;
                    }

                } while (read.StartsWith("[") == false && reader.EndOfStream == false);
            }

        } while (reader.EndOfStream == false);


        reader.Close();
        retArray = new double[minSize];
        return retArray;

    }
}
