using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Text.Unicode;
using System.Text.Encodings.Web;
using System.IO;

namespace ConsoleApp3_4
{
    public class Program
    {
        static void Main(string[] args)
        {
            Run();
        }

        public static void Run()
        {

            List<Product> products = new List<Product>();
            string Path = @"C:\testfolder\Products.json";

            for (int i = 0; i < 5; i++)
            {
                try
                {

                    Console.WriteLine("Введите код товара {0}", i + 1);
                    int code = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Введите название товара {0}", i + 1);
                    string name = Console.ReadLine();
                    Console.WriteLine("Введите стоимость товара {0}", i + 1);
                    double cost = double.Parse(Console.ReadLine().Replace(".", ","));
                    Product product = new Product()
                    {
                        Code = code,
                        Name = name,
                        Cost = cost
                    };
                    products.Add(product);
                }
                catch
                {
                    Console.WriteLine("Не число");
                    i --;
                }
            }

            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(products, options);

            WriteFile Writer = new WriteFile();
            Writer.Write(Path, json);

            ReadString Reader = new ReadString();

            string NewStr = Reader.Read(Path);
            Console.WriteLine("Прочитано из файла {1}: \n {0}", NewStr, Path);

            Product[] newproduct = JsonSerializer.Deserialize<Product[]>(NewStr);
            double maxcost = 0;
            int maxnum = 0;
            for (int i = 0; i<newproduct.Length; i++)
            {
                if (newproduct[i].Cost>maxcost)
                {
                    maxcost = newproduct[i].Cost;
                    maxnum = newproduct[i].Code;
                }
            }
            Console.WriteLine("Самый дорогой товар с кодом {0}, стоимостью {1}", maxnum, maxcost);
            Console.ReadLine();
            Run();
            Environment.Exit(0);
        }
                
        public class Product
        {
            public int Code { get; set; }
            public string Name { get; set; }
            public double Cost { get; set; }
        }

        public class ListProduct
        {
            public List<Product> product { get; set; }
        }

        public class WriteFile
        {
            public string Path { get; set; }
            public string Text { get; set; }

            public bool Write(string Path, string Text)
            {
                this.Path = Path;
                this.Text = Text;
                try
                {
                    using (StreamWriter sw = new StreamWriter(Path, false, System.Text.Encoding.Default))
                    {
                        sw.WriteLine(Text);
                    }
                    Console.WriteLine("Запись выполнена \n");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                return true;
            }
        }


        public class ReadString
        {
            public string Path { get; set; }
            public string Text { get; set; }

            public string Read(string Path)
                {
                try
                {
                    using (StreamReader sr = new StreamReader(Path))
                    {
                       Text = sr.ReadToEnd();
                    }
                }
                
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                return Text;
            }
        }

    }
}



