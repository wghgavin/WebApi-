using MyWebApiProject.IRepository;
using MyWebApiProject.IService;
using MyWebApiProject.Model.DbModel;
using MyWebApiProject.Service.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApiProject.Service
{
    public class RoleService : BaseService<Role>, IRoleService
    {
        IRoleRepository _dal;
        public RoleService(IRoleRepository dal)
        {
            this._dal = dal;
            this.baseDal = dal;
        }
    }
}
