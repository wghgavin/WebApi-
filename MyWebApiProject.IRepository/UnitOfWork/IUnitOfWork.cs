using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApiProject.IRepository.UnitOfWork
{
   public interface IUnitOfWork
    {
        SqlSugarClient GetDbClient();
        //开始事务
        void BeginTran();
        //提交事务
        void CommitTran();
        //回滚
        void RollbackTran();
    }
}
