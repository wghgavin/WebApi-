using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using MyWebApiProject.DbToJsonFileTool.Helper;
using MyWebApiProject.DbToJsonFileTool.Extension;
using System.Threading.Tasks;

namespace MyWebApiProject.DbToJsonFileTool.DB
{
    public class DbContext
    {
        public SqlSugarClient _db;
        public DbContext()
        {
            DbConfig dbConfig = ConfigHelper.GetDbConfig();
            _db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = dbConfig.ConStr,
                DbType = dbConfig.DbType,
                InitKeyType = InitKeyType.Attribute,//从特性读取主键和自增列信息
                IsAutoCloseConnection = true,//开启自动释放模式和EF原理一样我就不多解释了
            });
        }
        #region 根据数据库表创建json
        /// <summary>
        /// 获取对应表的所有直转为json
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetList<T>()
        {
            return (await _db.Queryable<T>().ToListAsync()).ToJson();
        }
        /// <summary>
        /// 使用type类型作为泛型参数使用方法
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Task<string> ExportByClassType(Type type)
        {
            return this.GetType().GetMethod("GetList").MakeGenericMethod(type).Invoke(this, null) as Task<string>;
        }
        #endregion

        #region 根据数据库表生产Model层
        /// <summary>
        /// 功能描述:根据数据库表生产Model层
        /// </summary>
        /// <param name="strPath">实体类存放路径</param>
        /// <param name="strNameSpace">命名空间</param>
        /// <param name="lstTableNames">生产指定的表</param>
        /// <param name="strInterface">实现接口</param>
        /// <param name="blnSerializable">是否序列化</param>
        public void Create_Model_ClassFileByDBTalbe(
          string strPath,
          string strNameSpace,
          string[] lstTableNames,
          string strInterface =null,
          bool blnSerializable = false)
        {
            var IDbFirst = _db.DbFirst;
            if (lstTableNames != null && lstTableNames.Length > 0)
            {
                IDbFirst = IDbFirst.Where(lstTableNames);
            }
            IDbFirst.IsCreateDefaultValue().IsCreateAttribute()

                .SettingClassTemplate(p => p = @"
{using}

namespace " + strNameSpace + @"
{
    {ClassDescription}{SugarTable}" + (blnSerializable ? "[Serializable]" : "") + @"
    public class {ClassName}" + (string.IsNullOrEmpty(strInterface) ? "" : (" : " + strInterface)) + @"
    {
        public {ClassName}()
        {
        }
        {PropertyName}
    }
}
                    ")

                .SettingPropertyTemplate(p => p = @"
        {SugarColumn}
        public {PropertyType} {PropertyName} { get; set; }

            ")

                 //.SettingPropertyDescriptionTemplate(p => p = "          private {PropertyType} _{PropertyName};\r\n" + p)
                 //.SettingConstructorTemplate(p => p = "              this._{PropertyName} ={DefaultValue};")

                 .CreateClassFile(strPath, strNameSpace);

        }
        #endregion
    }
}
