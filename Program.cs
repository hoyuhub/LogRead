using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace TXTCtrl
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Read();
        }

        //需要指定文件路径（包括文件名称）
        public static List<string[]> Read()
        {
            string path = "e://20171010.txt";
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            string line = string.Empty;
            StreamReader sr = new StreamReader(fs, Encoding.Default);
            List<string[]> list = new List<string[]>();
            while ((line = sr.ReadLine()) != null)
            {
                string s = line;
                if (s.Contains("Request"))
                {
                    string[] strArray = s.Split(" ");
                    list.Add(strArray);
                }
            }
            return list;
        }

        public static void Ctrl(List<string[]> list)
        {
                        
        }

    }


}
