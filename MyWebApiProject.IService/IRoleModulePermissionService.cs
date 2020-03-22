using MyWebApiProject.IService.Base;
using MyWebApiProject.Model.DbModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApiProject.IService
{
   public interface IRoleModulePermissionService : IBaseService<RoleModulePermission>
    {
        Task<List<RoleModulePermission>> GetRoleModule();
        Task<List<RoleModulePermission>> RoleModuleMaps();
    }
}
