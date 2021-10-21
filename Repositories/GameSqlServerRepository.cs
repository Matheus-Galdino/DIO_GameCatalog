using APIGamesCatalog.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace APIGamesCatalog.Repositories
{
    public class GameSqlServerRepository : IGameRepository
    {
        private readonly SqlConnection sqlConnection;

        public GameSqlServerRepository(IConfiguration configuration) => sqlConnection = new SqlConnection(configuration.GetConnectionString("Default"));

        public async Task<List<Game>> GetAllAsync(int page, int amount)
        {
            var games = new List<Game>();

            var command = $"select * from Games order by id offset {(page - 1) * amount} rows fetch next {amount} rows only";
            var sqlCmd = new SqlCommand(command, sqlConnection);

            await sqlConnection.OpenAsync();

            var sqlDataReader = await sqlCmd.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                games.Add(new Game
                {
                    Id = (Guid)sqlDataReader["Id"],
                    Name = (string)sqlDataReader["Name"],
                    Price = (decimal)sqlDataReader["Price"],
                    Producer = (string)sqlDataReader["Producer"],
                });
            }

            await sqlConnection.CloseAsync();

            return games;
        }

        public async Task<Game> GetByIdAsync(Guid id)
        {
            Game game = null;
            var command = $"select * from Games where Id = @id";
            var sqlCmd = new SqlCommand(command, sqlConnection);
            sqlCmd.Parameters.AddWithValue("@id", id);

            await sqlConnection.OpenAsync();

            var sqlDataReader = await sqlCmd.ExecuteReaderAsync();

            if (sqlDataReader.Read())
            {
                game = new Game
                {
                    Id = (Guid)sqlDataReader["Id"],
                    Name = (string)sqlDataReader["Name"],
                    Price = (decimal)sqlDataReader["Price"],
                    Producer = (string)sqlDataReader["Producer"],
                };
            }

            await sqlConnection.CloseAsync();

            return game;
        }

        public async Task<Game> GetByNameAndProducerAsync(string name, string producer)
        {
            Game game = null;
            var command = "select * from Games where Name = @name and Producer = @producer";
            var sqlCmd = new SqlCommand(command, sqlConnection);
            sqlCmd.Parameters.AddWithValue("@name", name);
            sqlCmd.Parameters.AddWithValue("@producer", producer);

            await sqlConnection.OpenAsync();
            var sqlDataReader = await sqlCmd.ExecuteReaderAsync();

            if (sqlDataReader.Read())
            {
                game = new Game
                {
                    Id = (Guid)sqlDataReader["Id"],
                    Name = (string)sqlDataReader["Name"],
                    Price = (decimal)sqlDataReader["Price"],
                    Producer = (string)sqlDataReader["Producer"],
                };
            }

            await sqlConnection.CloseAsync();

            return game;
        }

        public async Task InsertASync(Game model)
        {
            var cmd = "insert into Games (Id, Name, Producer, Price) values (@id, @name, @producer, @price)";
            var sqlCmd = new SqlCommand(cmd, sqlConnection);
            sqlCmd.Parameters.AddWithValue("@id", model.Id);
            sqlCmd.Parameters.AddWithValue("@name", model.Name);
            sqlCmd.Parameters.AddWithValue("@price", model.Price);
            sqlCmd.Parameters.AddWithValue("@producer", model.Producer);

            await sqlConnection.OpenAsync();
            await sqlCmd.ExecuteNonQueryAsync();
            await sqlConnection.CloseAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            var command = $"delete from Games where Id = @id";
            var sqlCmd = new SqlCommand(command, sqlConnection);
            sqlCmd.Parameters.AddWithValue("@id", id);

            await sqlConnection.OpenAsync();
            await sqlCmd.ExecuteNonQueryAsync();
            await sqlConnection.CloseAsync();
        }

        public async Task UpdateAsync(Game model)
        {
            var cmd = "update Games set Name = @name, Producer = @producer, Price = @price where Id = @id";
            var sqlCmd = new SqlCommand(cmd, sqlConnection);
            sqlCmd.Parameters.AddWithValue("@id", model.Id);
            sqlCmd.Parameters.AddWithValue("@name", model.Name);
            sqlCmd.Parameters.AddWithValue("@price", model.Price);
            sqlCmd.Parameters.AddWithValue("@producer", model.Producer);

            using (sqlConnection)
                await sqlCmd.ExecuteNonQueryAsync();
        }

        public void Dispose()
        {
            sqlConnection?.Close();
            sqlConnection?.Dispose();
        }
    }
}
