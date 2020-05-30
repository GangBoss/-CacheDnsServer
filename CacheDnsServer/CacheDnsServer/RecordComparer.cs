using System.Collections.Generic;
using Makaretu.Dns;

namespace CacheDnsServer
{
    class RecordComparer : IComparer<ResourceRecord>
    {
        public int Compare(ResourceRecord x, ResourceRecord y)
        {
            return x.GetExpareTime() < y.GetExpareTime()? -1:1;
        }
    }
}