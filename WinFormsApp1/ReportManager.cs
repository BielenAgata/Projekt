using System;
using System.Collections.Generic;
using System.IO;

namespace WinFormsApp1
{
    public class ReportManager
    {
        public static void SaveReport(List<string> parts, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var part in parts)
                {
                    writer.WriteLine(part);
                }
            }
        }
    }
}
