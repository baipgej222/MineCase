﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MineCase.Gateway.Network
{
    class ConnectionRouter
    {
        private readonly TcpListener _listener;

        public ConnectionRouter()
        {
            _listener = new TcpListener(new IPEndPoint(IPAddress.Any, 25565));
        }

        public async Task Startup(CancellationToken cancellationToken)
        {
            _listener.Start();
            while (!cancellationToken.IsCancellationRequested)
            {
                DispatchIncomingClient(await _listener.AcceptTcpClientAsync(), cancellationToken);
            }
            _listener.Stop();
        }

        private async void DispatchIncomingClient(TcpClient tcpClient, CancellationToken cancellationToken)
        {
            try
            {
                using (var session = new ClientSession(tcpClient))
                {
                    await session.Startup(cancellationToken);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}