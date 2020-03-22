using MyWebApiProject.IRepository.Base;
using MyWebApiProject.Model.DbModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApiProject.IRepository
{
   public interface IRoleModulePermissionRepository : IBaseRepository<RoleModulePermission>
    {
        Task<List<RoleModulePermission>> WithChildrenModel();
        
        Task<List<RoleModulePermission>> RoleModuleMaps();
    }
}
