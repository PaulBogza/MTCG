using System;
using Npgsql;


namespace DB.Connection{
    public class Connection{
        public void CreateConnection(){
            var connString = "Host=127.0.0.1;Port=5432;Database=mydb;Username=postgres;Password=postgres;Persist Security Info=True";
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            Console.WriteLine("Opened");
                        

            using (var cmd = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS test (id INT PRIMARY KEY);", conn))
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine("Executed");
            }

            conn.Close(); 
        }
    }
}
