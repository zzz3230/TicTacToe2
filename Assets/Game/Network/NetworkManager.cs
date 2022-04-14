using RiptideNetworking;
using RiptideNetworking.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;

internal enum MessageId : ushort
{
    spawnPlayer = 1,
    playerMovement
}

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _singleton;
    public static NetworkManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(NetworkManager)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }

    [SerializeField] private ushort port;
    [SerializeField] private ushort maxPlayers;

    internal Server Server { get; private set; }
    internal Client Client { get; private set; }


    public Action<string> ipCheckingFailed;
    public Action<string> ipCheckingSucceeded;

    List<string> ipToCheck = new List<string>();
    public LanManager lanManager;


    private void Awake()
    {
        Singleton = this;
        lanManager = new LanManager(port);
    }

    private void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
        
        Server = new Server { AllowAutoMessageRelay = true };

        Client = new Client(timeoutTime: 500);
        Client.Connected += DidConnect;
        Client.ConnectionFailed += FailedToConnect;
        Client.ClientConnected += PlayerJoined;
        Client.ClientDisconnected += PlayerLeft;
        Client.Disconnected += DidDisconnect;

    }

    private void FixedUpdate()
    {
        if (Server.IsRunning)
            Server.Tick();

        Client.Tick();
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
        DisconnectClient();
    }

    internal void StartHost()
    {
        Server.Start(port, maxPlayers);
        Client.Connect($"127.0.0.1:{port}");
    }

    internal void JoinGame(string ipString)
    {
        //ipToCheck = Utils.GetAllLocalIPv4(System.Net.NetworkInformation.NetworkInterfaceType.Ethernet).ToList();
        lanManager.SendRangePing();
        //Debug.Log(string.Join(';', ipToCheck));

        
        //Client.Connect($"{ipToCheck[0]}:{port}");
        //ipToCheck = ipToCheck.Skip(1).ToList();

        /*foreach (var adr in Utils.GetAllLocalIPv4(System.Net.NetworkInformation.NetworkInterfaceType.Ethernet))
    {
        if(adr != "127.0.0.1")
            Client.Connect($"{adr}:{port}");
    }*/

    }

    internal void LeaveGame()
    {
        Server.Stop();
        DisconnectClient();
    }

    private void DisconnectClient()
    {
        Client.Disconnect();
    }

    private void DidConnect(object sender, EventArgs e)
    {
        print($"did connected to {ipToCheck[0]}");
        Client.Disconnect();
        ipCheckingSucceeded(ipToCheck[0]);

        ipToCheck = ipToCheck.Skip(1).ToList();

        Client.Connect($"{ipToCheck[0]}:{port}");
        
    }

    private void FailedToConnect(object sender, EventArgs e)
    {
        print($"failed connected to {ipToCheck[0]}"); 
        ipCheckingFailed(ipToCheck[0]);

        ipToCheck = ipToCheck.Skip(1).ToList();
        
        Client.Connect($"{ipToCheck[0]}:{port}");
        
    }

    private void PlayerJoined(object sender, ClientConnectedEventArgs e)
    {
        print("joined");
    }

    private void PlayerLeft(object sender, ClientDisconnectedEventArgs e)
    {
        print("left");
    }

    private void DidDisconnect(object sender, EventArgs e)
    {
        print("did disconnected");
    }

    internal static void OnAddressFound(IPAddress address)
    {
        print(address);
    }
}