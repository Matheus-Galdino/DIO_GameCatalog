using APIGamesCatalog.Exceptions;
using APIGamesCatalog.Models;
using APIGamesCatalog.Services;
using APIGamesCatalog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace APIGamesCatalog.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService service)
        {
            _gameService = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Game>>> GetGames([FromQuery, Range(1, int.MaxValue)] int page = 1, [FromQuery, Range(1, 50)] int amount = 5)
        {
            var games = await _gameService.GetAllAsync(page, amount);
            return Ok(games);
        }

        [HttpGet("{gameId:guid}")]
        public async Task<ActionResult<object>> GetGame([FromRoute] Guid gameId)
        {
            var game = await _gameService.GetByIdAsync(gameId);

            if (game is null)
                return NotFound();

            return Ok(game);
        }

        [HttpPost]
        public async Task<ActionResult<object>> CreateGame([FromBody] GameViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var game = await _gameService.InsertASync(model);
                return Created($"v1/games/{game.Id}", game);
            }
            catch (GameAlreadyAddedException e)           
            {
                return UnprocessableEntity(e.Message);                
            }            
        }

        [HttpPut("{gameId:guid}")]
        public async Task<ActionResult> UpdateGame([FromRoute] Guid gameId, [FromBody] GameViewModel model)
        {
            try
            {
                await _gameService.UpdateAsync(gameId, model);

                return Ok();
            }
            catch (GameNotAddedException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPatch("{gameId:guid}/price/{price:decimal}")]
        public async Task<ActionResult> GetGame([FromRoute] Guid gameId, [FromRoute] decimal price)
        {
            try
            {
                await _gameService.UpdatePriceAsync(gameId, price);

                return Ok();
            }
            catch (GameNotAddedException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{gameId:guid}")]
        public async Task<ActionResult> RemoveGame([FromRoute] Guid gameId)
        {
            try
            {
                await _gameService.RemoveAsync(gameId);

                return Ok();
            }
            catch (GameNotAddedException e)
            {
                return NotFound(e.Message);                
            }
        }
    }
}
