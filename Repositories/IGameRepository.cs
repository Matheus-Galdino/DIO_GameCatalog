using APIGamesCatalog.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIGamesCatalog.Repositories
{
    public interface IGameRepository : IDisposable
    {
        Task<List<Game>> GetAllAsync(int page, int amount);
        Task<Game> GetByIdAsync(Guid id);
        Task<Game> GetByNameAndProducerAsync(string name, string producer);
        Task InsertASync(Game model);
        Task UpdateAsync(Game model);
        Task RemoveAsync(Guid id);
    }
}
