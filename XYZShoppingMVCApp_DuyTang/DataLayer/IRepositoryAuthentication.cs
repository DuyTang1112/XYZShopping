using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYZShoppingMVCApp_DuyTang.Models.DomainModels;

namespace XYZShoppingMVCApp_DuyTang.DataLayer
{
    public interface IRepositoryAuthentication
    {
        int CheckIfValidUser(string username, string password);
        string GetRolesForUser(string username);
        bool ChangePassword(string username, string oldPassword, string newPassword);
        bool AddUser(RegisterUserVM registerUserVM);
    }
}
