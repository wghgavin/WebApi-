using MyWebApiProject.IRepository;
using MyWebApiProject.IService;
using MyWebApiProject.Model.DbModel;
using MyWebApiProject.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApiProject.Service
{
    public class LoginService : BaseService<LoginEntity>, ILoginService
    {
        ILoginRepository _dal;
        public LoginService(ILoginRepository dal)
        {
            this._dal = dal;

        }

        public async Task<bool> QueryByUserNameAndPwd(string userName, string passWord)
        {
            LoginEntity result = (await _dal.Query(obj => obj.UserName == userName && obj.PassWord == passWord)).FirstOrDefault();
            if (result == null)
            {
                return false;
            }
            return true;
        }
    }
}
