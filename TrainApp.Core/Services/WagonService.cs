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
    public class WagonService : IWagonService
    {
        private readonly IWagonRepository _wagonRepository;

        public WagonService(IWagonRepository wagonRepository)
        {
            _wagonRepository = wagonRepository;
        }
        public async Task<WagonModel> CreateWagonAsync(WagonModel wagonModel)
        {
            if (wagonModel is null || String.IsNullOrEmpty(wagonModel.TrainId.ToString()))
            {
                throw new ArgumentNullException(nameof(wagonModel));
            }

            var chairs = new List<Data.Entities.Chair>();
            for (int i = 1; i < wagonModel.NumberOfChairs+1; i++)
            {
                chairs.Add(new Data.Entities.Chair { NearWindow = false, Number = i, Reserved = false });
            }

            var wagonEntity = new Data.Entities.Wagon
            {
                Chairs = chairs,
                TrainId=wagonModel.TrainId
            };

            wagonEntity = await _wagonRepository.AddAsync(wagonEntity);

            var listChairs = wagonEntity.Chairs
                      .Select(x => new ChairModel() { ChairId = x.ChairId, NearWindow = x.NearWindow, Number = x.Number, Reserved = x.Reserved, WagonId=x.WagonId })
                      .ToList();
            return new WagonModel
            {
                WagonId = wagonEntity.WagonId,
                NumberOfChairs = wagonEntity.Chairs.Count,
                TrainId = wagonEntity.TrainId,
                Chairs = listChairs
            };
        }

        public async Task DeleteWagonAsync(int wagonId)
        {
            await _wagonRepository.RemoveAsync(wagonId);
        }

        public async Task<WagonModel> GetWagonAsync(int wagonId)
        {
            var wagonEntity = await _wagonRepository.FindAsync(wagonId);

            if (wagonEntity is null)
            {
                return null;
            }

            return new WagonModel
            {
                WagonId = wagonEntity.WagonId,
                NumberOfChairs = wagonEntity.Chairs.Count,
                Chairs = wagonEntity.Chairs
                      .Select(x => new ChairModel() { ChairId=x.ChairId, NearWindow = x.NearWindow, Number = x.Number, Reserved = x.Reserved, WagonId = x.WagonId })
                      .ToList()
            };
        }

        public async Task<List<WagonModel>> GetWagonsAsync()
        {
            IQueryable<Data.Entities.Wagon> query = _wagonRepository.Get();
            return await query.Select(wagonEntity => new WagonModel
            {
                WagonId = wagonEntity.WagonId,
                NumberOfChairs = wagonEntity.Chairs.Count,
                Chairs = wagonEntity.Chairs
                      .Select(x => new ChairModel() { ChairId = x.ChairId, NearWindow = x.NearWindow, Number = x.Number, Reserved = x.Reserved, WagonId = x.WagonId })
                      .ToList()
            })
            .ToListAsync();
        }

        public async Task<WagonModel> UpdateWagonAsync(WagonModel wagonModel)
        {
            var wagonEntity = new Data.Entities.Wagon
            {
                WagonId = wagonModel.WagonId,
                Chairs = wagonModel.Chairs
                      .Select(x => new Data.Entities.Chair() { ChairId = x.ChairId, NearWindow = x.NearWindow, Number = x.Number, Reserved = x.Reserved, WagonId = x.WagonId })
                      .ToList()
            };

            wagonEntity = await _wagonRepository.UpdateAsync(wagonEntity);

            return new WagonModel
            {
                WagonId = wagonEntity.WagonId,
                NumberOfChairs = wagonEntity.Chairs.Count,
                Chairs = wagonEntity.Chairs
                      .Select(x => new ChairModel() { ChairId = x.ChairId, NearWindow = x.NearWindow, Number = x.Number, Reserved = x.Reserved, WagonId = x.WagonId })
                      .ToList()
            };
        }
    }
}
