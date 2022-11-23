using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabManagementServiceTests.Service
{
    public class UserDetailServiceDuplicate
    {
        private readonly List<TbUser> userList;
        private readonly List<TbUser> userDisplay;
        public UserDetailServiceDuplicate()
        {
            userList = new List<TbUser>()
             {
                new TbUser() { UserId=1, FirstName="jyothi", LastName="V", EmailId="jyothi@gmail.com",  Password="1234", UserRoleId=1, CreateDate=DateTime.Now, UpdateDate=null, Status=1},
                new TbUser() { UserId=2, FirstName="vanshika", LastName="Agarwal", EmailId="vanshika@gmail.com",  Password="1234", UserRoleId=1, CreateDate=DateTime.Now, UpdateDate=null,Status=1},
                new TbUser() { UserId=3, FirstName="avezz", LastName="mohammed", EmailId="avezz@gmail.com",  Password="1234", UserRoleId=1, CreateDate=DateTime.Now, UpdateDate=null,Status=1},
            };
            userDisplay = new List<TbUser>()
            {
                new TbUser() { UserId=1, FirstName="jyothi", LastName="V", EmailId="bvgokulgok@gmail.com",UserRoleId=1, CreateDate=DateTime.Now, UpdateDate=null, Status=1},
                new TbUser() { UserId=2, FirstName="vanshika", LastName="Agarwal", EmailId="diptesh@gmail.com", UserRoleId=1, CreateDate=DateTime.Now, UpdateDate=null, Status=1},
                new TbUser() { UserId=3, FirstName="avezz", LastName="mohammed", EmailId="kazeem@gmail.com",  UserRoleId=1, CreateDate=DateTime.Now, UpdateDate=null, Status=1},
            };
        }
    }
        public List<TbUser> GetUser()
        {
            return userDisplay.ToList();
        }
        public bool CheckExtistUser(Registration user)
        {
            TblUser user1 = userList.Where(x => x.Email == user.Email).FirstOrDefault()!;
            if (user1 is null)
            {
                return false;
            }
            return true;
        }
        public bool CheckPassword(Registration user)
        {
            TblUser user1 = userList.Where(x => x.Email == user.Email).FirstOrDefault()!;
            if (user.Password == user.ConfirmPassword)
            {
                return true;
            }
            return false;
        }
        public bool Registration(Registration user)
        {
            user.CreatedDate = DateTime.Now;
            user.UpdatedDate = null;
            user.Active = true;
            userList.Add(user);
            return true;
        }
        public bool LogIn(TblUser login)
        {
            TblUser user = userList.Where(x => x.Email == login.Email && x.Password == login.Password!).FirstOrDefault()!;
            if (user != null)
            {
                return true;
            }
            return false;
        }
        public bool ForgetPassword(Registration changePassword)
        {
            int index = userList.FindIndex(item => item.Email == changePassword.Email);
            TblUser user1 = userList.Where(x => x.Email == changePassword.Email).FirstOrDefault()!;
            user1.Password = changePassword.Password;
            user1.UpdatedDate = DateTime.Now;
            userList[index] = user1;
            return true;
        }
    }
}

    }
}
