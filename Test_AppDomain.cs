using Support.Net.Util;
////*************************Copyright © 2013 Feng 豐**************************	
// Created    : 9/30/2014 5:30:05 PM 
// Description: Test_AppDomain.cs  
// Revisions  :            		
// **************************************************************************** 
using System;
using System.Collections.Generic;
using System.Text;
using UTDll;
namespace UTool.Test
{
    [Serializable]
    public class MyData //: MarshalByRefObject
    {
        public string Name { get; set; }
    }
    public class A:MarshalByRefObject
    {
        public A (){}
        public DateTime GetTm(){return DateTime.Now;}
        public string GetDomainName() {
            //throw new Exception("******");
            return AppDomain.CurrentDomain.FriendlyName; 
        }
        public MyData SetData(MyData a)
        {
            MyData it = new MyData();
            it.Name = a.Name;
            return it;
        }
        static public string GetDomainName2() { return AppDomain.CurrentDomain.FriendlyName; }
    }
    class Test_AppDomain : UTest
    {
        public Test_AppDomain()
        {
            //
            // TODO: Add constructor logic here
            //      
            
        }
        [UMethod]
        public void T_PassParam()
        {// TODO: Add Testing logic here
            var factory = AppDomainFactory.Instance;            
            var appDomain = factory.TakeNewAppDomain("Test");            
            var a = appDomain.Entity.CreateInstance<A>();
            var tm=a.GetTm();
            var dName1 = a.GetDomainName();
            var dName2 = A.GetDomainName2();
            this.printf("{0}  {1} {2}", a.GetTm(), a.GetDomainName(), A.GetDomainName2());

            var data = new MyData() { Name = "11111" };
            var data2=a.SetData(data);
            var name = data2.Name;
        }
    }
}
