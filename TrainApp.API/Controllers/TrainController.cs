using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrainApp.API.Models;
using TrainApp.Core.Interfaces;
using TrainApp.Core.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrainApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainController : ControllerBase
    {
        private readonly ITrainService _trainService;

        public TrainController(ITrainService trainService)
        {
            _trainService = trainService;
        }
        [HttpGet("{trainId}")]
        [ActionName("GetTrainAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TrainModel>> GetTrainAsync(int trainId)
        {
            var train = await _trainService.GetTrainAsync(trainId);

            if (train is null)
            {
                return NotFound();
            }

            return Ok(train);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<TrainModel>>> GetTrainsAsync()
        {
            var trains = await _trainService.GetTrainsAsync();
            return Ok(trains);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<TrainModel>> CreateTrainAsync(CreateTrainModel createTrainModel)
        {
            var trainModel = new TrainModel
            {
                TrainName = createTrainModel.TrainName,
                Wagons = null
            };
            var createdTrain = await _trainService.CreateTrainAsync(trainModel);

            return CreatedAtAction(nameof(GetTrainAsync), new { TrainId = createdTrain.TrainId }, createdTrain);
        }

        [HttpPut("{trainId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateTrainAsync([FromRoute] int trainId, [FromBody] UpdateTrainModel updateTrainModel)
        {
            if (trainId != updateTrainModel.TrainId)
            {
                return BadRequest();
            }

            var train = await _trainService.GetTrainAsync(trainId);
            if (train is null)
            {
                return NotFound();
            }

            var trainModel = new TrainModel
            {
                TrainId = trainId,
                TrainName = updateTrainModel.TrainName,
            };

            await _trainService.UpdateTrainAsync(trainModel);

            return Ok();
        }

        [HttpDelete("{trainId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteTrainAsync(int trainId)
        {
            var train = await _trainService.GetTrainAsync(trainId);
            if (train is null)
            {
                return NotFound();
            }

            await _trainService.DeleteTrainAsync(trainId);
            return NoContent();
        }
    }
}
