using AutoMapper;
using MyWebApiProject.Model;
using MyWebApiProject.Model.DbModel;
using MyWebApiProject.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApiProject.AutoMapper
{
    public class CustomProfile : Profile
    {
        /// <summary>
        /// 根据IMapperTo接口 自动初始化AutoMapper
        /// </summary>
        public CustomProfile()
        {
            typeof(BlogArticle).Assembly.GetTypes()
                .Where(i => i.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IMapperTo<>)
                    )).ToList().ForEach(
                       item => {
                           item.GetInterfaces()
                              .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IMapperTo<>))
                              .ToList()
                              .ForEach(i => {
                                  var t2 = i.GetGenericArguments()[0];
                                  CreateMap(item, t2);
                                  CreateMap(t2, item);
                              });
                    });
        }
    }
}
