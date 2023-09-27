using System;
using System.Linq;

namespace ecommerceEF
{
    internal class Program
    {

        static void CheckAdmin()
        {
            var ctx = new ordersEntities();
            foreach (var login in ctx.logins)
            {
                if (ctx.logins.Count() == 0)
                {
                    var newAd = new login()
                    {
                        username = "admin",
                        password = "admin"
                    };
                    ctx.logins.Add(newAd);
                    ctx.SaveChanges();
                }
            }
        }

        static bool Login(string user, string pw)
        {
            var ctx = new ordersEntities();
            var utente = ctx.logins.FirstOrDefault(u => u.username == user);

            if (utente != null)
            {
                if (utente.password == pw)
                {
                    return true;
                }
            }
            return false;
        }

        static void Register(string user, string pw)
        {
            var ctx = new ordersEntities();
            var newL = new login()
            {
                username = user,
                password = pw
            };
            ctx.logins.Add(newL);
            ctx.SaveChanges();
        }

        static void query1()
        {
            var ctx = new ordersEntities();
            foreach (order order in ctx.orders)
            {
                int prezzoTot = 0;
                Console.WriteLine($"\nId : {order.orderid}  - Data : {order.orderdate} - Customer {order.customer}");
                foreach (orderitem oi in ctx.orderitems)
                {
                    if (order.orderid == oi.orderid)
                    {
                        Console.WriteLine($" - Item : {oi.item} - Prezzo {oi.price}");
                        prezzoTot = prezzoTot + oi.price;

                    }
                }
                Console.WriteLine($"------------- Prezzo Totale : {prezzoTot}  -----------------\n");
            }
        }
        static void query2(int ID)
        {
            var ctx = new ordersEntities();
            foreach (order order in ctx.orders)
            {
                if (order.orderid == ID)
                {
                    Console.WriteLine($"\nId : {order.orderid}  - Data : {order.orderdate} - Customer {order.customer}");
                    foreach (orderitem oi in ctx.orderitems)
                    {
                        if (order.orderid == oi.orderid)
                        {
                            Console.WriteLine($" - Item : {oi.item} - Prezzo {oi.price} -Quantità {oi.qty}");
                        }
                    }
                }
            }
        }

        //static void query3(int ID, string cliente, DateTime dataOrdine, )






        static void Main(string[] args)
        {
            CheckAdmin();

            while (true)
            {
                Console.WriteLine("Benvenuto! scegli LOGIN oppure REGISTER");
                Console.ForegroundColor = ConsoleColor.Cyan;
                string choice = Console.ReadLine();
                Console.ResetColor();
                Console.Clear();

                if (choice.ToLower().Equals("register"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("-----  REGISTER  -----");
                    Console.ResetColor();
                    Console.Write("Username: ");
                    string user = Console.ReadLine();
                    Console.Write("Password: ");
                    string pass = Console.ReadLine();
                    Register(user, pass);

                    break;

                }
                else if (choice.ToLower().Equals("login"))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("-----  LOGIN  -----");
                    Console.ResetColor();
                    Console.Write("Username: ");
                    string user = Console.ReadLine();
                    Console.Write("Password: ");
                    string pass = Console.ReadLine();

                    if (Login(user, pass))
                    {
                        Console.WriteLine("Accesso riuscito.");
                        Console.Clear();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Accesso non riuscito. Verifica le tue credenziali.\n");
                    }
                }
            }
            Console.Clear();
            Console.WriteLine("Accesso Riuscito!\n");
            bool isRunning = true;
            while (isRunning)
            {
                Console.WriteLine("\n(1) Visualizza tutti gli ordini");
                Console.WriteLine("(2) Visualizza in dettaglio un ordine");
                Console.WriteLine("(3) Crea un ordine");
                Console.WriteLine("(4) Esci");
                Console.Write("\nScegli una di queste opzioni: ");
                string scelta = Console.ReadLine();

                switch (scelta)
                {
                    case "1":
                        query1();
                        break;

                    case "2":
                        Console.Write("Inserisci l'ID dell'ordine per avere tutti i dettagli: ");
                        int ID = Convert.ToInt32(Console.ReadLine());
                        query2(ID);

                        break;

                    case "3":
                        break;

                    case "4":

                        isRunning = false;
                        break;

                    default:
                        Console.WriteLine("hai sbagliato numero!");
                        break;
                }
            }
            Console.Clear();
            Console.WriteLine("Chiusura del programma...");

        }
    }
}
