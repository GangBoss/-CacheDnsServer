using System.Drawing;
using System.Windows.Forms;

namespace CacheDnsServer
{
    public partial class Form1 : Form
    {
        private readonly DnsServer server = new DnsServer(10000);

        public Form1()
        {
            var currentSize = new Size(ClientSize.Width, 30);
            var createServer = new Button();
            createServer.Location = new Point(0, 0);
            createServer.Size = currentSize;
            createServer.Text = "Start server";
            createServer.Click += (o, e) => { server.Start(); };

            var stopServer = new Button();
            stopServer.Location = new Point(createServer.Right, 0);
            stopServer.Size = currentSize;
            stopServer.Text = "Stop server";
            stopServer.Click += (o, e) => server.Stop();
            Controls.Add(createServer);
            Controls.Add(stopServer);

            InitializeComponent();
        }
    }
}