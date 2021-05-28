using ApiCatalogoJogos.Entities;
using ApiCatalogoJogos.Exceptions;
using ApiCatalogoJogos.InputModel;
using ApiCatalogoJogos.Repositories;
using ApiCatalogoJogos.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Services
{
    public class GameService: IGameService
    {
        private readonly IGameRepository _gameRepository;

        public GameService(IGameRepository repository)
        {
            this._gameRepository = repository;
        }

        public void Dispose()
        {
            _gameRepository?.Dispose();
        }

        public async Task<List<GameViewModel>> FindGame(int page, int quantity)
        {
            var games = await _gameRepository.FindGame(page, quantity);

            return games.Select(game => new GameViewModel
            {
                Id = game.Id,
                Name = game.Name,
                Producer = game.Producer,
                Price = game.Price
            })
                .ToList();
        }

        public async Task<GameViewModel> FindGamePerId(Guid id)
        {
            var game = await _gameRepository.FindGamePerId(id);

            if (game == null)
                return null;

            return new GameViewModel
            {
                Id = game.Id,
                Name = game.Name,
                Producer = game.Producer,
                Price = game.Price
            };
        }

        public async Task<GameViewModel> InsertGame(GameInputModel game)
        {
            var entityGame = await _gameRepository.FindGamePer(game.Name, game.Producer);

            if (entityGame.Count > 0)
                throw new PreviouslyRegistredGame();

            var gameInsert = new Game
            {
                Id = Guid.NewGuid(),
                Name = game.Name,
                Producer = game.Producer,
                Price = game.Price
            };

            await _gameRepository.InsertGame(gameInsert);

            return new GameViewModel
            {
                Id = gameInsert.Id,
                Name = game.Name,
                Producer = game.Producer,
                Price = game.Price
            };
        }

        public async Task RemoveGame(Guid id)
        {
            var game = await _gameRepository.FindGamePerId(id);

            if(game == null)
                throw new GameNotRegistredException();

            await _gameRepository.RemoveGame(id);
        }

        public async Task UpdateGame(Guid id, GameInputModel game)
        {
            var entityGame = await _gameRepository.FindGamePerId(id);

            if (entityGame == null)
                throw new GameNotRegistredException();

            entityGame.Name = game.Name;
            entityGame.Producer = game.Producer;
            entityGame.Price = game.Price;

            await _gameRepository.UpdateGame(entityGame);
        }

        public async Task UpdateGame(Guid id, double price)
        {
            var entityGame = await _gameRepository.FindGamePerId(id);

            if (entityGame == null)
                throw new GameNotRegistredException();

            entityGame.Price = price;

            await _gameRepository.UpdateGame(entityGame);
        }
    }
}
