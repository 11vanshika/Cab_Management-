using Domain;
using Microsoft.EntityFrameworkCore;
using Service.Inteface;
using Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Persistence;
using System.Runtime.Intrinsics.X86;

namespace CabManagementServiceTests.Service
{
    public class UserDetailServiceDuplicate : IUserDetails
    {
        private readonly List<TbUser> userList;
        private readonly IEncrypt _encrypt;
        public UserDetailServiceDuplicate()
        {
            _encrypt = new Encrypt();

            userList = new List<TbUser>()
             {
                new TbUser() { UserId=1, FirstName="jyothi", LastName="V", EmailId="jyothi@gmail.com",  Password="1234", UserRoleId=1, CreateDate=DateTime.Now, UpdateDate=null, Status=1},
                new TbUser() { UserId=2, FirstName="vanshika", LastName="Agarwal", EmailId="vanshika@gmail.com",  Password="1234", UserRoleId=1, CreateDate=DateTime.Now, UpdateDate=null,Status=1},
                new TbUser() { UserId=3, FirstName="avezz", LastName="mohammed", EmailId="avezz@gmail.com",  Password="1234", UserRoleId=1, CreateDate=DateTime.Now, UpdateDate=null,Status=1},
            };
        }


        public List<TbUser> GetUsersDetails()
        {
            return userList.ToList();
        }
        public bool CheckExtistUser(TbUser tbluser)
        {
            TbUser user1 = userList.Where(x => x.EmailId == tbluser.EmailId).FirstOrDefault()!;
            if (user1 is null)
            {
                return false;
            }
            return true;
        }
        public bool CheckConfirmPassword(Login tbluser)
        {
            TbUser user1 = userList.Where(x => x.EmailId == tbluser.EmailId).FirstOrDefault()!;
            if (tbluser.Password == tbluser.ConfirmPassword)
            {
                return true;
            }
            return false;
        }
        public bool CheckExtistUser(Login login)
        {
            var Email = userList.Where(x => x.EmailId == login.EmailId).FirstOrDefault();
            if (Email == null)
            {
                return false;
            }
            return true;
        }
        public bool Register(TbUser tbluser)
        {
            tbluser.CreateDate = DateTime.Now;
            tbluser.UpdateDate = null;
            tbluser.Status = 1;
            userList.Add(tbluser);
            return true;
        }
        public bool UserLogin(Login login)
        {
            TbUser user = userList.Where(x => x.EmailId == login.EmailId && x.Password == _encrypt.EncodePasswordToBase64(login.Password)).FirstOrDefault();

            if (user != null)
            {
                return true;
            }
            return false;
        }
        public bool ForgotPassword(Login login)
        {
            int index = userList.FindIndex(item => item.EmailId == login.EmailId);
            TbUser user1 = userList.Where(x => x.EmailId == login.EmailId).FirstOrDefault()!;
            user1.Password = _encrypt.EncodePasswordToBase64(login.Password);
            user1.Password = login.Password;
            user1.UpdateDate = DateTime.Now;
            userList[index] = user1;
            return true;
        }
    }
}





