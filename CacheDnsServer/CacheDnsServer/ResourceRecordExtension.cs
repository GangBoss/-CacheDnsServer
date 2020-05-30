using System;
using Makaretu.Dns;

namespace CacheDnsServer
{
    static class ResourceRecordExtension
    {
        public static DateTime GetExpareTime(this ResourceRecord rec)
        {
            return rec.CreationTime.Add(rec.TTL);
        }
    }
}