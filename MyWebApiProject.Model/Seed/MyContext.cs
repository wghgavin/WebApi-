using MyWebApiProject.Common.DB;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApiProject.Model.Seed
{
    public class MyContext
    {
        private static DBConnInfo connectInfo => GetMainConnectionDb();
        private static string _connectionString = connectInfo.ConnStr;
        private static DbType _dbType = (DbType)connectInfo.DbType;
        private SqlSugarClient _db;
        /// <summary>
        /// 连接字符串 
        /// </summary>
        public static string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }
        /// <summary>
        /// 数据库类型 
        /// </summary>
        public static DbType DbType
        {
            get { return _dbType; }
            set { _dbType = value; }
        }
        /// <summary>
        /// 数据连接对象 
        /// </summary>
        public SqlSugarClient Db
        {
            get { return _db; }
            private set { _db = value; }
        }
        private static DBConnInfo GetMainConnectionDb()
        {
            var mainConnetctDb = BaseDbConfig.MutiConnectionString.Find(x => x.ConnId == BaseDbConfig.CurrentDbId);
            if (BaseDbConfig.MutiConnectionString.Count > 0)
            {
                if (mainConnetctDb == null)
                {
                    mainConnetctDb = BaseDbConfig.MutiConnectionString[0];
                }
            }
            else
            {
                throw new Exception("请确保appsettigns.json中配置连接字符串,并设置Enabled为true;");
            }
            return mainConnetctDb;
        }
        #region 构造函数
        public MyContext()
        {
            if (string.IsNullOrEmpty(_connectionString))
                throw new ArgumentNullException("数据库连接字符串为空");
            _db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = _connectionString,
                DbType = _dbType,
                IsAutoCloseConnection = true,
                IsShardSameThread = true,
                InitKeyType = InitKeyType.Attribute,
                ConfigureExternalServices = new ConfigureExternalServices()
                {
                    //DataInfoCacheService = new HttpRuntimeCache()
                },
                MoreSettings = new ConnMoreSettings()
                {
                    //IsWithNoLockQuery = true,
                    IsAutoRemoveDataCache = true
                }
            });
        }
        private MyContext(bool blnIsAutoCloseConnection)
        {
            if (string.IsNullOrEmpty(_connectionString))
                throw new ArgumentNullException("数据库连接字符串为空");
            _db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = _connectionString,
                DbType = _dbType,
                IsAutoCloseConnection = blnIsAutoCloseConnection,
                IsShardSameThread = true,
                InitKeyType = InitKeyType.Attribute, //从实体特性中读取主键自增列信息
                ConfigureExternalServices = new ConfigureExternalServices()
                {
                    //DataInfoCacheService = new HttpRuntimeCache()
                },
                MoreSettings = new ConnMoreSettings()
                {
                    //IsWithNoLockQuery = true,
                    IsAutoRemoveDataCache = true
                }
            });
        }
        #endregion
        public SimpleClient<T> GetEntityDB<T>() where T : class, new()
        {
            return new SimpleClient<T>(_db);
        }
        #region 数据库查询插入操作
        public async Task<bool> ExitList<T>()
        {
            return await _db.Queryable<T>().AnyAsync();
        }
        public async Task<bool> InsertTables<T>(object table) where T : class, new()
        {
            var tables = table as List<T>;
            if (tables==null)
            {
                return false;
            }
            if (tables.Count == 0)
            {
                return true;
            }
            return await Task.Run(() => GetEntityDB<T>().InsertRange(tables));
        }

        #endregion
        #region 根据数据库表生产实体类

        // <summary>
        /// 功能描述:根据数据库表生产实体类
        ///</summary>       
        /// <param name="strPath">实体类存放路径</param>
        public void CreateClassFileByDBTalbe(string strPath)
        {
            CreateClassFileByDBTalbe(strPath, "MyWebApiProject.Model.DbModel");
        }
        /// <summary>
        /// 功能描述:根据数据库表生产实体类
        /// </summary>
        /// <param name="strPath">实体类存放路径</param>
        /// <param name="strNameSpace">命名空间</param>
        public void CreateClassFileByDBTalbe(string strPath, string strNameSpace)
        {
            CreateClassFileByDBTalbe(strPath, strNameSpace, null);
        }
        public void CreateClassFileByDBTalbe(
            string strPath,
            string strNameSpace,
            string[] lstTableNames)
        {
            CreateClassFileByDBTalbe(strPath, strNameSpace, lstTableNames, string.Empty);
        }
        /// <summary>
        /// 功能描述:根据数据库表生产实体类
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="strPath">实体类存放路径</param>
        /// <param name="strNameSpace">命名空间</param>
        /// <param name="lstTableNames">生产指定的表</param>
        /// <param name="strInterface">实现接口</param>
        public void CreateClassFileByDBTalbe(
          string strPath,
          string strNameSpace,
          string[] lstTableNames,
          string strInterface,
          bool blnSerializable = false)
        {
            if (lstTableNames != null && lstTableNames.Length > 0)
            {
              //这里初始化类的模板
                _db.DbFirst.Where(lstTableNames).IsCreateDefaultValue().IsCreateAttribute()
                    .SettingClassTemplate(p => p = @"
{using}

namespace {Namespace}
{
    {ClassDescription}{SugarTable}" + (blnSerializable ? "[Serializable]" : "") + @"
    public partial class {ClassName}" + (string.IsNullOrEmpty(strInterface) ? "" : (" : " + strInterface)) + @"
    {
        public {ClassName}()
        {
{Constructor}
        }
{PropertyName}
    }
}
")
                    .SettingPropertyTemplate(p => p = @"
            {SugarColumn}
            public {PropertyType} {PropertyName}
            {
                get
                {
                    return _{PropertyName};
                }
                set
                {
                    if(_{PropertyName}!=value)
                    {
                        base.SetValueCall(" + "\"{PropertyName}\",_{PropertyName}" + @");
                    }
                    _{PropertyName}=value;
                }
            }")
                    .SettingPropertyDescriptionTemplate(p => p = "          private {PropertyType} _{PropertyName};\r\n" + p)
                    .SettingConstructorTemplate(p => p = "              this._{PropertyName} ={DefaultValue};")
                    .CreateClassFile(strPath, strNameSpace);
            }
            else
            {
                _db.DbFirst.IsCreateAttribute().IsCreateDefaultValue()
                    .SettingClassTemplate(p => p = @"
{using}

namespace {Namespace}
{
    {ClassDescription}{SugarTable}" + (blnSerializable ? "[Serializable]" : "") + @"
    public partial class {ClassName}" + (string.IsNullOrEmpty(strInterface) ? "" : (" : " + strInterface)) + @"
    {
        public {ClassName}()
        {
{Constructor}
        }
{PropertyName}
    }
}
")
                    .SettingPropertyTemplate(p => p = @"
            {SugarColumn}
            public {PropertyType} {PropertyName}
            {
                get
                {
                    return _{PropertyName};
                }
                set
                {
                    if(_{PropertyName}!=value)
                    {
                        base.SetValueCall(" + "\"{PropertyName}\",_{PropertyName}" + @");
                    }
                    _{PropertyName}=value;
                }
            }")
                    .SettingPropertyDescriptionTemplate(p => p = "          private {PropertyType} _{PropertyName};\r\n" + p)
                    .SettingConstructorTemplate(p => p = "              this._{PropertyName} ={DefaultValue};")
                    .CreateClassFile(strPath, strNameSpace);
            }
        }
        #endregion
        #region 根据实体类生成数据库表
        /// <summary>
        /// 功能描述:根据实体类生成数据库表
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="blnBackupTable">是否备份表</param>
        /// <param name="lstEntitys">指定的实体</param>
        public void CreateTableByEntity<T>(bool blnBackupTable, params T[] lstEntitys) where T : class, new()
        {
            Type[] lstTypes = null;
            if (lstEntitys != null)
            {
                lstTypes = new Type[lstEntitys.Length];
                for (int i = 0; i < lstEntitys.Length; i++)
                {
                    T t = lstEntitys[i];
                    lstTypes[i] = typeof(T);
                }
            }
            CreateTableByEntity(blnBackupTable, lstTypes);
        }
        /// <summary>
        /// 功能描述:根据实体类生成数据库表
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="blnBackupTable">是否备份表</param>
        /// <param name="lstEntitys">指定的实体</param>
        public void CreateTableByEntity(bool blnBackupTable, params Type[] lstEntitys)
        {
            if (blnBackupTable)
            {
                _db.CodeFirst.BackupTable().InitTables(lstEntitys); //change entity backupTable            
            }
            else
            {
                _db.CodeFirst.InitTables(lstEntitys);
            }
        }
        #endregion

    }
}
