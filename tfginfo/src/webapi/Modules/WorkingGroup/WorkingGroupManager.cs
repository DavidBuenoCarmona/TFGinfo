using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TFGinfo.Common;
using TFGinfo.Data;
using TFGinfo.Models;
using TFGinfo.Objects;

namespace TFGinfo.Api
{
    public class WorkingGroupManager : BaseManager
    {
        public WorkingGroupManager(ApplicationDbContext context) : base(context) {}

        public List<WorkingGroupBase> GetAllWorkingGroups()
        {
            return context.working_group.ToList().ConvertAll(model => new WorkingGroupBase(model));
        }

        public WorkingGroupBase CreateWorkingGroup(WorkingGroupBase WorkingGroup, List<int>? professors = null, List<int>? students = null, List<int>? TFGs = null)
        { 

            WorkingGroupModel model = new WorkingGroupModel {
                name = WorkingGroup.name,
                description = WorkingGroup.description,
                isPrivate = WorkingGroup.isPrivate ? 1 : 0
            };
            context.working_group.Add(model);
            context.SaveChanges();

            model.Professors = new List<WorkingGroupProfessorModel>();
            if (professors != null) {
                foreach (int professorId in professors) {
                    WorkingGroupProfessorModel workingGroupProfessor = new WorkingGroupProfessorModel {
                        working_group = model.id,
                        professor = professorId
                    };
                    model.Professors.Add(workingGroupProfessor);
                }
            }
            model.Students = new List<WorkingGroupStudentModel>();
            if (students != null) {
                foreach (int studentId in students) {
                    WorkingGroupStudentModel workingGroupStudent = new WorkingGroupStudentModel {
                        working_group = model.id,
                        student = studentId
                    };
                    model.Students.Add(workingGroupStudent);
                }
            }
            model.TFGs = new List<WorkingGroupTFGModel>();
            if (TFGs != null) {
                foreach (int TFGId in TFGs) {
                    WorkingGroupTFGModel workingGroupTFG = new WorkingGroupTFGModel {
                        working_group = model.id,
                        tfg = TFGId
                    };
                    model.TFGs.Add(workingGroupTFG);
                }
            }
            context.Update(model);
            context.SaveChanges();

            return new WorkingGroupBase(model);
        }

        public void DeleteWorkingGroup(int id)
        {
            WorkingGroupModel? model = context.working_group.FirstOrDefault(WorkingGroup => WorkingGroup.id == id);
            if (model == null) {
                throw new NotFoundException();
            }
            context.working_group.Remove(model);
            context.SaveChanges();
        }

        public WorkingGroupBase UpdateWorkingGroup(WorkingGroupBase WorkingGroup)
        {
            WorkingGroupModel? model = context.working_group.FirstOrDefault(d => d.id == WorkingGroup.id);
            if (model == null) {
                throw new NotFoundException();
            }
            model.description = WorkingGroup.description;
            model.name = WorkingGroup.name;
            model.isPrivate = WorkingGroup.isPrivate ? 1 : 0;
            context.SaveChanges();

            return new WorkingGroupBase(model);
        }

        public WorkingGroupBase GetWorkingGroup(int id)
        {
            WorkingGroupModel? model = context.working_group.FirstOrDefault(WorkingGroup => WorkingGroup.id == id);
            if (model == null) {
                throw new NotFoundException();
            }
            return new WorkingGroupBase(model);
        }

        public List<ProfessorDTO> GetProfessorsByWorkingGroup(int id)
        {
            List<ProfessorModel> professors = context.working_group_professor.Where(WorkingGroup => WorkingGroup.working_group == id).Select(p => p.professorModel).ToList();
            return professors.ConvertAll(model => new ProfessorDTO(model));
        }

        public List<StudentDTO> GetStudentsByWorkingGroup(int id)
        {
            List<StudentModel> students = context.working_group_student.Where(WorkingGroup => WorkingGroup.working_group == id).Select(p => p.studentModel).ToList();
            return students.ConvertAll(model => new StudentDTO(model));
        }

        public List<TFGDTO> GetTFGsByWorkingGroup(int id)
        {
            List<TFGModel> TFGs = context.working_group_tfg.Where(WorkingGroup => WorkingGroup.id == id).Select(p => p.tfgModel).ToList();
            return TFGs.ConvertAll(model => new TFGDTO(model));
        }

        public List<WorkingGroupBase> GetWorkingGroupsByProfessor(int id)
        {
            List<WorkingGroupModel> workingGroups = context.working_group_professor.Where(WorkingGroup => WorkingGroup.professor == id).Select(p => p.workingGroupModel).ToList();
            return workingGroups.ConvertAll(model => new WorkingGroupBase(model));
        }

        public List<WorkingGroupBase> GetWorkingGroupsByStudent(int id)
        {
            List<WorkingGroupModel> workingGroups = context.working_group_student.Where(WorkingGroup => WorkingGroup.student == id).Select(p => p.workingGroupModel).ToList();
            return workingGroups.ConvertAll(model => new WorkingGroupBase(model));
        }


        #region Private Methods
        #endregion
    }
}
