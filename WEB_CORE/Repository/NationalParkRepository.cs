using WEB_CORE.Models;
using WEB_CORE.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WEB_CORE.Repository
{
    public class NationalParkRepository : Repository<NationalPark>, INationalParkRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        public NationalParkRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }
}
