using TFGinfo.Models;

namespace TFGinfo.Objects  
{
    public class TFGBase {
        public int? id { get; set; }
        public DateTime? startDate { get; set; }
        public string? external_tutor_name { get; set; }
        public string? external_tutor_email { get; set; }
        public bool accepted { get; set; }

        public TFGBase () {}

        public TFGBase (TFGModel model) {
            id = model.id;
            startDate = model.startDate;
            external_tutor_name = model.external_tutor_name;
            external_tutor_email = model.external_tutor_email;
            accepted = model.accepted;
        }
    }

    public class TFGDTO : TFGBase {
        public TFGLineDTO tfgLine  { get; set; }

        public TFGDTO () {}

        public TFGDTO (TFGModel model) : base(model) {
            if (model.tfgLineModel != null) {
                tfgLine = new TFGLineDTO(model.tfgLineModel);
            }
        }
    }

    public class TFGFlatDTO : TFGBase {
        public int tfgLineId { get; set; }
        public TFGFlatDTO () {}
        public TFGFlatDTO (TFGModel model) : base(model) {
            tfgLineId = model.tfg_line;
        }
    }

    public class TFGRequest {
        public string studentEmail { get; set; }
        public int professorId { get; set; }
        public int? secondaryProfessorId { get; set; }
        public TFGFlatDTO tfg { get; set; }
    }

    public class TFGStudentFlatDTO {
        public int tfgId { get; set; }
        public int studentId { get; set; }
    }
}