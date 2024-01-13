using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Store
{
    public class Clothes
    {
        internal int id;
        internal string clothes;
        internal int amount;
        internal float price;
        internal string sizes;
        
        internal Clothes(int id, string clothes, int amount, float price, string sizes)
        {
            this.id = id;
            this.clothes = clothes;
            this.amount = amount;
            this.price = price;
            this.sizes = sizes;
        }
    }

    public class Customer
    {
        internal string nameCustomer;
        internal string clothes;
        internal string size;
        internal int amount;
        internal long numReceipt;
        internal int idClothe;

        internal Customer(string nameCustomer, string clothes, string size, int amount, int idClothe)
        {
            this.nameCustomer = nameCustomer;
            this.clothes = clothes;
            this.size = size;
            this.amount = amount;
            Random random = new Random();
            this.numReceipt = random.Next(100000, 999999);
            this.idClothe = idClothe;
        }
    }

    public class ClothingStore
    {
        public string name;
        public List<Clothes> clothes;
        public List<Customer> customers;

        private void clothesStock()
        {
            List<Clothes> clothes1 = new List<Clothes> ();
            MySqlConnection connection = new MySqlConnection("user = root; database = TiendaDeRopa; server = localhost; password = reynaldo066512");
            connection.Open ();
            MySqlCommand cmd = new MySqlCommand("Select * from ropa", connection);
            MySqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                var id = 0;
                Int32.TryParse(reader["id"].ToString(), out id);
                var name = reader["nombre"].ToString();
                var amount = 0;
                Int32.TryParse(reader["cantidad"].ToString(), out amount );
                var price = (float)reader["precio"];
                string? sizes = reader["tallas"].ToString();
                Clothes clothe = new Clothes(id, name, amount, price, sizes);
                clothes1.Add(clothe);
            }
            connection.Close ();
            clothes = clothes1;
        }

        public ClothingStore(string name)
        {
            this.name = name;
            clothesStock();
        }

        public void Stock()
        {
            Console.WriteLine("Stock");
            clothesStock();
            foreach(var clothes in this.clothes)
            {
                Console.WriteLine($"clothes = {clothes.clothes}, price = {clothes.price}, sizes = {clothes.sizes}, amount = { clothes.amount}");
            }
        }

        private void Bill(string clothe, string size, int amount, int idClothe)
        {
            Console.WriteLine("can you give me your name?");
            string? name = Console.ReadLine();
            Customer customer = new Customer(name, clothe, size, amount, idClothe);
            MySqlConnection connection = new MySqlConnection("server = 127.0.0.1; user = root; database = TiendaDeRopa; password = reynaldo066512");
            connection.Open();
            MySqlCommand command = new MySqlCommand($"insert into VentaRecibo(nombreComprador, prenda, talla, cantidadPrendas, ropa, numRecibo) values ('{customer.nameCustomer}', '{customer.clothes}', '{customer.size}', {customer.amount}, {customer.idClothe}, {customer.numReceipt});", connection);
            command.ExecuteNonQuery();
            connection.Close();
        }

        private void sell(int idElement, int num)
        {
            MySqlConnection connection = new MySqlConnection("server = localhost; user = root; database = TiendaDeRopa; password = reynaldo066512");
            connection.Open();
            MySqlCommand command = new MySqlCommand("update ropa set cantidad = " + num + " where id = " + idElement, connection);
            command.ExecuteNonQuery();
            connection.Close();
            Console.WriteLine("thanks for the buy");
        }

        public void Purchase()
        {
            int i = 0;
            Stock();
            Console.WriteLine("what do you wanna buy?");
            string? answer = Console.ReadLine();
            while (i < clothes.Count)
            {
                if (clothes[i].clothes == answer)
                {
                    break;
                } 
                else if(i == clothes.Count - 1)
                {
                    Console.WriteLine("we dont have that");
                    Purchase();
                    break;
                }
                i++;
            }
            Console.WriteLine("of course, what size do you want?");
            string? size = Console.ReadLine().ToUpper();
            if(size == "small" || size == "S") 
            {
                size = "S";
            }
            else if (size == "medium" || size == "M")
            {
                size = "M";
            }
            else if (size == "large" || size == "L")
            {
                size = "L";
            }
            int u = 0;
            if (clothes[i].sizes.Contains(size) == false) 
            {
                Console.WriteLine("we dont have that size");
                Purchase();
            }
            Console.WriteLine("ok, thats alright, how many are you gonna buy?");
            int amount = 0;
            Int32.TryParse(Console.ReadLine(), out amount);
            if(amount <= clothes[i].amount)
            {
                var cost = amount * clothes[i].price;
                Console.WriteLine("ok, the bill make " + cost + ", thats ok? Y/yes - N/no");
                string? buy = Console.ReadLine().ToString().ToUpper();
                if(buy == "Y")
                {
                    int num = clothes[i].amount - amount;
                    Bill(clothes[i].clothes, size, amount, clothes[i].id);
                    sell(clothes[i].id, num);
                }
                else if(buy == "N")
                {
                    Console.WriteLine("ok, no problem");
                }
            }
            else
            {
                Console.WriteLine("we dont have that amount sorry");
            }
        }

        private void Sales()
        {
            MySqlConnection connection = new MySqlConnection("server = localhost; user = root; database = TiendaDeRopa; password = reynaldo066512");
            connection.Open();
            MySqlCommand command = new MySqlCommand("select * from VentaRecibo;", connection);
            MySqlDataReader reader = command.ExecuteReader();
            if(reader.HasRows)
            {
                while(reader.Read())
                {
                    Console.WriteLine($"id = {reader["id"]}, name customer = {reader["nombreComprador"]}, clothes = {reader["prenda"]}, size = {reader["talla"]}, amount clothes = {reader["cantidadPrendas"]}, numBill = {reader["numRecibo"]}");
                }
            }
            else
            {
                Console.WriteLine("we still didnt sell anything");
            }
        }

        public void SalesFile()
        {
            Console.WriteLine("login with the administrator user");
            string? user = Console.ReadLine();
            Console.WriteLine("insert password");
            string? password = Console.ReadLine();

            MySqlConnection connection = new MySqlConnection("server = 127.0.0.1; user = root; database = TiendaDeRopa; password = reynaldo066512");
            connection.Open();
            MySqlCommand command = new MySqlCommand($"select * from Administrador where usuario = '{user}' and clave = '{password}'", connection);
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                Console.WriteLine("login complete");
                Sales();
            }
            else
            {
                Console.WriteLine("login wrong");
            }
            connection.Close();
        }
    }
}
