using System;
using System.Collections.Generic;
using System.IO;

namespace SODA.Utilities.Tests.Mocks
{
    class FileMocks
    {
        public static string FileThatDoesNotExist(string extension = ".txt")
        {
            return Path.Combine(Path.GetTempPath(), Path.ChangeExtension("not-a-file", extension));
        }
        
        public static string[] ExcelMocks()
        {
            return new[] { ".\\Mocks\\mock.xls", ".\\Mocks\\mock.xlsx" };
        }
    }
}
