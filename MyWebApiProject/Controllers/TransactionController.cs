using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebApiProject.IRepository.UnitOfWork;
using MyWebApiProject.IService;
using MyWebApiProject.Model.DbModel;

namespace MyWebApiProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private IUserService userService;
        public TransactionController(IUnitOfWork unitOfWork,IUserService userService)
        {
            _unitOfWork = unitOfWork;
            this.userService = userService;
        }
        // GET: api/Transaction
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            try
            {
                Console.WriteLine($"");
                //开始事务
                Console.WriteLine($"Begin Transaction");
                _unitOfWork.BeginTran();
                var userEntitys = await userService.Query();
                // 第一次数据条数
                Console.WriteLine($"first time : the count of userEntitys is :{userEntitys.Count}");
                // 向表添加一条数据
                Console.WriteLine($"insert a data into the table user now.");
                var insert = await userService.Add(new UserEntity()
                {
                    PassWord = "46578",
                    UserName ="aaa"
                });
                // 第二次查看表有多少条数据，判断是否添加成功
                insert = (await userService.Query()).Count;
                Console.WriteLine($"second time : the count of User is :{insert}");
                Console.WriteLine($"");
                int ex = 0;
                // 出现了一个异常！
                Console.WriteLine($"\nThere's an exception!!");
                int throwEx = 1 / ex;
                _unitOfWork.CommitTran();
            }
            catch(Exception ex)
            {
                _unitOfWork.RollbackTran();
                var count = (await userService.Query()).Count;
                Console.WriteLine($"third time : the count of User is :{count}");
            }
            return new string[] { "value1", "value2" };
        }
   

        // POST: api/Transaction
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Transaction/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
