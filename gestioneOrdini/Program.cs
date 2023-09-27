using System;
using System.Data.SqlClient;
using System.Data;



namespace gestioneOrdini
{
    internal class Program
    {
        static void CheckAdmin(SqlConnection con)
        {
            string q = "select count(*) from login";
            var cmd = new SqlCommand(q, con);
            if ((int)cmd.ExecuteScalar() == 0)
                new SqlCommand("insert into login (username,password) VALUES ('admin','admin')").ExecuteNonQuery();
        }

        static bool Login(SqlConnection con, string user, string pw)
        {
            bool accessoRiuscito = false;
            SqlCommand cmd = new SqlCommand("select count(*) from login where username=@username and password=@password", con);
            cmd.Parameters.Add(new SqlParameter("@username", user));
            cmd.Parameters.Add(new SqlParameter("@password", pw));
            int count = (int)cmd.ExecuteScalar();

            if (count > 0)
            {
                // L'utente esiste nel database, quindi l'accesso è riuscito
                accessoRiuscito = true;
            }
            return accessoRiuscito;
        }

        static void Register(SqlConnection con, string user, string pw)
        {
            SqlCommand cmd = new SqlCommand("insert into login (username,password) VALUES (@user,@pw)", con);
            cmd.Parameters.Add(new SqlParameter("@user", user));
            cmd.Parameters.Add(new SqlParameter("@pw", pw));
            cmd.ExecuteNonQuery();
        }

        static void query1(SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("select o.orderid, customer , orderdate ,sum(price) as TotSpeso  from orders as o join orderitems as oi  on o.orderid = oi.orderid group by customer, orderdate , o.orderid;", con);
            
            using (var orders = cmd.ExecuteReader())
            {
                while (orders.Read()) { Console.WriteLine("{0} | {1} | {2} | {3}", orders["orderid"], orders["customer"], orders["orderdate"], orders["TotSPeso"]); }
            }
        }

        static void query2(SqlConnection con, string ID)
        {
            SqlCommand cmd = new SqlCommand("select * from orders where orderid = @ID;", con);
            cmd.Parameters.Add(new SqlParameter("@ID", ID));
            using (var orders = cmd.ExecuteReader())
            {

                while (orders.Read()) { Console.WriteLine("{0} | {1} | {2}", orders["orderid"], orders["customer"], orders["orderdate"]); }
            }

            cmd = new SqlCommand("select * from orderitems where orderid = @ID;", con);
            cmd.Parameters.Add(new SqlParameter("@ID", ID));
            using (var orderitems = cmd.ExecuteReader())
            {
             
                while (orderitems.Read()) { Console.WriteLine("{0} | {1} | {2} | {3}", orderitems["orderid"], orderitems["item"], orderitems["qty"], orderitems["price"]); }
            }
        }



        static void Main(string[] args)
        {
            string connStr = @"Server=localhost;" + "initial catalog=orders;" + "User ID=sa; Password=password123";
            SqlConnection con = new SqlConnection(connStr);
            con.Open();
            CheckAdmin(con);
            
            
            
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
                    Register(con, user, pass);
                    Console.WriteLine($"Utente creato con successo!");
                    Console.Clear();
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

                    if (Login(con, user, pass))
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
                Console.WriteLine("(1) Visualizza tutti gli ordini");
                Console.WriteLine("(2) Visualizza in dettaglio un ordine");
                Console.WriteLine("(3) Crea un ordine");
                Console.WriteLine("(4) Esci");
                Console.Write("\nScegli una di queste opzioni: ");
                string scelta = Console.ReadLine();

                switch (scelta)
                {
                    case "1":
                        query1(con);
                        break;

                    case "2":
                        Console.Write("Inserisci l'ID dell'ordine per avere tutti i dettagli: ");
                        string ID = Console.ReadLine();
                        query2(con, ID);

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
