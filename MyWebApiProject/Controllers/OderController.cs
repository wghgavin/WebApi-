using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using MyWebApiProject.IService;
using MyWebApiProject.Model;
using MyWebApiProject.Model.DbModel;
using MyWebApiProject.Model.ViewModel;

namespace MyWebApiProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OderController : ControllerBase
    {
        IAreaInfoService areaInfoService;
        IOrderIdInfoService orderIdInfoService;
        IShopInfoService shopInfoService;
        IGoodsService goodsService;
        IOrderInfoService orderInfoService;
        public OderController(IAreaInfoService areaInfoService, IOrderIdInfoService orderIdInfoService, IShopInfoService shopInfoService, IGoodsService goodsService, IOrderInfoService orderInfoService)
        {
            this.areaInfoService = areaInfoService;
            this.orderIdInfoService = orderIdInfoService;
            this.shopInfoService = shopInfoService;
            this.goodsService = goodsService;
            this.orderInfoService = orderInfoService;
        }
        /// <summary>
        /// 请求某个地区的拼单的所有商家名
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<AreaShopInfo>> GetAreaInfos(string areaName)
        {
            var areaInfo = await areaInfoService.GetAreaInfoByName(areaName);
            if (areaInfo != null)
            {
                var shopInfos = await shopInfoService.GetShopInfoByAreaId(areaInfo.Id);
                AreaShopInfo areaShopInfo = new AreaShopInfo
                {
                    AreaName = areaInfo.AreaName,
                    SharingNum = areaInfo.SharingNum,
                    StartPrice = areaInfo.StartPrice,
                    LeftTime = areaInfo.LeftTime,
                    PlatFormCode = areaInfo.PlatFormCode,
                    shopNames = shopInfos?.Select(obj => obj.ShopName).ToList()
                };
                return new MessageModel<AreaShopInfo>
                {
                    success = true,
                    response = areaShopInfo
                };
            }
            return new MessageModel<AreaShopInfo>
            {
                success = true,
                msg = "无相关地区信息"
            };
        }
        /// <summary>
        /// 根据同户名获取所有拼单唯一标识码
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<List<string>>> GetOrderIdByUserName(string userName)
        {
            var result = await orderIdInfoService.GetOrderIdByUserName(userName);
            return new MessageModel<List<string>>
            {
                success = true,
                response = result?.Select(obj => obj.OrderId).ToList()
            };
        }
        /// <summary>
        /// 请求发布拼单信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<int>> PublishOrderInfo([FromBody]OrderInfoEntity orderInfoEntity)
        {
           var result = await orderInfoService.Add(orderInfoEntity);
            if (result > 0)
            {
                return new MessageModel<int>
                {
                    success = true,
                    msg="上传成功",
                    response = result
                };
            }
            else
            {
                return new MessageModel<int>
                {
                    success = false,
                    msg = "上传失败",
                    response = result
                };
            }
        }
        /// <summary>
        /// 确认拼单
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<bool>> ConfirmOrder(int orderId)
        {
            var result = await orderInfoService.QueryByID(orderId);
            result.Status = 7;
            var success = await orderInfoService.Update(result);
            return new MessageModel<bool>
            {
                success = success
            };
        }

        /// <summary>
        /// 取消拼单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<bool>> CancelOrder(int orderId)
        {
            var result = await orderInfoService.QueryByID(orderId);
            result.Status = 2;
            var success = await orderInfoService.Update(result);
            return new MessageModel<bool>
            {
                success = success
            };
        }



        /// <summary>
        /// 使用拼单唯一识别码请求拼单状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<string>> GetOrderInfoByOrderId(int orderId)
        {
            var result = await orderInfoService.QueryByID(orderId);
            string status = "";
            switch (result?.Status)
            {
                case 1:
                    status = "发布成功";
                    break;
                case 2:
                    status = "拼主取消拼单";
                    break;
                case 3:
                    status = "拼客取消拼单";
                    break;
                case 4:
                    status = "拼单过期";
                    break;
                case 5:
                    status = "拼主已付款";
                    break;
                case 6:
                    status = "拼客已付款";
                    break;
                case 7:
                    status = "拼单成功";
                    break;
                default:
                    break;
            }
            return new MessageModel<string>
            {
                success = true,
                response=status
            };
        }

    }
}