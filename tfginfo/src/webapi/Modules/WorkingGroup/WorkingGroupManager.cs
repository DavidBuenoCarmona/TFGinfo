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

        public WorkingGroupBase CreateWorkingGroup(WorkingGroupBase WorkingGroup)
        { 

            WorkingGroupModel model = new WorkingGroupModel {
                name = WorkingGroup.name,
                description = WorkingGroup.description,
                isPrivate = WorkingGroup.isPrivate ? 1 : 0
            };
            context.working_group.Add(model);
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


        #region Private Methods
        #endregion
    }
}
