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

        [HttpGet]
        public async Task<ActionResult<List<GameViewModel>>> FindGame([FromQuery, Range(1, int.MaxValue)] int page = 1,[FromQuery, Range(1, 50)] int quantity = 5) 
        {
            var games = await _gameService.FindGame(page, quantity);

            if (games.Count() == 0)
                return NoContent();

            return Ok(games);   
        }

        [HttpGet("{idGame:guid}")]
        public async Task<ActionResult<GameViewModel>> FindGamePerId([FromRoute] Guid idGame)
        {
            var game = await _gameService.FindGamePerId(idGame);
            if (game == null)
                return NoContent();

            return Ok(game);
        }

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
