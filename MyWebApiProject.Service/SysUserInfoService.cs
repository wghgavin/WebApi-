using MyWebApiProject.Common.Util;
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
    public class SysUserInfoService : BaseService<SysUserInfo>, ISysUserInfoService
    {
        ISysUserInfoRepository _dal;
        IUserRoleRepository _userRoleRepository;
        IRoleRepository _roleRepository;
        public SysUserInfoService(ISysUserInfoRepository dal, IUserRoleRepository userRoleRepository, IRoleRepository roleRepository)
        {
            this._dal = dal;
            this._userRoleRepository = userRoleRepository;
            this._roleRepository = roleRepository;
            base.baseDal = dal;
        }
        public async Task<string> GetUserRoleNameStr(string loginName, string loginPwd)
        {
            string roleName = "";
            var user = (await base.Query(x => x.UloginName == loginName && x.UloginPWD == loginPwd)).FirstOrDefault();//查询用户
            var roleList = await _roleRepository.Query(a => a.IsDeleted == false);//查询所有角色
            if (user != null)
            {
                var userRoles = await _userRoleRepository.Query(ur => ur.UserId == user.UiD);//查询所有user跟role的关系表
                if (userRoles.Count > 0)
                {
                    var arr = userRoles.Select(ur => ur.RoleId.ObjectToString()).ToList();
                    var roles = roleList.Where(r => arr.Contains(r.Id.ObjectToString()));
                    roleName = string.Join(',', roles.Select(r => r.Name).ToArray());
                }
            }
            return roleName;
        }

        public async Task<SysUserInfo> SaveUserInfo(string loginName, string loginPwd)
        {
            SysUserInfo sysUserInfo = new SysUserInfo(loginName, loginPwd);
            SysUserInfo model = new SysUserInfo();
            var userList = await base.Query(a => a.UloginName == sysUserInfo.UloginName && a.UloginPWD == sysUserInfo.UloginPWD);
            if (userList.Count > 0)
            {
                model = userList.FirstOrDefault();
            }
            else
            {
                var id = await base.Add(sysUserInfo);
                model = await base.QueryByID(id);
            }
            return model;
        }
    }
}
