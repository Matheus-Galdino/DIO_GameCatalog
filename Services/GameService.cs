using APIGamesCatalog.Exceptions;
using APIGamesCatalog.Models;
using APIGamesCatalog.Repositories;
using APIGamesCatalog.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIGamesCatalog.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;

        public GameService(IGameRepository gameRepository) => _gameRepository = gameRepository;

        public async Task<List<Game>> GetAllAsync(int page, int amount) => await _gameRepository.GetAllAsync(page, amount);

        public async Task<Game> GetByIdAsync(Guid id) => await _gameRepository.GetByIdAsync(id);

        public async Task<Game> InsertASync(GameViewModel model)
        {
            var addedGame = await _gameRepository.GetByNameAndProducerAsync(model.Name, model.Producer);

            if (addedGame != null)
                throw new GameAlreadyAddedException();

            var game = new Game
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Price = model.Price,
                Producer = model.Producer,
            };

            await _gameRepository.InsertASync(game);

            return game;
        }

        public async Task RemoveAsync(Guid id)
        {
            var addedGame = await _gameRepository.GetByIdAsync(id);

            if (addedGame is null)
                throw new GameNotAddedException();

            await _gameRepository.RemoveAsync(id);
        }

        public async Task UpdateAsync(Guid id, GameViewModel model)
        {
            var addedGame = await _gameRepository.GetByIdAsync(id);

            if (addedGame is null)
                throw new GameNotAddedException();

            addedGame.Name = model.Name;
            addedGame.Price = model.Price;
            addedGame.Producer = model.Producer;

            await _gameRepository.UpdateAsync(addedGame);
        }

        public async Task UpdatePriceAsync(Guid id, decimal price)
        {
            var addedGame = await _gameRepository.GetByIdAsync(id);

            if (addedGame is null)
                throw new GameNotAddedException();
            
            addedGame.Price = price;            

            await _gameRepository.UpdateAsync(addedGame);
        }

        public void Dispose() => _gameRepository?.Dispose();
    }
}
