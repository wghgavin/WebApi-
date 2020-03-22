using MyWebApiProject.IRepository;
using MyWebApiProject.IService;
using MyWebApiProject.Model.DbModel;
using MyWebApiProject.Service.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApiProject.Service
{
    public class ModuleService : BaseService<Module>, IModuleServices
    {

        IModuleRepository _dal;
        public ModuleService(IModuleRepository dal)
        {
            this._dal = dal;
            base.baseDal = dal;
        }

    }
}
