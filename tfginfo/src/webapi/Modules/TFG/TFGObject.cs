using TFGinfo.Models;

namespace TFGinfo.Objects
{
    public class TFGBase
    {
        public int? id { get; set; }
        public DateTime? startDate { get; set; }
        public string? external_tutor_name { get; set; }
        public string? external_tutor_email { get; set; }
        public int status { get; set; }

        public TFGBase() { }

        public TFGBase(TFGModel model)
        {
            id = model.id;
            startDate = model.startDate;
            external_tutor_name = model.external_tutor_name;
            external_tutor_email = model.external_tutor_email;
            status = model.status;
        }
    }

    public class TFGDTO : TFGBase
    {
        public TFGLineDTO tfgLine { get; set; }
        public List<TFGStudentFlatDTO> students { get; set; }
        public List<TFGProfessorFlatDTO> professors { get; set; }

        public TFGDTO() { }

        public TFGDTO(TFGModel model) : base(model)
        {
            if (model.tfgLineModel != null)
            {
                tfgLine = new TFGLineDTO(model.tfgLineModel);
            }
            if (model.Students != null)
            {
                students = model.Students.ConvertAll(student => new TFGStudentFlatDTO
                {
                    tfgId = model.id,
                    studentId = student.student,
                });
            }
            if (model.Professors != null)
            {
                professors = model.Professors.ConvertAll(professor => new TFGProfessorFlatDTO
                {
                    tfgId = model.id,
                    professorId = professor.professor
                });
            }
        }
    }

    public class TFGFlatDTO : TFGBase
    {
        public int tfgLineId { get; set; }
        public TFGFlatDTO() { }
        public TFGFlatDTO(TFGModel model) : base(model)
        {
            tfgLineId = model.tfg_line;
        }
    }

    public class TFGRequest
    {
        public string studentEmail { get; set; }
        public int professorId { get; set; }
        public int? secondaryProfessorId { get; set; }
        public TFGFlatDTO tfg { get; set; }
    }

    public class TFGStudentFlatDTO
    {
        public int tfgId { get; set; }
        public int studentId { get; set; }
    }

    public class TFGProfessorFlatDTO
    {
        public int tfgId { get; set; }
        public int professorId { get; set; }
    }

    public class TFGPendingRequestDTO
    {
        public string studentName { get; set; }
        public string tfgName { get; set; }
        public int tfgId { get; set; }
        public int tfgStatus { get; set; }
    }
    
    public enum TFGStatus
    {
        Pending = 0,
        Aproved = 1,
        PreliminaryAproved = 2,
        Finished = 3
    }
}