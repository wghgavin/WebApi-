﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebApiProject.Common;
using MyWebApiProject.Common.Util;
using MyWebApiProject.IService;
using MyWebApiProject.Model.DbModel;
using StackExchange.Profiling;

namespace MyWebApiProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GoodsController : ControllerBase
    {
        IGoodsService goodsService;
        public GoodsController(IGoodsService goodsService)
        {
            this.goodsService = goodsService;
        }
        /// <summary>
        /// 获取所有商品列表
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetAllGoodsInfo")]

        public async Task<List<GoodsEntity>> GetAllGoodsInfo()
        {
            return await goodsService.Query();
        }
    }
}
