using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Yesware
{
    class Program
    {
        public static void ExtractData(string path)
        {
            using (StreamReader reader = File.OpenText(path))
            {
                while (!reader.EndOfStream)
                {
                    var value = reader.ReadLine();
                    var regex = new Regex(@"^[A-Z][a-zA-Z]+(?:, *[A-Z][a-zA-Z]+)");
                    var match = regex.Match(value);
                    var trimmed = Regex.Replace(match.Value, @"\s+", "");
                    File.AppendAllText(@"C:\Users\payam\Desktop\names.txt", trimmed + Environment.NewLine);
                }
            }
        }

        public static void CreateReport(string namesFilePath)
        {
            HashSet<string> uniquefirstNames = new HashSet<string>();
            HashSet<string> uniquelastNames = new HashSet<string>();
            HashSet<string> uniquefullNames = new HashSet<string>();
            List<string> lastNames = new List<string>();
            List<string> firstNames = new List<string>();



            using (StreamReader reader = File.OpenText(namesFilePath))
            {
                while (!reader.EndOfStream)
                {
                    var lineValue = reader.ReadLine();
                    if (!string.IsNullOrEmpty(lineValue))
                    {

                        var names = lineValue.Split(',');

                        uniquefirstNames.Add(names[1]);
                        uniquelastNames.Add(names[0]);

                        uniquefullNames.Add(names[1] + " " + names[0]);

                        lastNames.Add(names[0]);
                        firstNames.Add(names[1]);


                    }

                }

            }


            var commonLastNames =
                lastNames.GroupBy(i => i).OrderByDescending(grep => grep.Count())
                    .Select(grep => new
                    {
                        commonLastName = grep.Key,
                        freq = grep.Count()
                    }).Take(10);



            var commonFirstNames = firstNames.GroupBy(i => i)
                .OrderByDescending(grep => grep.Count()).Select(grep => new
                {
                    commonFirstName = grep.Key,
                    freq = grep.Count()
                }).Take(10);

            Console.WriteLine("Top 10 common last names and their frequency");
            Console.WriteLine();
            foreach (var lName in commonLastNames)
            {
                Console.WriteLine(lName.commonLastName + " " + " " + lName.freq);

            }

            Console.WriteLine("Top 10 common first names and their frequency");
            Console.WriteLine();

            foreach (var fName in commonFirstNames)
            {
                Console.WriteLine(fName.commonFirstName + " " + " " + fName.freq);
            }

            Console.WriteLine("There are {0} uniqe full names.", uniquefullNames.Count);
            Console.WriteLine("There are {0} unique first names.", uniquefirstNames.Count);
            Console.WriteLine("There are {0} unique last names", uniquelastNames.Count);



        }

        static void Main(string[] args)
        {

            // First I clean up the data set and save the clean data in a file on my
            //desktop. Then from that file, I created reports. 
            //var path = @"C:\Users\payam\Desktop\coding-test-data-current.txt";
            //ExtractData(path);

            var path = @"C:\Users\payam\Desktop\names.txt";
            CreateReport(path);


            Console.Read();

        }
    }
}
