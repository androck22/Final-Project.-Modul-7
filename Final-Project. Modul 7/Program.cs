using System;
using System.Collections.Generic;

namespace Доставка_товаров
{
    class Program
    {
        static void Main(string[] args)
        {
            Courier cour1 = new Courier() { Name = "Сергей", NumberOfTelephone = "11231" };
            Courier cour2 = new Courier() { Name = "Игорь", NumberOfTelephone = "2342341" };
            Courier cour3 = new Courier() { Name = "Петр", NumberOfTelephone = "3453451" };

            Product prod1 = new Product() { Price = 10, Name = "Магнитофон", Size = "Big" };
            Product prod2 = new Product() { Price = -1, Name = "Пылесос", Size = "Medium" };
            Product prod3 = new Product() { Price = 20, Name = "Плеер", Size = "Small" };

            List<Order<Delivery>> orders = new List<Order<Delivery>>();
            Order<Delivery> order1 = new Order<Delivery>() { Delivery = new HomeDelivery("Петров И.И.", "Грушевская,20", cour1, new DateTime(2021, 1, 21, 8, 0, 0)), Description = "Позвонить за полчаса", Number = 1, product = prod1 };
            Order<Delivery> order2 = new Order<Delivery>() { Delivery = new PickPointDelivery("Уманская, 9", cour2, new DateTime(2021, 1, 19, 9, 0, 0), new DateTime(2021, 1, 21, 10, 0, 0), "Иванов П.П."), Description = "По гарантии", Number = 2, product = prod2 };
            Order<Delivery> order3 = new Order<Delivery>() { Delivery = new ShopDelivery("Михаловская, 2", cour3, new DateTime(2021, 1, 21, 11, 0, 0), "21 vek"), Description = "Плеер", Number = 3, product = prod3 };
            orders.Add(order1);
            orders.Add(order2);
            orders.Add(order3);

            CheckOrderStatus.CheckTimeBeforeOrderTime(orders);
            CheckOrderStatus.CheckBigSizeOfProduct(orders);
            Console.ReadLine();
        }


        #region Delivery
        public abstract class Delivery
        {
            public string Address;
            public Courier Courier;
            public DateTime ArrivedTime; //время прибытия курьера до точки доставки

            public Delivery(string address, Courier courier, DateTime time)
            {
                Address = address;
                Courier = courier;
                ArrivedTime = time;
            }

            public abstract void OrderStatus();
        }
        class HomeDelivery : Delivery
        {
            public string NameOfClient;


            public HomeDelivery(string name, string address, Courier courier, DateTime time) : base(address, courier, time)
            {
                NameOfClient = name;
            }

            public override void OrderStatus()
            {
                Console.WriteLine("Товар доставлен курьером " + Courier.Name + " по адресу " + Address + " в " + ArrivedTime.ToShortTimeString());
            }
        }
        class PickPointDelivery : Delivery
        {
            public DateTime TimeOfGetting;
            public string NameOfClient;

            public PickPointDelivery(string address, Courier courier, DateTime time, DateTime timeOfgetting, string nameOfClient) : base(address, courier, time)
            {
                TimeOfGetting = timeOfgetting;
                NameOfClient = nameOfClient;
            }

            public override void OrderStatus()
            {
                Console.WriteLine("Заказ доставлен на склад по адресу" + Address + " курьером " + Courier.Name + " в " + ArrivedTime.ToShortTimeString() +
                " . Заказ получен клиентом " + NameOfClient + "в " + TimeOfGetting.ToShortTimeString());
            }
        }
        class ShopDelivery : Delivery
        {
            public string ShopName;

            public ShopDelivery(string address, Courier courier, DateTime time, string shopName) : base(address, courier, time)
            {
                ShopName = shopName;
            }
            public override void OrderStatus()
            {
                Console.WriteLine("Доставлено в магазин + " + ShopName + " курьером " + Courier.Name + " в" + ArrivedTime.ToShortTimeString());
            }
        }
        #endregion 
        public class Order<TDelivery> where TDelivery : Delivery
        {
            public TDelivery Delivery;

            public int Number;

            public string Description;


            public void DisplayAddress()
            {
                Console.WriteLine(Delivery.Address);
            }

            public Product product;
        }

        public class Product
        {
            private double price;
            public string Name { get; set; }

            public string Size { get; set; }

            public double Price
            {
                set
                {
                    if (value < 0)
                    {
                        price = 1;
                    }
                    else
                    {
                        price = value;
                    }
                }

                get { return price; }
            }
        }

        public class Courier
        {
            private string numberOfTelephone;
            public string Name { get; set; }
            public string NumberOfTelephone
            {
                set
                {
                    if (value.Length != 7)
                    {
                        numberOfTelephone = "1111111";
                    }
                    else
                    {
                        numberOfTelephone = value;
                    }
                }
                get
                { return numberOfTelephone; }
            }
        }

        public static class CheckOrderStatus
        {
            public static void CheckTimeBeforeOrderTime(List<Order<Delivery>> orders)
            {
                foreach (Order<Delivery> order in orders)
                {
                    if (order.Delivery.ArrivedTime <= DateTime.Now)
                    {
                        Console.WriteLine("Заказ №" + order.Number + " должен быть выполнен уже");
                    }
                    else
                    {
                        Console.WriteLine("До времения выпопнения заказа №" + order.Number + " осталось " + order.Delivery.ArrivedTime.Subtract(DateTime.Now));
                    }
                }
            }

            public static void CheckBigSizeOfProduct(List<Order<Delivery>> orders)
            {
                foreach (Order<Delivery> order in orders)
                {
                    if (order.product.Size == "Big")
                    {
                        Console.WriteLine("В заказе №" + order.Number + " имееются габаритные товары");
                    }
                }
            }
        }
    }
}

