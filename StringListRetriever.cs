using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2020
{
    public static class StringListRetriever
    {
        public static IEnumerable<string> Retreive(string fileName)
        {
            string path = $"..\\..\\..\\{fileName}";
            return File.ReadLines(path);
        }
    }
}
