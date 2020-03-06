using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyWebApiProject.Common.Util
{
    public class FileUtil : IDisposable
    {
        private bool _alreadyDispose = false;
        
        #region 构造函数
        public FileUtil()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        ~FileUtil()
        {
            Dispose(); ;
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (_alreadyDispose) return;
            _alreadyDispose = true;
        }
        #endregion
        #region IDisposable 成员
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
        #region 读文件
        /// <summary>
        /// 读文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <returns></returns>
        public static string ReadFile(string Path)
        {
            string s = "";
            if (!System.IO.File.Exists(Path))
                throw new Exception($"不存在为{Path}的路径");
            else
            {
                StreamReader f2 = new StreamReader(Path, System.Text.Encoding.UTF8);
                s = f2.ReadToEnd();
                f2.Close();
                f2.Dispose();
            }

            return s;
        } 
        #endregion
    }
}
