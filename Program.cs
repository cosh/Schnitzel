using System;
using System.IO;

namespace Schnitzel
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var pathToCSV = @"G:\telecom-graph\telecom-graphCopy\edge_user_use_app\edge_user_use_app.txt";
            var path = Path.GetDirectoryName(pathToCSV);
            var fileToExtract = Path.GetFileName(pathToCSV);
            var fileExtension = Path.GetExtension(fileToExtract);
            var extractedAndPartitionedData = "extracted";
            var extractHeader = false;

            Directory.CreateDirectory(Path.Combine(path, extractedAndPartitionedData));

            int numberOfLines = 1500000;

            using (StreamReader sr = File.OpenText(pathToCSV))
            {
                String header = String.Empty;

                if(extractHeader)
                {
                    header = sr.ReadLine();
                }

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
                            if (extractHeader)
                            {
                                writer.WriteLine(header);
                            }
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
