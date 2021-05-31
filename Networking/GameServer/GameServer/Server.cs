using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace GameServer
{
    public class Server
    {
        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }
        public readonly static Dictionary<int, Client> clients = new();

        public delegate void PacketHandler(int _fromClient, Packet _packet);

        public static Dictionary<int, PacketHandler> packetHandlers;
        private static TcpListener _tcpListener;

        public static void Start(int _maxPlayers, int _port)
        {
            
            Console.WriteLine("Starting server...");
            MaxPlayers = _maxPlayers;
            InitializeServerData();
            Port = _port;

            _tcpListener = new TcpListener(IPAddress.Any, Port);
            _tcpListener.Start();
            _tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);
            
            Console.WriteLine($"Server started on {Port}.");
        }

        private static void TCPConnectCallback(IAsyncResult _result)
        {
            TcpClient _client = _tcpListener.EndAcceptTcpClient(_result);
            _tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);
            Console.WriteLine($"Incoming connection from {_client.Client.RemoteEndPoint}...");

            for (int i = 1; i <= MaxPlayers; i++)
            {
                if (clients[i].tcp.socket == null)
                {
                    clients[i].tcp.Connect(_client);
                    return;
                }
            }
            
            Console.WriteLine($"{_client.Client.RemoteEndPoint} failed to connect: Server full!");
        }

        private static void InitializeServerData()
        {
            for (int i = 1; i <= MaxPlayers; i++)
            {
                clients.Add(i, new Client(i));
            }

            packetHandlers = new Dictionary<int, PacketHandler>()
            {
                {(int) ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived}
            };
            Console.WriteLine("Initialized packets.");
        }
    }
}