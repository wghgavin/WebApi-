using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebApiProject.IService;
using MyWebApiProject.Model.DbModel;
namespace MyWebApiProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderInfoController : ControllerBase
    {
        IOrderInfoService orderInfoService;
        public OrderInfoController(IOrderInfoService orderInfoService)
        {
            this.orderInfoService = orderInfoService;
        }
        /// <summary>
        /// 获取所有订单信息
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetAllOrderInfo")]
        public async Task<List<OrderInfoEntity>> GetAllOrderInfo()
        {
            return await orderInfoService.Query();
        }
        /// <summary>
        /// 增加订单信息
        /// </summary>
        /// <param name="orderInfoEntity"></param>
        /// <returns></returns>
        [HttpPost(Name = "AddOrderInfo")]
       
        public async Task<int> AddOrderInfo(OrderInfoEntity orderInfoEntity)
        {
            return await orderInfoService.Add(orderInfoEntity);
        }
    }
}
