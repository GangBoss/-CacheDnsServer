using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using Makaretu.Dns;

namespace CacheDnsServer
{
    internal class DnsCache
    {
        private readonly ConcurrentDictionary<string, DomainInfo> DomainInformation =
            new ConcurrentDictionary<string, DomainInfo>();

        public void Serialize()
        {
            var stream = new FileStream("settings.txt", FileMode.OpenOrCreate);
            var sw = new StreamWriter(stream);
            var writer = new PresentationWriter(sw);
            foreach (var info in DomainInformation.Values)
            foreach (var rec in info.DomainRecords)
            {
                writer.WriteEndOfLine();
                rec.Write(writer);
            }

            stream.Position = 0;
            sw.Close();
        }

        public void Deserialize()
        {
            if (File.Exists("settings.txt"))
            {
                var stream = new FileStream("settings.txt", FileMode.Open);
                var sr = new StreamReader(stream);
                var reader = new PresentationReader(sr);
                var res = new ResourceRecord();
                while (res != null)
                {
                    res = reader.ReadResourceRecord();
                    if (res != null)
                        AddRecord(res);
                }

                sr.Close();
            }
        }

        public Message GetUnlistedMessagePart(Message msg)
        {
            var remove = new List<Question>();
            foreach (var question in msg.Questions)
                if (ContainsInfo(question.Name, question.Type, question.Class))
                    remove.Add(question);
            foreach (var question in remove)
                msg.Questions.Remove(question);

            return msg;
        }

        private bool ContainsInfo(DomainName n, DnsType type, DnsClass cls)
        {
            var name = n.ToString();
            return DomainInformation.ContainsKey(name) &&
                   DomainInformation[name].Contains(name, type, cls) &&
                   !DomainInformation[name].Get(name, type, cls).IsExpired();
        }

        public void AddInfo(Message msg)
        {
            foreach (var answer in msg.Answers) AddRecord(answer);
        }

        private void AddRecord(ResourceRecord record)
        {
            var name = record.Name.ToString();
            if (!ContainsInfo(record.Name, record.Type, record.Class))
            {
                if (!DomainInformation.ContainsKey(name)) DomainInformation[name] = new DomainInfo();
                DomainInformation[record.Name.ToString()].AddRecord(record);
            }
        }

        public async void DeleteExpiredAsync()
        {
            var remove = new List<string>();
            foreach (var info in DomainInformation) await info.Value.RemoveExpiredAsync();

            foreach (var info in remove)
                if (DomainInformation[info].RecordsCount == 0)
                    DomainInformation.TryRemove(info, out var value);
        }
    }
}