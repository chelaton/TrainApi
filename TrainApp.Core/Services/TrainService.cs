using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainApp.Core.Interfaces;
using TrainApp.Core.Models;
using TrainApp.Data.Interfaces;

namespace TrainApp.Core.Services
{
    public class TrainService : ITrainService
    {
        private readonly ITrainRepository _trainRepository;

        public TrainService(ITrainRepository trainRepository)
        {
            _trainRepository = trainRepository;
        }
        public async Task<TrainModel> CreateTrainAsync(TrainModel trainModel)
        {
            if (trainModel is null)
            {
                throw new ArgumentNullException(nameof(trainModel));
            }

            var trainEntity = new Data.Entities.Train
            {
                TrainName = trainModel.TrainName
            };

            trainEntity = await _trainRepository.AddAsync(trainEntity);

            return new TrainModel
            {
                TrainId= trainEntity.TrainId,
                TrainName = trainEntity.TrainName,
                //Wagons = trainEntity.Wagons
                //                    .Select(x => new WagonModel() { WagonId = x.WagonId })
                //                    .ToList()
            };
        }

        public async Task DeleteTrainAsync(int trainId)
        {
            await _trainRepository.RemoveAsync(trainId);
        }

        public async Task<TrainModel> GetTrainAsync(int trainId)
        {
            var trainEntity = await _trainRepository.FindAsync(trainId);

            if (trainEntity is null)
            {
                return null;
            }

            return new TrainModel
            {
                TrainId = trainEntity.TrainId,
                TrainName = trainEntity.TrainName,
                Wagons = trainEntity.Wagons
                      .Select(x => new WagonModel() { WagonId = x.WagonId, WagonPosition= x.WagonPosition, NumberOfChairs=x.Chairs.Count, 
                          Chairs=x.Chairs.Select(x => new ChairModel() { ChairId=x.ChairId, WagonId = x.WagonId,  NearWindow=x.NearWindow, Number=x.Number, Reserved=x.Reserved})
                                .ToList()
                      })
                      .ToList()
            };
        }

        public async Task<List<TrainModel>> GetTrainsAsync()
        {
            IQueryable<Data.Entities.Train> query = _trainRepository.Get();
            return await query.Select(trainEntity => new TrainModel
            {
                TrainId = trainEntity.TrainId,
                TrainName= trainEntity.TrainName,
                Wagons = trainEntity.Wagons
                      .Select(x => new WagonModel() { WagonId = x.WagonId, WagonPosition=x.WagonPosition, NumberOfChairs=x.Chairs.Count })
                      .ToList()
            })
            .ToListAsync();
        }

        public async Task<TrainModel> UpdateTrainAsync(TrainModel trainModel)
        {
            var trainEntity = new Data.Entities.Train
            {
                TrainId = trainModel.TrainId,
                TrainName = trainModel.TrainName,
                //Wagons = trainModel.Wagons
                //      .Select(x => new Data.Entities.Wagon() { WagonId = x.WagonId, WagonPosition=x.WagonPosition })
                //      .ToList()
            };

            trainEntity = await _trainRepository.UpdateAsync(trainEntity);

            return new TrainModel
            {
                TrainId = trainEntity.TrainId,
                TrainName= trainEntity.TrainName,
                Wagons = trainEntity.Wagons
                      .Select(x => new WagonModel() { WagonId = x.WagonId, WagonPosition=x.WagonPosition })
                      .ToList()
            };
        }
    }
}
