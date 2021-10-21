using System;
using System.Threading.Tasks;
using APIGamesCatalog.Models;
using APIGamesCatalog.ViewModels;
using System.Collections.Generic;

namespace APIGamesCatalog.Services
{
    public interface IGameService : IDisposable
    {
        Task<List<Game>> GetAllAsync(int page, int amount);
        Task<Game> GetByIdAsync(Guid id);
        Task<Game> InsertASync(GameViewModel model);
        Task UpdateAsync(Guid id, GameViewModel model);
        Task UpdatePriceAsync(Guid id, decimal price);
        Task RemoveAsync(Guid id);
    }
}
