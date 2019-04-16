using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 八音盒配置工具
{
    public class Utility
    {
        public static string GetExtensionName(string srcString)
        {
            string extension = "";
            int index = srcString.IndexOf(".");
            if (srcString.Length > 0 && index >= 0)
            {
                extension = srcString.Substring(index + 1, srcString.Length - index - 1);
                extension.TrimEnd(' ');
            }
            return extension;
        }
        public static bool IsXlsFile(string path)
        {
            string extension = GetExtensionName(path);
            if (extension.Equals("xls") || extension.Equals("xlsx"))
            {
                return true;
            }
            return false;
        }
        public static bool IsXmlFile(string path)
        {
            string extension = GetExtensionName(path);
            if (extension.Equals("xml"))
            {
                return true;
            }
            return false;
        }
    }
}
