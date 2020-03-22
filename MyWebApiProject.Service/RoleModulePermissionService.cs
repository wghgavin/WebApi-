using MyWebApiProject.IRepository;
using MyWebApiProject.IService;
using MyWebApiProject.Model.DbModel;
using MyWebApiProject.Service.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApiProject.Service
{
   public class RoleModulePermissionService : BaseService<RoleModulePermission>, IRoleModulePermissionService
    {
        readonly IRoleModulePermissionRepository _dal;
        readonly IModuleRepository _moduleRepository;
        readonly IRoleRepository _roleRepository;
        //多个仓储注入
        public RoleModulePermissionService(IRoleModulePermissionRepository dal, IModuleRepository moduleRepository, IRoleRepository roleRepository)
        {
            this._dal = dal;
            this._moduleRepository = moduleRepository;
            this._roleRepository = roleRepository;
            this.baseDal = dal;
        }
        public async Task<List<RoleModulePermission>> GetRoleModule()
        {
            var roleModulePermissions = await _dal.Query(a => a.IsDeleted == false);
            if (roleModulePermissions.Count > 0)
            {
                foreach(var item in roleModulePermissions)
                {
                    item.Role = await _roleRepository.QueryByID(item.RoleId);
                    item.Module = await _moduleRepository.QueryByID(item.ModuleId);
                }
            }
            return roleModulePermissions;
        }
        public async Task<List<RoleModulePermission>> RoleModuleMaps()
        {
            return await _dal.RoleModuleMaps();
        }
    }
}
