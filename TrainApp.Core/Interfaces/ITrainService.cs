using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainApp.Core.Models;

namespace TrainApp.Core.Interfaces
{
    public interface ITrainService
    {
        Task<TrainModel> CreateTrainAsync(TrainModel trainModel);
        Task<TrainModel> UpdateTrainAsync(TrainModel trainModel);
        Task<TrainModel> GetTrainAsync(int trainId);
        Task DeleteTrainAsync(int trainId);
        Task<List<TrainModel>> GetTrainsAsync();
    }
}
