﻿using MyWebApiProject.IRepository;
using MyWebApiProject.IRepository.UnitOfWork;
using MyWebApiProject.Model.DbModel;
using MyWebApiProject.Repository.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApiProject.Repository
{
    public class RoleModulePermissionRepository : BaseRepository<RoleModulePermission>, IRoleModulePermissionRepository
    {
        public RoleModulePermissionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
  
        public async Task<List<RoleModulePermission>> RoleModuleMaps()
        {
            return await QueryMuch<RoleModulePermission, Module, Role, RoleModulePermission>(
                (rmp,m,r)=>new object[] { 
                      JoinType.Left,rmp.ModuleId==m.Id,
                      JoinType.Left,rmp.RoleId==r.Id
                },
                (rmp,m,r)=>new RoleModulePermission()
                {
                    Role=r,
                    Module =m,
                    IsDeleted=rmp.IsDeleted
                },
                (rmp,m,r)=>rmp.IsDeleted==false&&m.IsDeleted==false&&r.IsDeleted==false
                );
        }
        /// <summary>
        /// 角色权限Map
        /// RoleModulePermission, Module, Role 三表联合
        /// 第四个类型 RoleModulePermission 是返回值
        /// </summary>
        /// <returns></returns>
        public async Task<List<RoleModulePermission>> WithChildrenModel()
        {
            var list = await Task.Run(() => DB.Queryable<RoleModulePermission>()
              .Mapper(it => it.Role, it => it.RoleId)
              .Mapper(it => it.Permission, it => it.PermissionId)
              .Mapper(it => it.Module, it => it.ModuleId).ToList());
            return list;
        }
    }
}
