namespace ReadCSVandCopy
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class ProcessCopy
    {
        private const char commaASeperatorForCSVData = ',';

        public void PerformCopying(List<CopyDetails> pcolCopyDetails)
        {
            foreach (CopyDetails details in pcolCopyDetails)
            {
                if (!File.Exists(details.URL))
                {
                    throw new Exception("Source File doesn't exits" + details.URL);
                }
                if (!Directory.Exists(details.Title))
                {
                    throw new Exception("Target Folder doesn't exits" + details.Title);
                }
                File.Copy(details.URL, Path.Combine(details.Title, Path.GetFileName(details.URL)), true);
            }
        }

        public List<CopyDetails> ReadCSVAndPopulateList(string pstrCSVFilePath)
        {
            List<CopyDetails> list = new List<CopyDetails>();
            foreach (string str in File.ReadAllLines(pstrCSVFilePath, Encoding.UTF8))
            {
                char[] separator = new char[] { ',' };
                List<string> list2 = str.Split(separator).ToList<string>();
                CopyDetails item = new CopyDetails();
                item.Title = list2[0];
                item.URL = list2[1];
                item.Time = list2[2];
                list.Add(item);
                Console.WriteLine(str);
            }
            return list;
        }
    }
}

