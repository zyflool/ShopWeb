using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ShopWeb.DAL
{
    public class UserService
    {
        public static List<Models.User> GetUser()
        {
            return GetUserBySql("");
        }
        public static List<Models.User> SearchUserByName(string search)
        {
            return GetUserBySql(string.Format("where user_name like \'%{0}%\'",search));
        }

        public static List<Models.User> SearchUserById(string search)
        {
            return GetUserBySql(string.Format("where user_id = {0}", search));
        }
        public static Models.User GetUserByName(string name)
        {
            string whereSql = string.Format("where user_name = \'{0}\'", name);
            List<Models.User> users = GetUserBySql(whereSql);
            if (users == null || users.Count == 0)
            {
                return null;
            }
            else
            {
                return users[0];
            }
        }

        public static Models.User GetUserById(int id)
        {
            string whereSql = string.Format("where user_id = {0}", id);
            List<Models.User> users = GetUserBySql(whereSql);
            if(users == null || users.Count == 0)
            {
                return null;
            }
            else
            {
                return users[0];
            }
        }

        private static Models.User GetSingleUserBySql(string whereSql)
        {
            string sql = "select * from User";
            sql += whereSql;
            object obj = SQLHelper.GetSingleResult(sql);
            if (obj == null)
            {
                return null;
            }
            return (Models.User)obj;
        }

        private static List<Models.User> GetUserBySql(string whereSql)
        {
            string sql = "select * from [User] ";
            sql += whereSql;
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<Models.User> list = new List<Models.User>();
            while (objReader.Read())
            {
                list.Add(new Models.User()
                {
                    Id = Convert.ToInt32(objReader["user_id"]),
                    Name = Convert.ToString(objReader["user_name"]),
                    Phone = Convert.ToString(objReader["user_phone"]),
                    Password = Convert.ToString(objReader["user_password"])
                });
            }
            objReader.Close();
            return list;
        }

        public static bool AddUserAccount(Models.User user)
        {
            string sqlString = "INSERT INTO [User] (user_name, user_password, user_phone) " +
                "VALUES ('" + user.Name + "', '" + user.Password + "', '" + user.Phone + "')";
            try
            {
                SQLHelper.Update(sqlString);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public static bool UpdateUserCartId(int userId, int cartId)
        {
            string sql = string.Format("update [User] set cart_id = {0} where user_id = {1}",cartId, userId);
            try
            {
                SQLHelper.Update(sql);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public static bool UpdateUserPhone(int userId, string phone)
        {
            string sql = string.Format("update [User] set user_phone = \'{0}\' where user_id = {1}", phone, userId);
            try
            {
                SQLHelper.Update(sql);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public static bool UpdateUserPassword(int userId, string password)
        {
            string sql = string.Format("update [User] set user_password = \'{0}\' where user_id = {1}", password, userId);
            try
            {
                SQLHelper.Update(sql);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }


    }
}
