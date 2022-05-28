using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopWeb.Utils
{
    public class UsualUtils
    {

        /// <summary>
        /// 验证身份是否登录
        /// </summary>
        /// <returns></returns>
        public static int IsIdSession(object sessionObj)
        {
            if (sessionObj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(sessionObj);
            }
        }


        /// <summary>
        /// 获取当前的秒位时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp()
        {
            TimeSpan ts = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1);
            return (long)ts.TotalSeconds;//获取10位
        }

        public static double GetMinOrderCost()
        {
            return 20.0;
        }
    }
}