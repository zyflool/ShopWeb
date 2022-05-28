using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopWeb.DAL;
using ShopWeb.Models;

namespace ShopWeb.BLL
{
    /// <summary>
    /// 管理员业务逻辑类
    /// </summary>
    public class ManagerManager
    {
        public static bool CheckIdExist(int managerId)
        {
            return ManagerService.GetManagerById(managerId) != null;
        }
        public static bool CheckIdAndPassword(string name, string password)
        {
            if (name == null || password == null)
            {
                return false;
            }

            Manager Manager = ManagerService.GetManagerByName(name);

            if (Manager == null || !password.Equals(Manager.Password))
            {
                return false;
            }
            return true;
        }

        public static bool CheckIdAndPassword(int managerId, string password)
        {
            if (managerId == 0 || password == null)
            {
                return false;
            }

            Manager manager = ManagerService.GetManagerById(managerId);

            if (manager == null || !password.Equals(manager.Password))
            {
                return false;
            }
            return true;
        }

        public static bool ChangePassword(int id, string newPassword)
        {
            return ManagerService.UpdateManagerPassword(id, newPassword);
        }

        public static Manager GetManagerByName(string name)
        {
            return ManagerService.GetManagerByName(name);
        }

    }
}
