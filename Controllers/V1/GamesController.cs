using ApiCatalogoJogos.Exceptions;
using ApiCatalogoJogos.InputModel;
using ApiCatalogoJogos.Services;
using ApiCatalogoJogos.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService) 
        {
            this._gameService = gameService;
        }

        /// <summary>
        /// Buscar todos os jogos de forma paginada
        /// </summary>
        /// <remarks>
        /// Não é possível retornar os jogos sem paginação
        /// </remarks>
        /// <param name="pagina">Indica qual página está sendo consultada. Mínimo 1</param>
        /// <param name="quantidade">Indica a quantidade de reistros por página. Mínimo 1 e máximo 50</param>
        /// <response code="200">Retorna a lista de jogos</response>
        /// <response code="204">Caso não haja jogos</response>   
        [HttpGet]
        public async Task<ActionResult<List<GameViewModel>>> FindGame([FromQuery, Range(1, int.MaxValue)] int page = 1,[FromQuery, Range(1, 50)] int quantity = 5) 
        {
            var games = await _gameService.FindGame(page, quantity);

            if (games.Count() == 0)
                return NoContent();

            return Ok(games);   
        }

        /// <summary>
        /// Buscar um jogo pelo seu Id
        /// </summary>
        /// <param name="idJogo">Id do jogo buscado</param>
        /// <response code="200">Retorna o jogo filtrado</response>
        /// <response code="204">Caso não haja jogo com este id</response> 
        [HttpGet("{idGame:guid}")]
        public async Task<ActionResult<GameViewModel>> FindGamePerId([FromRoute] Guid idGame)
        {
            try
            {
                var game = await _gameService.FindGamePerId(idGame);
                if (game == null)
                    return NoContent();

                return Ok(game);
            }
            catch (GameNotRegistredException ex)
            {
                return NotFound("This game doesn't exist");
            }
        }

        /// <summary>
        /// Inserir um jogo no catálogo
        /// </summary>
        /// <param name="jogoInputModel">Dados do jogo a ser inserido</param>
        /// <response code="200">Cao o jogo seja inserido com sucesso</response>
        /// <response code="422">Caso já exista um jogo com mesmo nome para a mesma produtora</response>   
        [HttpPost]
        public async Task<ActionResult<GameViewModel>> InsertGame([FromBody] GameInputModel gameInputModel) 
        {
            try
            {
                var game = await _gameService.InsertGame(gameInputModel);
                return Ok(game);
            }
            catch (PreviouslyRegistredGame ex) 
            {
                return UnprocessableEntity("There is already a game with that name that belongs to this producer");
            }
        }

        /// <summary>
        /// Atualizar um jogo no catálogo
        /// </summary>
        /// /// <param name="idJogo">Id do jogo a ser atualizado</param>
        /// <param name="jogoInputModel">Novos dados para atualizar o jogo indicado</param>
        /// <response code="200">Cao o jogo seja atualizado com sucesso</response>
        /// <response code="404">Caso não exista um jogo com este Id</response> 
        [HttpPut("{idGame:guid}")]
        public async Task<ActionResult> UpdateGame([FromRoute] Guid idGame, [FromBody]GameInputModel game) 
        {
            try
            {
                await _gameService.UpdateGame(idGame, game);
                return Ok();
            }
            catch (GameNotRegistredException ex)
            {
                return NotFound("This game doesn't exist");
            }
        }

        /// <summary>
        /// Atualizar o preço de um jogo
        /// </summary>
        /// /// <param name="idJogo">Id do jogo a ser atualizado</param>
        /// <param name="preco">Novo preço do jogo</param>
        /// <response code="200">Cao o preço seja atualizado com sucesso</response>
        /// <response code="404">Caso não exista um jogo com este Id</response>
        [HttpPatch("{idGame:guid}/price/{price:double}")]
        public async Task<ActionResult> UpdateGame([FromRoute] Guid idGame, [FromRoute] double price)
        {
            try
            {
                await _gameService.UpdateGame(idGame, price);
                return Ok();
            }
            catch (GameNotRegistredException ex)
            {
                return NotFound("This game doesn't exist");
            }
        }

        /// <summary>
        /// Excluir um jogo
        /// </summary>
        /// /// <param name="idJogo">Id do jogo a ser excluído</param>
        /// <response code="200">Cao o preço seja atualizado com sucesso</response>
        /// <response code="404">Caso não exista um jogo com este Id</response>   
        [HttpDelete("{idGame:guid}")]
        public async Task<ActionResult> RemoveGame( [FromRoute] Guid idGame) 
        {
            try
            {
                await _gameService.RemoveGame(idGame);
                return Ok();
            }
            catch (GameNotRegistredException ex)
            {
                return NotFound("This game doesn't exist");
            }
            
        }

    }
}
