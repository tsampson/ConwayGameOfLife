using AutoMapper;
using ConwayGameOfLife.Api.Dtos;
using ConwayGameOfLife.Api.Validation;
using ConwayGameOfLife.Orchestration;
using Microsoft.AspNetCore.Mvc;

namespace ConwayGameOfLife.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GameOfLifeController(
    IMapper _mapper,
    IGameOfLifeManager manager,
    GridValidator _validator)
    : ControllerBase
{
    [HttpPost]
    [Route("UploadBoard")]
    public async Task<IActionResult> UploadBoardAsync([FromBody] List<List<byte>> gridList)
    {
        var validationResult = _validator.Validate(gridList);
        if (!validationResult.IsValid)
        {            
            return BadRequest(validationResult.Errors);

        }
        var grid = _mapper.Map<byte[,]>(gridList);
        var guid =  await manager.UploadBoardAsync(grid);

        return Ok(guid);
    }

    [HttpPost]
    [Route("GetNextState/{boardId}")]
    public async Task<IActionResult> GetNextStateAsync(Guid boardId)
    {
        var boardState = await manager.GetNextStateAsync(boardId);
        if (boardState == null)
        {
            return NotFound("Could not find the board with the given ID.");
        }
        return Ok(_mapper.Map<GameStateResponse>(boardState));        
    }

    [HttpPost]
    [Route("GetStatesAhead/{boardId}/{generation}")]
    public async Task<IActionResult> GetStatesAheadAsync(Guid boardId, int generation)
    {
        var boardState = await manager.GetGenerationAsync(boardId, generation);
        if (boardState == null)
        {
            return NotFound("Could not find the board with the given ID.");
        }
        return Ok(_mapper.Map<GameStateResponse>(boardState));
    }

    [HttpPost]
    [Route("FinalState/{boardId}")]
    public async Task<IActionResult> GetFinalStateAsync(Guid boardId)
    {
        var boardState = await manager.GetFinalStateAsync(boardId);
        if (boardState == null)
        {
            return NotFound("Could not find the board with the given ID.");
        }
        return Ok(_mapper.Map<GameStateResponse>(boardState));
    }
}
