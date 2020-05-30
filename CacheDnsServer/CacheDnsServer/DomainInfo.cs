using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Makaretu.Dns;

namespace CacheDnsServer
{
    internal class DomainInfo
    {
        public List<ResourceRecord> DomainRecords { get; } = new List<ResourceRecord>();

        public int RecordsCount
        {
            get
            {
                lock (DomainRecords)
                {
                    return DomainRecords.Count;
                }
            }
        }

        public void AddRecord(ResourceRecord rec)
        {
            lock (DomainRecords)
            {
                DomainRecords.Add(rec);
            }
        }

        public Task RemoveExpiredAsync()
        {
            return Task.Run(() =>
            {
                lock (DomainRecords)
                {
                    var remove = new List<ResourceRecord>();
                    foreach (var record in DomainRecords.Where(record => record.IsExpired())) remove.Add(record);
                    foreach (var record in remove) DomainRecords.Remove(record);
                }
            });
        }

        public bool Contains(DomainName name, DnsType type, DnsClass cls)
        {
            lock (DomainRecords)
            {
                return
                    DomainRecords.First(r =>
                        r.Name == name
                        && r.Type == type
                        && r.Class == cls) != null;
            }
        }

        public ResourceRecord Get(DomainName name, DnsType type, DnsClass cls)
        {
            lock (DomainRecords)
            {
                return DomainRecords.First(r =>
                    r.Name == name
                    && r.Type == type
                    && r.Class == cls);
            }
        }
    }
}