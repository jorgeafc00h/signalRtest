using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShuttle.Client.Desktop
{
    public static class Settings
    {
        public static string WebApiUri
        {
            get
            {
#if !DEBUG
                return ConfigurationManager.AppSettings["WebApiUrlRelease"];
#else
                return ConfigurationManager.AppSettings["WebApiUrl"];
#endif
            }
            set
            {
#if !DEBUG
                ConfigurationManager.AppSettings["WebApiUrlRelease"] = value;
#else
                ConfigurationManager.AppSettings["WebApiUrl"] = value;
#endif
            }
        }
    }
}
