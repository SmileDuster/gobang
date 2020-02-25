using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public static class Client
{
    public const string ConnectError = "eee";
    
    private const string LoginCode = "al";
    private const string RegisterCode = "ar";
    private const string GuestCode = "ag";
    private const string MatchCode = "m";
    private const string RoomCode = "mr";
    private const string LogoutCode = "ae";
    private const string StepCode = "bs";
    private const string SurrenderCode = "be";
    private const string Split = "`";
    
    private const string Host = "47.100.61.46";
    // private const string Host = "127.0.0.1";
    private const int Port = 4567;

    private static bool _connected;

    private static Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    public static string Name = "未登录";

    public static bool ServerEnable = true;

    public static void Surrender()
    {
        Send(SurrenderCode);
    }

    public static void Step(int x, int y)
    {
        var code = x * 100 + y;
        Send(StepCode, code.ToString());
    }

    public static void Logout()
    {
        Send(LogoutCode);
    }

    public static void Room(string roomNumber)
    {
        Send(RoomCode, roomNumber);
    }
    
    public static void Match()
    {
        Send(MatchCode);
    }

    public static void Guest()
    {
        var mac = NetworkInterface.GetAllNetworkInterfaces()[0].GetPhysicalAddress().ToString();
        Send(GuestCode, mac, mac);
    }
    
    public static void Register(string username, string password)
    {
        Send(RegisterCode, username, password);
    }
    
    public static void Login(string username, string password)
    {
        Send(LoginCode, username, password);
    }

    private static void Send(params string[] message)
    {
        Connecting();
        try
        {
            var request = message[0];
            if (message.Length == 1)
            {
                request += "`0";
            }
            else
            {
                var i = 1;
                while (i < message.Length)
                {
                    request += Split + message[i];
                    i++;
                }
            }
            _socket.Send(Encoding.Default.GetBytes(request));
        }
        catch (SocketException e)
        {
            Debug.LogException(e);
            _connected = false;
            _socket.Close();
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
    }

    private static void Connecting()
    {
        if (_connected) return;
        try
        {
            _socket.Connect(Host, Port);
            _connected = true;
            Debug.Log("已连接");
        }
        catch (Exception e)
        {
            ServerEnable = false;
        }
    }

    public static string Receive()
    {
        Connecting();
        try
        {
            var buffer = new byte[64];
            var length = _socket.Receive(buffer);
            return Encoding.Default.GetString(buffer, 0, length);
        }
        catch (SocketException e)
        {
            Debug.LogException(e);
            _connected = false;
            _socket.Close();
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            return ConnectError;
        }
    }

}
