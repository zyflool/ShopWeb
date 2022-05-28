using ShopWeb.DAL;
using ShopWeb.Models;
using System.Collections.Generic;

namespace ShopWeb.BLL
{
    /// <summary>
    /// 用户业务逻辑类
    /// </summary>
    public class UserManager
    {
        /// <summary>
        /// 检查账户名和密码是否正确
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns>正确返回对应用户id，反之返回0</returns>
        public static int CheckNameAndPassword(string name, string password)
        {
            if (name == null || password == null)
            {
                return 0;
            }

            Models.User user = UserService.GetUserByName(name);

            if (user == null || !password.Equals(user.Password))
            {
                return 0;
            }
            return user.Id;
        }

        /// <summary>
        /// 检查id和密码是否对应
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns>正确返回对应用户id，反之返回0</returns>
        public static int CheckIdAndPassword(int userId, string password)
        {
            if (userId <= 0 || password == null)
            {
                return 0;
            }

            User user = UserService.GetUserById(userId);

            if (user == null || !password.Equals(user.Password))
            {
                return 0;
            }
            return user.Id;
        }

        public static List<Models.User> GetUsers()
        {
            return UserService.GetUser();
        }

        public static List<Models.User> SearchUsers(string content, string search)
        {
            if ("id".Equals(content))
            {
                return UserService.SearchUserById(search);
            }
            else if ("name".Equals(content))
            {
                return UserService.SearchUserByName(search);
            }
            else
            {
                return new List<User>();
            }
        }

        public static Models.User GetUserById(int userId)
        {
            Models.User user = UserService.GetUserById(userId);
            return user;
        }

        public static int FindUserId(string name)
        {
            Models.User user = UserService.GetUserByName(name);
            return user != null ? user.Id : 0;
        }

        /// <summary>
        /// 注册账号
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="phone"></param>
        /// <returns>注册成功返回新的用户id，反之返回0</returns>
        public static int Register(string name, string password, string phone)
        {
            if (name == null || password == null || phone == null)
            {
                return 0;
            }

            if (UserService.GetUserByName(name) != null)
            {
                return 0;
            }

            if (UserService.AddUserAccount(new Models.User() {
                Name = name,
                Password = password,
                Phone = phone
            }))
            {
                int userId = FindUserId(name);
                // 注册成功后创建用户的购物车
                CartManager.GenerateNewCartObject(userId);
                return userId;
            }
            else
            {
                return 0;
            }
        }

        public static bool UpdateCartId(int userId, int cartId)
        {
            return UserService.UpdateUserCartId(userId, cartId);
        }

        public static bool UpdatePhoneNumber(int userId, string phone)
        {
            return UserService.UpdateUserPhone(userId, phone);
        }
        public static bool ChangePassword(int userId, string password)
        {
            return UserService.UpdateUserPassword(userId, password);
        }
    }
}
