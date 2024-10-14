using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.IO;
using TMPro;
using System.Globalization;

public class TCPListner : MonoBehaviour
{
    public static TCPListner instance;
    public static int dataBufferSize = 4096;

    public string ip = "192.168.31.18";
    public int port = 5045;
    public int myId = 0;
    public static TCP tcp;

    // public TMP_Text X;
    // public TMP_Text Y;

    public static Vector3 eulers {get; set;}


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    private void Start()
    {
        tcp = new TCP();
        ConnectToServer();
    }
    private void Update()
    {
        // X.text = MessageToString.GetX().ToString();
        // Y.text = MessageToString.GetY().ToString();
        //xValue = MessageToString.GetX();
        //yValue = MessageToString.GetY();
        eulers = MessageToString.GetEulers();
    }

    public static void ConnectToServer()
    {
        tcp.Connect(instance.ip, instance.port);
    }

    public static void ConnectToServer(string ip, int port)
    {
        tcp.Connect(ip, port);
    }

    public static class MessageToString
    {
        public static string Coordinates;
        public static Vector3 GetEulers(){
            //print(Coordinates); 
            if (string.IsNullOrEmpty(Coordinates))
                return Vector3.zero;
            //string substrX = Coordinates.Substring(4,6);
            string[] subs = Coordinates.Split(',');
            Vector3 eulers = new Vector3(float.Parse(subs[0], CultureInfo.InvariantCulture), float.Parse(subs[2],CultureInfo.InvariantCulture), float.Parse(subs[2],CultureInfo.InvariantCulture));
            print(eulers);
            return eulers;
        }
    }

    public class TCP
    {
        public TcpClient socket;

        private NetworkStream stream;
        private byte[] receiveBuffer;

        public void Connect(string ip, int port)
        {
            print("Attempting to connect to " + ip + ":" + port);
            socket = new TcpClient
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };

            receiveBuffer = new byte[dataBufferSize];
            socket.BeginConnect(ip, port, ConnectCallback, socket);
        }
        private void ConnectCallback(IAsyncResult _result)
        {
             socket.EndConnect(_result);
             if (!socket.Connected)
             {
                 return;
             }
             stream = socket.GetStream();
             stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);

        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
             {
                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    return;
                }
                byte[] _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);

                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);

                string results = System.Text.Encoding.UTF8.GetString(receiveBuffer);
                MessageToString.Coordinates = results;
             }
             catch
             {
                //stream.Close();
             }
           
        }


    }
}
