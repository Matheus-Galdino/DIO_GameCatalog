using APIGamesCatalog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGamesCatalog.Repositories
{
    public class GameRepository : IGameRepository
    {
        private static List<Game> games = new List<Game>
        {
            new Game { Id = Guid.NewGuid(), Name = "FIFA 22", Producer = "EA", Price = 150},
            new Game { Id = Guid.NewGuid(), Name = "GTA V", Producer = "Rockstar", Price = 100},
            new Game { Id = Guid.NewGuid(), Name = "Black Ops 2", Producer = "Treyarch", Price = 250},
            new Game { Id = Guid.NewGuid(), Name = "Devil May Cry 3", Producer = "Capcom", Price = 80},
            new Game { Id = Guid.NewGuid(), Name = "Street Fighter V", Producer = "Capcom", Price = 125},
        };

        public Task<List<Game>> GetAllAsync(int page, int amount) => Task.FromResult(games.Skip((page - 1) * amount).Take(amount).ToList());

        public Task<Game> GetByIdAsync(Guid id) => Task.FromResult(games.FirstOrDefault(x => x.Id == id));

        public Task<Game> GetByNameAndProducerAsync(string name, string producer) => Task.FromResult(games.FirstOrDefault(x => x.Name.Equals(name) && x.Producer.Equals(producer)));

        public Task InsertASync(Game model)
        {
            games.Add(model);
            return Task.CompletedTask;
        }

        public Task RemoveAsync(Guid id)
        {
            games.RemoveAt(games.FindIndex(x => x.Id == id));
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Game model)
        {
            var gameIndex = games.FindIndex(x => x.Id == model.Id);
            games[gameIndex] = model;

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            // close connection to db
        }
    }
}
