using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using Makaretu.Dns;

namespace CacheDnsServer
{
    internal class DnsServer
    {
        private readonly UdpClient Reciever;
        private readonly UdpClient Resender;
        private DnsCache cache;
        private bool IsRunning;

        public DnsServer(int port)
        {
            Reciever = new UdpClient(53);
            Resender = new UdpClient(port);
            cache = new DnsCache();
            var timer = new Timer(100);
            timer.Elapsed += (o, e) => cache.DeleteExpiredAsync();
            timer.Start();
        }

        public void Start()
        {
            if (IsRunning) return;
            IsRunning = true;
            cache.Deserialize();
            StartListen(Resender);
            StartListen(Reciever);
        }

        private async void StartListen(UdpClient client)
        {
            while (IsRunning)
            {
                var data = await client.ReceiveAsync();
                ResolveData(data);
            }
        }

        private void ResolveData(UdpReceiveResult data)
        {
            var stream = new MemoryStream(data.Buffer);
            var message = new Message();
            message.Read(stream);
            if (message.IsResponse)
                cache.AddInfo(message);
            else
                ResolveUnlisted(message);
        }

        private void ResolveUnlisted(Message message)
        {
            var msg = cache.GetUnlistedMessagePart(message);
            var stream = new MemoryStream();
            stream.Write(msg.ToByteArray());
            Resender.SendAsync(stream.ToArray(), (int) stream.Length, new IPEndPoint(IPAddress.Parse("8.8.8.8"), 53));
        }

        public void Stop()
        {
            if (!IsRunning)
                return;
            IsRunning = false;
            cache.Serialize();
            cache = new DnsCache();
        }
    }
}