using System;
using System.Collections.Generic;
using System.IO;

namespace SODA.Utilities.Tests.Mocks
{
    class FileMocks
    {
        public static string FileThatDoesNotExist(string extension = ".txt")
        {
            return String.Format("{0}{1}", "C:\\temp\\not-a-file", extension);
        }
        
        public static string[] ExcelMocks()
        {
            return new[] { ".\\Mocks\\mock.xls", ".\\Mocks\\mock.xlsx" };
        }
    }
}
