////*************************Copyright © 2013 Feng 豐**************************	
// Created    : 4/1/2014 3:17:48 PM 
// Description: Test_DataService.cs  
// Revisions  :            		
// **************************************************************************** 

// http://msdn.microsoft.com/zh-tw/library/dd673933%28v=vs.110%29.aspx
//http://msdn.microsoft.com/zh-tw/library/dd756368%28v=vs.110%29.aspx


//http://msdn.microsoft.com/zh-tw/library/ff650917%28v=vs.95%29.aspx

using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Text;
using UTDll;
using UTool.NorthwndServiceReference;
using System.Linq;
//using System.Data.Services.Client;

namespace UTool.Test
{
    class Test_DataService : UTest
    {
        public Test_DataService()
        {
            //
            // TODO: Add constructor logic here
            //                  
        }
        private UserMgrDataServiceContext GetNewContext()
        {
            return new UserMgrDataServiceContext(new Uri("http://localhost:1570/DataService.svc/"));
        }
        [UMethod]
        public void T1()
        {// TODO: Add Testing logic here
            var src = GetNewContext();
            foreach (var customer in src.Customers)
            {
                
            }            
        }
        
        [UMethod]
        public void t_queryOption()
        {// TODO: Add Testing logic here
            var context = GetNewContext();
            DataServiceQuery<Order> selectedOrders = context.Orders
            .AddQueryOption("$filter", "Freight gt 30")
            .AddQueryOption("$orderby", "OrderID desc");
            foreach (Order order in selectedOrders)
            {
                printf("Order ID: {0} - Freight: {1}",order.OrderID, order.Freight);
            }
        }


        [UMethod]
        public void t_queryOption2()
        {// TODO: Add Testing logic here
            var context = GetNewContext();
            DataServiceQuery<Order> selectedOrders = context.Orders;
            
            foreach (Order order in selectedOrders)
            {
                printf("Order ID: {0} - Freight: {1}", order.OrderID, order.Freight);
            }
        }

        [UMethod]
        public void t_add()
        {// TODO: Add Testing logic here
            var context = GetNewContext();
            //CreateLog(ref context);
            // Create the new product.
            
            Product newProduct =Product.CreateProduct(0,  false);

            // Set property values.
            newProduct.ProductName = productName;
            newProduct.QuantityPerUnit = "120gm bags";
            newProduct.ReorderLevel = 5;
            newProduct.UnitPrice = 5.2M;

            try
            {
                // Add the new product to the Products entity set.
                context.AddToProducts(newProduct);

                // Send the insert to the data service.
                DataServiceResponse response = context.SaveChanges();

                // Enumerate the returned responses.
                foreach (ChangeOperationResponse change in response)
                {
                    // Get the descriptor for the entity.
                    EntityDescriptor descriptor = change.Descriptor as EntityDescriptor;

                    if (descriptor != null)
                    {
                        Product addedProduct = descriptor.Entity as Product;

                        if (addedProduct != null)
                        {
                            printf("New product added with ID {0}.",addedProduct.ProductID);
                        }
                    }
                }
            }
            catch (DataServiceRequestException ex)
            {
                throw new ApplicationException(
                    "An error occurred when saving changes.", ex);
            }
            //context.UpdateObject();

        }
        [UMethod]
        public void t_addRelatedObject()
        {// TODO: Add Testing logic here
            var context = GetNewContext();
            var selectedProduct = (from prod in context.Products
                                where prod.ProductName == productName
                                select prod).Single();
            string customerId = "ALFKI";
            var cust = (from customer in context.Customers.Expand("Orders")
                        where customer.CustomerID == customerId
                        select customer).Single();
            // Get the first order. 
            Order order = cust.Orders.FirstOrDefault();
            

            // Create a new order detail for the specific product.
            var newItem = Order_Detail.CreateOrder_Detail(order.OrderID, selectedProduct.ProductID, 10, 5, 0);
            // Add the new item with a link to the related order.
            context.AddRelatedObject(order, "Order_Details", newItem);//**須這才會真存到DB***
            // Add the new order detail to the collection, and
            // set the reference to the product.

            //****以下可有可無,只在讓記憶體的資料與DB上一致,不用重新query一次
            order.Order_Details.Add(newItem);
            newItem.Order = order;
            newItem.Product = selectedProduct;
            // Send the changes to the data service.


            DataServiceResponse response = context.SaveChanges();
        }

        [UMethod]
        public void t_update()
        {// TODO: Add Testing logic here
            var context = GetNewContext();
            var prodToChange = (from prod in context.Products
                                where prod.ProductName == productName
                                    select prod).Single();
            prodToChange.QuantityPerUnit= "*******";
            context.UpdateObject(prodToChange);
            context.SaveChanges();

        }
        [UMethod]
        public void t_delete()
        {// TODO: Add Testing logic here
            var context = GetNewContext();           


            var prodToChange = (from prod in context.Products.Expand("Order_Details")
                                where prod.ProductName == productName
                                select prod).Single();
            
            
            var detailToChange = prodToChange.Order_Details.First();

            prodToChange.Order_Details.Remove(detailToChange);

            //var detailToChange = (from detail in context.Order_Details
            //                      where detail.ProductID == prodToChange.ProductID
            //                      select detail).Single();
            //context.DeleteObject(detailToChange);
            
            
            context.DeleteObject(detailToChange);            
            context.DeleteObject(prodToChange);
            context.SaveChanges();

        }

        void prodToChange_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
                //throw new NotImplementedException();
        }
        private void CreateLog(ref UserMgrDataServiceContext context)
        {            
            //var log = new ChangedLog();
            //log.Sponsor = "Gensys";
            //context.AddToChangedLogs(log);
        }
        private string productName = "*TestProduct*";
        private Uri svcUri = new Uri("http://localhost:1570/DataService.svc/");
    }
}
