namespace ReadAndCopy
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class ProcessCopy
    {
        private const char commaASeperatorForCSVData = ',';

        public List<CopyDetails> ReadAndPopulateList(string pstrCSVFilePath)
        {
            List<CopyDetails> list = new List<CopyDetails>();
            foreach (string str in File.ReadAllLines(pstrCSVFilePath, Encoding.UTF8))
            {
                char[] separator = new char[] { ',' };
                List<string> list2 = str.Split(separator).ToList<string>();
                CopyDetails item = new CopyDetails();
                item.UN = list2[0];
                item.PS = list2[1];
                item.Ser = list2[2];
                list.Add(item);
                Console.WriteLine(str);
            }
            return list;
        }
    }
}

