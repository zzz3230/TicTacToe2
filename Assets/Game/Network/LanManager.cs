using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

public class LanManager
{
    public List<string> _addresses { get; private set; }

    // Addresse that the ping will reach
    [HideInInspector] public string addressToBroadcast = "192.168.1.";

    public int timeout = 10;

    public bool _isSearching { get; private set; }
    public float _percentSearching { get; private set; }

    public int _portServer { get; private set; }

    public LanManager(int port)
    {
        _portServer = port;
        _addresses = new List<string>();
    }

    public IPAddress GetIpInLan()
    {
        foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (ni.OperationalStatus == OperationalStatus.Up)
            {
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip != null && ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            return ip.Address;
                        }

                    }
                }
            }

        }

        return null;
    }

    public IEnumerator SendRangePing(bool bClear = true)
    {
        if (bClear)
        {
            _addresses.Clear();
        }

        _isSearching = true;
        for (int i = 0; i < 255; i++)
        {
            string address = addressToBroadcast + i;

            SendPing(address);

            _percentSearching = (float)i / 255;

            yield return null;
        }
        _isSearching = false;
    }

    public void SendPing(string address)
    {
        System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();

        // 128 TTL and DontFragment
        PingOptions pingOption = new PingOptions(16, true);

        // Once the ping has reached his target (or has timed out), call this function
        ping.PingCompleted += ping_PingCompleted;

        byte[] buffer = Encoding.ASCII.GetBytes("ping");

        try
        {
            // Do not block the main thread
            //ping.SendAsync(address, timeout, buffer, pingOption, addressToBroadcast);
            PingReply reply = ping.Send(address, timeout, buffer, pingOption);

            Debug.Log("Ping Sent at " + address);

            displayReply(reply);
        }
        catch (PingException ex)
        {
            Debug.Log(string.Format("Connection Error: {0}", ex.Message));
        }
        catch (SocketException ex)
        {
            Debug.Log(string.Format("Connection Error: {0}", ex.Message));
        }
    }


    private void ping_PingCompleted(object sender, PingCompletedEventArgs e)
    {
        string address = (string)e.UserState;

        if (e.Cancelled)
        {
            Debug.Log("Ping Canceled!");
        }

        if (e.Error != null)
        {
            Debug.Log(e.Error);
        }

        displayReply(e.Reply);
    }

    private void displayReply(PingReply reply)
    {
        if (reply != null)
        {
            Debug.Log("Status: " + reply.Status);
            if (reply.Status == IPStatus.Success)
            {
                Debug.Log("Pong from " + reply.Address);
                _addresses.Add(reply.Address.ToString());

                NetworkManager.OnAddressFound(reply.Address);
            }
        }
        else
        {
            Debug.Log("No reply");
        }
    }
}