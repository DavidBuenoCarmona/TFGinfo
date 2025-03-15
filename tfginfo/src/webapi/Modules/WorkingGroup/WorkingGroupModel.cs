using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TFGinfo.Models
{
    public class WorkingGroupModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int isPrivate { get; set; }
    }
}