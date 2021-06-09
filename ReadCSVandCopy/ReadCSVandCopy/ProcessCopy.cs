using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace ReadCSVandCopy
{
    public class ProcessCopy
    {
        public void PerformCopying(List<CopyDetails> pcolCopyDetails)
        {
            foreach (var x in pcolCopyDetails)
            {
                if (File.Exists(x.SourceFile))
                {
                    if (Directory.Exists(x.TargetFolder))
                    {
                        File.Copy(x.SourceFile, Path.Combine(x.TargetFolder, Path.GetFileName(x.SourceFile)), true);
                    }
                    else
                    {
                        throw new Exception(string.Concat("Target Folder doesn't exits", x.TargetFolder));
                    }
                }
                else
                {
                    throw new Exception(string.Concat("Source File doesn't exits", x.SourceFile));

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
                lcolSplittedStrings = line.Split(",").ToList();
                copyDetails.Add(new CopyDetails
                {
                    SourceFile = lcolSplittedStrings[0],
                    TargetFolder = lcolSplittedStrings[1]
                });
                Console.WriteLine(line);
            }

            return copyDetails;
        }
    }
}
