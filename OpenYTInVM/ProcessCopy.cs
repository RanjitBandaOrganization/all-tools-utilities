using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace ReadCSVandCopy
{
    public class ProcessCopy
    {
        private const char commaASeperatorForCSVData = ',';

        public void PerformCopying(List<CopyDetails> pcolCopyDetails)
        {
            foreach (var x in pcolCopyDetails)
            {
                if (File.Exists(x.URL))
                {
                    if (Directory.Exists(x.Title))
                    {
                        File.Copy(x.URL, Path.Combine(x.Title, Path.GetFileName(x.URL)), true);
                    }
                    else
                    {
                        throw new Exception(string.Concat("Target Folder doesn't exits", x.Title));
                    }
                }
                else
                {
                    throw new Exception(string.Concat("Source File doesn't exits", x.URL));

                }
            }
        }


        public List<CopyDetails> ReadCSVAndPopulateList(string pstrCSVFilePath)
        {
            List<CopyDetails> copyDetails = new List<CopyDetails>();
            string[] lines = File.ReadAllLines(pstrCSVFilePath, Encoding.UTF8);
            List<string> lcolSplittedStrings;
            foreach (string line in lines)
            {
                lcolSplittedStrings = line.Split(commaASeperatorForCSVData).ToList();
                copyDetails.Add(new CopyDetails
                {
                    Title = lcolSplittedStrings[0],
                    URL = lcolSplittedStrings[1],
                    Time = lcolSplittedStrings[2]
                });
                Console.WriteLine(line);
            }

            return copyDetails;
        }
    }
}
