using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainApp.Data.Entities;
using TrainApp.Data.Interfaces;

namespace TrainApp.Data.Repositories
{
    public class TrainRepository : ITrainRepository
    {
        private readonly TrainContext _trainContext;
        public TrainRepository(TrainContext trainContext)
        {
            _trainContext = trainContext;
        }
        public async Task<Train> AddAsync(Train train)
        {
            _trainContext.Add(train);
            await _trainContext.SaveChangesAsync();
            return train;
        }

        public async Task<Train> FindAsync(int trainId)
        {
            return await _trainContext.Trains
                .Include(c => c.Wagons)
                .Include("Wagons.Chairs")
                .FirstOrDefaultAsync(x => x.TrainId == trainId);
        }

        public IQueryable<Train> Get()
        {
            return _trainContext.Trains.AsQueryable();
        }

        public async Task RemoveAsync(int trainId)
        {
            var train = await _trainContext.Trains.FindAsync(trainId);
            if (train is not null)
            {
                _trainContext.Trains.Remove(train);
                await _trainContext.SaveChangesAsync();
            }
        }

        public async Task<Train> UpdateAsync(Train train)
        {
            Train actualTrain = _trainContext.Trains.Include(x=>x.Wagons).FirstOrDefault(x => x.TrainId == train.TrainId);
            // Update parent

            if (actualTrain != null)
            {
                _trainContext.Entry(actualTrain).CurrentValues.SetValues(train);

                // Delete children
                foreach (var existingChild in actualTrain.Wagons.ToList())
                {
                    if (!train.Wagons.Any(c => c.WagonId == existingChild.WagonId))
                        _trainContext.Wagons.Remove(existingChild);
                }

                // Update and Insert children
                foreach (var childModel in train.Wagons)
                {
                    var existingChild = actualTrain.Wagons
                        .Where(c => c.WagonId == childModel.WagonId && c.WagonId != default)
                        .SingleOrDefault();

                    if (existingChild != null)
                        // Update child
                        _trainContext.Entry(existingChild).CurrentValues.SetValues(childModel);
                    else
                    {
                        // Insert child
                        var newChild = new Wagon
                        {
                            WagonId = childModel.WagonId,
                            Chairs  = childModel.Chairs,
                        };
                        actualTrain.Wagons.Add(newChild);
                    }
                }

                await _trainContext.SaveChangesAsync();
            }
            return train;
        }
    }
}
