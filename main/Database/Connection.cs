using System;
using Npgsql;

namespace DB.Connection{
    public class Connection{        
        public async void TryConnection(){
            var connectionString = "Host=host.docker.internal:5432;Username=user1;Password=user1pw;Database=mydb";
            await using var dataSource = NpgsqlDataSource.Create(connectionString);
        }
    }
}