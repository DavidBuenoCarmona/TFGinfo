using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TFGinfo.Models
{
    public class ExternalTutorModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
    }

}