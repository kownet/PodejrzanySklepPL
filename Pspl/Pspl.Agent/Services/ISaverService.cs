using Pspl.Shared.Models;
using Pspl.Shared.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pspl.Agent.Services
{
    public interface ISaverService
    {
        Task SaveAll(IEnumerable<Ad> ads);
        Task<IEnumerable<Ad>> GetAllAsync();
        Task ClearAll(IEnumerable<Ad> ads);
        Task DeleteAll();
    }

    public class SaverService : ISaverService
    {
        private readonly IAdRepository _adRepository;

        public SaverService(IAdRepository adRepository)
        {
            _adRepository = adRepository;
        }

        public async Task ClearAll(IEnumerable<Ad> ads)
        {
            foreach (var ad in ads)
            {
                await _adRepository.DeleteAsync(ad.Name);
            }
        }

        public async Task DeleteAll()
        {
            await _adRepository.DeleteAllAsync();
        }

        public async Task<IEnumerable<Ad>> GetAllAsync()
            => await _adRepository.GetAllAsync();

        public async Task SaveAll(IEnumerable<Ad> ads)
        {
            foreach (var ad in ads)
            {
                await _adRepository.CreateAsync(ad);
            }
        }
    }
}