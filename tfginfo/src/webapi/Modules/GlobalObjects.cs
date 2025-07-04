using TFGinfo.Models;

namespace TFGinfo.Objects
{
    public class Filter
    {
        public string key { get; set; }
        public string value { get; set; }

    }

    public class CSVImport
    {
        public string content { get; set; }
    }

    public class CSVOutput
    {
        public int success { get; set; }
        public List<string> errorItems { get; set; }

        public CSVOutput()
        {
            success = 0;
            errorItems = new List<string>();
        }
    }
}