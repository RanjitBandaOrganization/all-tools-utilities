using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace YT_ATP.Test.Steps
{
    public static class SetUp
    {
        public static string projectPath = (Directory.GetParent(Directory.GetCurrentDirectory()).FullName + "\\").Replace("\\bin\\Debug\\","");
        public static string driverPath = projectPath + "\\T.C\\Webdrivers\\";
    }
}
