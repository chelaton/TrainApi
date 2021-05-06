using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainApp.API.Models;
using TrainApp.Core.Interfaces;
using TrainApp.Core.Models;

namespace TrainApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WagonController : ControllerBase
    {
        private readonly IWagonService _wagonService;

        public WagonController(IWagonService wagonService)
        {
            _wagonService = wagonService;
        }

        [HttpGet("{wagonId}")]
        [ActionName("GetWagonAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WagonModel>> GetWagonAsync(int wagonId)
        {
            var wagon = await _wagonService.GetWagonAsync(wagonId);

            if (wagon is null)
            {
                return NotFound();
            }

            return Ok(wagon);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<WagonModel>>> GetWagonsAsync()
        {
            var wagons = await _wagonService.GetWagonsAsync();
            return Ok(wagons);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<WagonModel>> CreateWagonAsync(CreateWagonModel createWagonModel)
        {
            if (String.IsNullOrEmpty(createWagonModel.TrainId.ToString()))
            {
                return NotFound("Missing selected Train!");
            }
            var wagonModel = new WagonModel
            {
                NumberOfChairs = createWagonModel.NumberOfChairs,
                TrainId = createWagonModel.TrainId
            };

            var createdWagon = await _wagonService.CreateWagonAsync(wagonModel);

            return CreatedAtAction(nameof(GetWagonAsync), new { wagonId = createdWagon.WagonId }, createdWagon);
        }


        [HttpPut("{wagonId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateWagonAsync([FromRoute] int wagonId, [FromBody] UpdateWagonModel updateWagonModel)
        {
            if (wagonId != updateWagonModel.WagonId)
            {
                return BadRequest();
            }

            var wagon = await _wagonService.GetWagonAsync(wagonId);
            if (wagon is null)
            {
                return NotFound();
            }

            var wagonModel = new WagonModel
            {
                WagonId = wagonId,
                NumberOfChairs = updateWagonModel.NumberOfChairs,
                Chairs = updateWagonModel.Chairs
                      .Select(x => new ChairModel() { NearWindow = x.NearWindow, Number = x.Number, Reserved = x.Reserved })
                      .ToList()
            };

            await _wagonService.UpdateWagonAsync(wagonModel);

            return Ok();
        }

        [HttpDelete("{wagonId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteWagonAsync(int wagonId)
        {
            var wagon = await _wagonService.GetWagonAsync(wagonId);
            if (wagon is null)
            {
                return NotFound();
            }

            await _wagonService.DeleteWagonAsync(wagonId);
            return NoContent();
        }
    }
}
