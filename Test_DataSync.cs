////*************************Copyright © 2008 Feng 豐**************************	
// Created    : 9/26/2011 11:27:10 AM 
// Description: Test_SysAgentService.cs  
// Revisions  :            		
// **************************************************************************** 
using System;
using System.Collections.Generic;
using System.Text;
using UTDll;
using Support.Net.WCF;
using ClientProxy;
using CMS.PolicyLib.Action;
using System.ServiceModel;
using DataSyncService.Core;
using Common.DataCore;
using Support.Serializer;
using Common.DataContract;
using CMS.PolicyLib.Entity;
using CMS.PolicyLib.Extension;
using CMS.ServiceFacade;
//GIT Test *******
namespace UTool.Test
{
    public class Test_DataSync : UTest
    {
        public Test_DataSync()
        {
            //
            // TODO: Add constructor logic here
            //      
        }

        [UMethod]
        public void T_DataSyncAction()
        {// TODO: Add Testing logic here
            DataSyncAction action = new DataSyncAction();
            action.UpdateCurrentUsers2SQL(); 
        }

        [UMethod]
        public void T_CMS2History()
        {// TODO: Add Testing logic here
            DataSyncAction action = new DataSyncAction();
            action.SynCMSUser2History();
        }

        [UMethod]
        public void T_DataSyncService()
        {// TODO: Add Testing logic here
            SvcProxy<ICmdService> proxy = new SvcProxy<ICmdService>();
            CmdParameter rlt;
            using (proxy.CreateOperationContextScope())
            {
                proxy.AddMessageHeader("ICmdService", "", "DataSyncService");
                //rlt = proxy.Service.Exec("api.DataSync.Foo(true)");
                //print(rlt.Value);
                rlt = proxy.Service.Exec("api.DataSync.UpdateCurrentUsers2SQL()");
                print(rlt.Value);
            }

        }

        [UMethod]
        public void t_DataSyncExecutor()
        {// TODO: Add Testing logic here
            var actSet = new IDomainAction[] { new DataSyncAction() };
            var executor = new CmdExecutor<IDataSyncActionSet>(actSet);

            //var rlt = executor.Exec<bool>("api.DataSync.Foo(true)");
            //print(rlt.Value);
            var rlt = executor.Exec<bool>("api.DataSync.UpdateCurrentUsers2SQL()");
            print(rlt.Value);
        }

    }

}
