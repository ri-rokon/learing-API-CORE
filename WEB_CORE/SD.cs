using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB_CORE
{
    public static class SD
    {
        public static readonly string APIBaseUrl = "https://localhost:44389/";
        public static readonly string NationalParkAPIPath = APIBaseUrl+ "api/nationalparks/";
        public static readonly string TrailAPIPath = APIBaseUrl+ "api/trails/";
        public static readonly string AccountAPIPath = APIBaseUrl + "api/Users/";

    }
}
