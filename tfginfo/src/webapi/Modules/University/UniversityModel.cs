namespace TFGinfo.Models
{
    public class UniversityModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public List<UniversityDepartmentModel> Departments { get; set; }
        public List<CareerModel> Careers { get; set; }
    }

}