using ShopWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.DAL
{
    public class ManagerService
    {
        public static List<Manager> GetManager()
        {
            return GetManagerBySql("");
        }

        public static Manager GetManagerById(int id)
        {
            string whereSql = string.Format("where manager_id = {0}", id);
            List<Manager> list = GetManagerBySql(whereSql);
            if (list == null || list.Count < 1)
            {
                return null;
            }
            else
            {
                return list[0];
            }
        }

        public static Manager GetManagerByName(string name)
        {
            string whereSql = string.Format("where manager_name = \'{0}\'", name);
            List<Manager> list = GetManagerBySql(whereSql);
            if (list == null || list.Count < 1)
            {
                return null;
            }
            else
            {
                return list[0];
            }
        }
        
        private static List<Manager> GetManagerBySql(string whereSql)
        {
            string sql = "select * from Manager ";
            sql += whereSql;
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<Manager> list = new List<Manager>();
            while (objReader.Read())
            {
                list.Add(new Manager()
                {
                    Id = Convert.ToInt32(objReader["manager_id"]),
                    Name = Convert.ToString(objReader["manager_name"]),
                    Password = Convert.ToString(objReader["manager_password"])
                });
            }
            objReader.Close();
            return list;
        }


        #region 修改密码
        public static bool UpdateManagerPassword(int id, string password)
        {
            string updateSql = "SET manager_password = "+password+" ";
            updateSql += "WHERE manager_id = "+id;
            return UpdateManagerBySql(updateSql);
        }

        public static bool UpdateManagerBySql(string updateSql)
        {
            string sql = "UPDATE Manager ";
            sql += updateSql;
            try
            {
                SQLHelper.Update(sql);
                return true;
            } catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
    }
}
