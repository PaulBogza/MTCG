using System;
using System.Runtime.ConstrainedExecution;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Npgsql;
using Npgsql.Internal;

namespace DB.Connection{
    public class Connection{
        public void DBConnection(){
            var connstring = "Host=127.0.0.1;Port=5432;Username=admin;Password=admin;Database=mydb";
            using(var conn = new NpgsqlConnection(connstring)){
                try{
                    conn.Open();
                    System.Console.WriteLine("Connected");

                    conn.Close();
                }
                catch{
                    System.Console.WriteLine("Error");
                }   
            }
        }
    }
}