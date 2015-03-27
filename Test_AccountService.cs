////*************************Copyright © 2010 Sean Hsu**************************	
// Created    : 7/28/2011 11:23:00 AM 
// Description: Test_AccountService.cs  
// Revisions  :            		
// **************************************************************************** 
using System;
using System.Collections.Generic;
using System.Text;
using UTDll;
using AuthService.Contract;
using AuthService.Core;
using ClientProxy;
using Support.Net.Security;
using System.Globalization;
namespace UTool.Test
{
    class Test_AccountService : UTest
    {
        public Test_AccountService()
        {
            //
            // TODO: Add constructor logic here
            //      
        }


        [UMethod]
        public void Test_AuthSerevice_ChangePwd(string uid,string oldPwd, string newPwd)
        {// TODO: Add Testing logic here
            IAuthService service = SvcProxy<IAuthService>.Instance.Service;
            // "0\\systemadm\\", "SystemAdm", "1234"); 
            //printf(" Result: {0}", service.ChangePassword(uid,oldPwd,newPwd));
            // printf(" Result: {0}", service.ChangePassword("0\\systemadm\\", "SystemAdm", "1234"));
            printf(" Result: {0}", service.ChangePassword(uid,oldPwd,newPwd));
            
        }


        [UMethod]
        public void Test_AuthSerevice_Imp_ChangePwd(string uid, string oldPwd, string newPwd)
        {// TODO: Add Testing logic here
            AuthService.Core.AuthService service = new AuthService.Core.AuthService();
            // "0\\systemadm\\", "SystemAdm", "1234"); 
            //printf(" Result: {0}", service.ChangePassword(uid,oldPwd,newPwd));
            // printf(" Result: {0}", service.ChangePassword("0\\systemadm\\", "SystemAdm", "1234"));
            printf(" Result: {0}", service.ChangePassword(uid, oldPwd, newPwd));

        }

        [UMethod]
        public void Test_AuthSerevice_Auth(string uid, string oldPwd)
        {// TODO: Add Testing logic here
            IAuthService service = SvcProxy<IAuthService>.Instance.Service;
            // "0\\systemadm\\", "SystemAdm", "1234"); 
            //printf(" Result: {0}", service.ChangePassword(uid,oldPwd,newPwd));
            // printf(" Result: {0}", service.ChangePassword("0\\systemadm\\", "SystemAdm", "1234"));
            printf(" Result: {0}", service.Authenticate(uid, oldPwd, ""));

        }


        [UMethod]
        public void Test_AuthSerevice_Imp_Auth(string uid, string oldPwd)
        {// TODO: Add Testing logic here
            AuthService.Core.AuthService service = new AuthService.Core.AuthService();
            // "0\\systemadm\\", "SystemAdm", "1234"); 
            //printf(" Result: {0}", service.ChangePassword(uid,oldPwd,newPwd));
            // printf(" Result: {0}", service.ChangePassword("0\\systemadm\\", "SystemAdm", "1234"));
            printf(" Result: {0}", service.Authenticate(uid, oldPwd,""));

        }

        

        [UMethod]
        public void Test_ServiceImpl_Create()
        {// TODO: Add Testing logic here

            AccountServiceImp imp = new AccountServiceImp();
            imp.CreateUser(new UserInfo { Account = "123456", Name = "Sean", TenantId = "T-Cat" }, "123456");
            imp.CreateUser(new UserInfo { Account = "234567", Name = "Ken", TenantId = "T-Cat" }, "234567");
            imp.CreateUser(new UserInfo { Account = "111111", Name = "Joe", TenantId = "T-Cat" }, "111111");
        }

        [UMethod]
        public void Test_ServiceImpl_CheckPwd(string tenantId, string user, string agentid, string pwd)
        {
            AccountServiceImp imp = new AccountServiceImp();
            printf(" Result: {0}", imp.CheckPassword(new UserInfo { Account = user, Name = "Sean", AgentId = agentid, TenantId = tenantId }, Encrypt(pwd)));
        }

        [UMethod]
        public void Test_ServiceImpl_ChangePwd(string oldPwd, string pwd)
        {
            AccountServiceImp imp = new AccountServiceImp();
            imp.ResetPassword(new UserInfo { Account = "123456", Name = "Sean", TenantId = "T-Cat" }, "123456");
            printf(" Result: {0}", imp.ChangePassword(new UserInfo { Account = "123456", Name = "Sean", TenantId = "T-Cat" }, oldPwd, pwd));
        }

        [UMethod]
        public void Test_ServiceImpl_ResetPwd(string tenantId, string user)
        {
            AccountServiceImp imp = new AccountServiceImp();

            imp.ResetPassword(new UserInfo { Account = user, Name = "Sean", TenantId = tenantId }, Encrypt(user));
            printf(" Result: {0}", imp.CheckPassword(new UserInfo { Account = user, Name = "Sean", TenantId = tenantId }, Encrypt(user)));
        }

        [UMethod]
        public void Test_ServiceImpl_Delete()
        {// TODO: Add Testing logic here

            AccountServiceImp imp = new AccountServiceImp();
            imp.DeleteUser (new UserInfo { Account = "123456", Name = "Sean Hsu", TenantId = "T-Cat" });
        }

        [UMethod]
        public void Test_ServiceImpl_QueryTenantUser()
        {// TODO: Add Testing logic here

            AccountServiceImp imp = new AccountServiceImp();
            imp.QueryTenantUsers("T-Cat");
        }

        [UMethod]
        public void Test_Service_CheckPwd(string tenantId, string user, string agentid, string pwd)
        {// TODO: Add Testing logic here
            SvcProxy<IAccountService> proxy = new SvcProxy<IAccountService>();
            printf(" Result: {0}", proxy.Service.CheckPassword (new UserInfo { Account = user, Name = "Sean",AgentId =agentid, TenantId = tenantId }, Encrypt(pwd)));
        }

        [UMethod]
        public void Test_Service_ResetPwd(string tenantId, string user, string pwd)
        {// TODO: Add Testing logic here
            IAccountService service = SvcProxy<IAccountService>.Instance.Service;
            service.ResetPassword(new UserInfo { Account = user, TenantId = tenantId }, Encrypt(pwd), false);
        }

        [UMethod]
        public void Test_Service_Create(string tenantId, string user, string pwd)
        {// TODO: Add Testing logic here

            IAccountService Service = SvcProxy<IAccountService>.Instance.Service;

            Service.CreateUser(new UserInfo { Account = user, Name = "Sean", TenantId = tenantId }, Encrypt(pwd));
            
        }

        [UMethod]
        public void Test_Service_ChangePwd(string tenantId, string user, string oldPwd, string newPwd)
        {// TODO: Add Testing logic here

            IAccountService Service = SvcProxy<IAccountService>.Instance.Service;

            printf(" Result: {0}",Service.ChangePassword(new UserInfo { Account = user, Name = "Sean", TenantId = tenantId }, Encrypt(oldPwd), Encrypt(newPwd)));

        }

        [UMethod]
        public void Test_DateTime(string datetime)
        {// TODO: Add Testing logic here

            DateTime a= DateTime.ParseExact(datetime, "MMddyyyyHHmm", CultureInfo.InvariantCulture);

            printf(" Result: {0}",a.ToString() );

        }

        
        private string Encrypt(string str)
        {
            return Crypto.TodayCrypto.Encrypt(str);
        }
    }
}
