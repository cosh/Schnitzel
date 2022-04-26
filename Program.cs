using System;
using System.IO;

namespace Schnitzel
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var pathToCSV = @"C:\Users\cosh\Downloads\pathToTheSchnitzel";
            var path = Path.GetDirectoryName(pathToCSV);
            var fileToExtract = Path.GetFileName(pathToCSV);
            var fileExtension = Path.GetExtension(fileToExtract);
            var extractedAndPartitionedData = "extracted";

            Directory.CreateDirectory(Path.Combine(path, extractedAndPartitionedData));

            int numberOfLines = 15000000;

            using (StreamReader sr = File.OpenText(pathToCSV))
            {
                var header = sr.ReadLine();

                StreamWriter writer = null;

                int x = 0;
                int batch = 0;
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    if (line != null)
                    {
                        if (x % numberOfLines == 0)
                        {
                            if(writer != null)
                            {
                                //flush it
                                writer.Flush();
                                writer.Close();
                            }

                            Console.WriteLine($"new batch {batch}");

                            //new file
                            var batchFilename = Path.Combine(path, extractedAndPartitionedData, 
                                Path.GetFileNameWithoutExtension(fileToExtract) + "_" + batch.ToString() + fileExtension);
                            writer = new StreamWriter(batchFilename, true);
                            writer.WriteLine(header);
                            batch++;
                        }

                        writer.WriteLine(line);
                    }
                    x += 1;
                }

                writer.Flush();
                writer.Close();
            } //Finished. Close the file


            Console.WriteLine("Done.");

            Console.ReadLine();
        }
    }
}
