using TFGinfo.Models;

namespace TFGinfo.Objects  
{
    public class TFGBase {
        public int? id { get; set; }
        public DateTime startDate { get; set; }

        public TFGBase () {}

        public TFGBase (TFGModel model) {
            id = model.id;
            startDate = model.startDate;
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
            tfgLineId = model.tfgLine;
        }
    }

    public class TFGStudentFlatDTO {
        public int tfgId { get; set; }
        public int studentId { get; set; }
    }
}