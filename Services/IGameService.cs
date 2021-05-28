using ApiCatalogoJogos.InputModel;
using ApiCatalogoJogos.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Services
{
    public interface IGameService : IDisposable
    {
        Task<List<GameViewModel>> FindGame(int page, int quantity);
        Task<GameViewModel> FindGamePerId(Guid id);
        Task<GameViewModel> InsertGame(GameInputModel game);
        Task UpdateGame(Guid id, GameInputModel game);
        Task UpdateGame(Guid id, double price);
        Task RemoveGame(Guid id);
    }
}
