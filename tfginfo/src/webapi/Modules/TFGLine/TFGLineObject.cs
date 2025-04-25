using TFGinfo.Models;

namespace TFGinfo.Objects  
{
    public class TFGLineBase {
        public int? id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int slots { get; set; }
        public bool group { get; set; }

        public TFGLineBase () {}

        public TFGLineBase (TFGLineModel model) {
            name = model.name;
            id = model.id;
            description = model.description;
            slots = model.slots;
            group = model.group == 1;
        }
    }

    public class TFGLineDTO : TFGLineBase {
        public DepartmentDTO department { get; set; }
        public CareerDTO[] careers { get; set; }
        public TFGDTO[] tfgs { get; set; }
        public ProfessorDTO[] professors { get; set; }

        public TFGLineDTO () {}

        public TFGLineDTO (TFGLineModel model) : base(model) {
            if (model.departmentModel != null) {
                department = new DepartmentDTO(model.departmentModel);
            }
            if (model.Careers != null) {
                careers = model.Careers.ConvertAll(c => new CareerDTO(c.careerModel)).ToArray();
            }
            if (model.Professors != null) {
                professors = model.Professors.ConvertAll(c => new ProfessorDTO(c.professorModel)).ToArray();
            }
        }
    }

    public class TFGLineFlatDTO : TFGLineBase {
        public int departmentId { get; set; }
        public TFGLineFlatDTO () {}
        public TFGLineFlatDTO (TFGLineModel model) : base(model) {
            departmentId = model.department;
        }
    }
}