using System.Linq;
using System.Threading.Tasks;

namespace TrainApp.Data.Interfaces
{
    public interface ITrainRepository
    {
        Task<Entities.Train> FindAsync(int trainId);
        Task<Entities.Train> UpdateAsync(Entities.Train train);
        Task<Entities.Train> AddAsync(Entities.Train train);
        Task RemoveAsync(int trainId);
        IQueryable<Entities.Train> Get();
    }
}
