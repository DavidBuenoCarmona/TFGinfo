using TFGinfo.Models;

namespace TFGinfo.Objects  
{
    public class CareerBase {
        public int? id { get; set; }
        public string name { get; set; }
        public bool doubleCareer { get; set; }

        public CareerBase() { }

        public CareerBase(CareerModel model)
        {
            name = model.name;
            id = model.id;
            doubleCareer = model.double_career == 1;
        }
    }

    public class CareerDTO : CareerBase {
        public UniversityBase? university { get; set; }
        public List<CareerDTO> doubleCareers { get; set; } = new List<CareerDTO>();

        public CareerDTO() { }

        public CareerDTO(CareerModel model) : base(model)
        {
            if (model.universityModel != null)
            {
                university = new UniversityBase(model.universityModel);
            }
            if (model.DoubleCareer != null)
            {
                doubleCareers.Add(new CareerDTO(model.DoubleCareer.primaryCareerModel));
                doubleCareers.Add(new CareerDTO(model.DoubleCareer.secondaryCareerModel));
            }
        }
    }

    public class CareerFlatDTO : CareerBase {
        public int? universityId { get; set; }
        public List<int> doubleCareers { get; set; } = new List<int>();
        public CareerFlatDTO() { }
        public CareerFlatDTO(CareerModel model) : base(model)
        {
            universityId = model.university ?? 0;
            if (model.DoubleCareer != null)
            {
                doubleCareers.Add(model.DoubleCareer.primary_career);
                doubleCareers.Add(model.DoubleCareer.secondary_career);
            }
        }
    }
}