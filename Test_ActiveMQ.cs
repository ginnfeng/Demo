////*************************Copyright © 2013 Feng 豐**************************	
// Created    : 10/22/2014 3:34:37 PM 
// Description: Test_ActiveMQ.cs  
//http://activemq.apache.org/nms/examples.html
//http://rantdriven.com/post/ActiveMQ-via-C-sharp-and-dotnet-using-ApacheNMS-Part-1.aspx
// Revisions  :            		
// **************************************************************************** 
using System;
using System.Collections.Generic;
using System.Text;
using UTDll;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using System.Windows.Threading;
using System.Windows.Forms;
//using System.Windows.Threading;

namespace UTool.Test
{
    class A<T>
    {
        static public implicit operator A<T>(T it)
        {
            return new A<T>() {MyProperty=it};
        }
        public T MyProperty { get; set; }

    }
    class Test_ActiveMQ : UTest
    {
        public Test_ActiveMQ()
        {
            //
            // TODO: Add constructor logic here
            //      
        }

        A<string> Fun()
        {
            return "sss";
        }
        [UMethod]
        public void T_SendMsg(string txtMessage)
        {// TODO: Add Testing logic here
            var factory = CreateFactory();
            TimeSpan receiveTimeout = TimeSpan.FromSeconds(3);
            using (IConnection connection = factory.CreateConnection())
             {
                 
                 using (ISession session = connection.CreateSession())
                 {
                     var destination = session.GetDestination(queueName);
                     IMessageProducer prod = session.CreateProducer(destination);
                     //prod.RequestTimeout = receiveTimeout;
                     //IMessageProducer prod = session.CreateProducer(new ActiveMQQueue(queueName));
                     ITextMessage message = session.CreateTextMessage();
                     
                     //给这个对象赋实际的消息
                     message.Text = txtMessage;                     
                     message.Properties["filter"]="demo";
                     //生产者把消息发送出去，几个枚举参数MsgDeliveryMode是否长链，MsgPriority消息优先级别，发送最小单位，当然还有其他重载
                     prod.Send(message, MsgDeliveryMode.NonPersistent, MsgPriority.Normal, TimeSpan.MinValue);                     
                 }
             }
        }
        [UMethod]
        public void t_Receiv()
        {
            var factory = CreateFactory();
            using (IConnection connection = factory.CreateConnection())
            {
                using (ISession session = connection.CreateSession())
                {
                    var destination = session.GetDestination(queueName);
                    using (IMessageConsumer consumer = session.CreateConsumer(destination, "filter='demo'"))
                    {
                        // Start the connection so that messages will be processed.
                        connection.Start();                       

                        // Consume a message
                        ITextMessage msg = null;
                        do
                        {
                            msg = consumer.Receive(TimeSpan.FromSeconds(1)) as ITextMessage;
                            if (msg!=null)
                                printf("接收到:{0} ID={1}", msg.Text,msg.NMSMessageId);
                            
                        } while (msg != null);
                        
                    }
                }
            }
        }
        [UMethod]
        public void t_StartListener()
        {// TODO: Add Testing logic here
             
             var factory = CreateFactory();             
             connectionListener = factory.CreateConnection();             
             //connectionListener.ClientId = "firstQueueListener";             
             connectionListener.Start();             
             ISession session = connectionListener.CreateSession();
             //new ActiveMQTopic()
             //IMessageConsumer consumer = session.CreateConsumer(new ActiveMQQueue(queueName), "filter='demo'");
             IMessageConsumer consumer = session.CreateConsumer(new ActiveMQQueue(queueName));
             consumer.Listener += OnConsumerListener;
             //messageBox("OnListener");
        }
        [UMethod]
        public void t_ExistMsg()
        {// TODO: Add Testing logic here

            var factory = CreateFactory();
            using (IConnection connection = factory.CreateConnection())
            {

                using (ISession session = connection.CreateSession())
                {
                    using (var queueBrowser = session.CreateBrowser((IQueue)new ActiveMQQueue("AppZoo.Contract.IZooActSet")))
                    {

                        connection.Start();
                        var messages = queueBrowser.GetEnumerator();
                        print(messages.MoveNext());
                    }
                }
            }
        }   

        private void OnConsumerListener(IMessage message)
        {
            ITextMessage msg = (ITextMessage)message;
            //printf("{0}{1}", msg.Text, Environment.NewLine);
            Delegate delegateMethod = new Action<ITextMessage>(UIShowMethod);
             //UTest.m_mainForm.Invoke(delegateMethod, msg);
            //printMsg(msg.Text);
            //dispatchOperator.Completed += OnDispatchOperatorCompleted;
            
        }

        void OnDispatchOperatorCompleted(object sender, EventArgs e)
        {
            //print("---");
        }

        private void UIShowMethod(ITextMessage msg)
        {
            printf("接收到:{0}{1}", msg.Text, Environment.NewLine);
            UTest.m_mainForm.Text = msg.Text;
        }

        private IConnectionFactory CreateFactory()
        {
            if (mqFactory == null)
            {
                mqFactory = new ConnectionFactory("tcp://sgx02L.cloudapp.net:61616?wireFormat.maxInactivityDuration=0");
               
                //Uri connecturi = new Uri("activemq:failover:(tcp://sgx02L.cloudapp.net:61616/)?transport.timeout=5000&transport.startupMaxReconnectAttempts=2");
                //mqFactory = NMSConnectionFactory.CreateConnectionFactory(connecturi);

            }
            return mqFactory;
        }
        //Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
        static private IConnectionFactory mqFactory;
        IConnection connectionListener;
        private string queueName = "AppZoo";

        
    }
}