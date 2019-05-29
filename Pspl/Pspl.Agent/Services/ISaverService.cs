using Pspl.Shared.Models;
using Pspl.Shared.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pspl.Agent.Services
{
    public interface ISaverService
    {
        Task Save(IEnumerable<Ad> ads);
    }

    public class SaverService : ISaverService
    {
        private readonly IAdRepository _adRepository;

        public SaverService(IAdRepository adRepository)
        {
            _adRepository = adRepository;
        }

        public async Task Save(IEnumerable<Ad> ads)
        {
            foreach (var ad in ads)
            {
                await _adRepository.CreateAsync(ad);
            }
        }
    }
}