using ApiCatalogoJogos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Repositories
{
    public interface IGameRepository : IDisposable
    {
        Task<List<Game>> FindGame(int page, int quantity);
        Task<Game> FindGamePerId(Guid id);
        Task<List<Game>> FindGamePer(string name, string producer);
        Task InsertGame(Game game);
        Task UpdateGame(Game game);
        Task RemoveGame(Guid id);
    }
}
