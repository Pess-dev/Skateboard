using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.IO;
using TMPro;

public class TCPListner : MonoBehaviour
{
    public static TCPListner instance;
    public static int dataBufferSize = 4096;

    public string ip = "192.168.31.18";
    public int port = 5045;
    public int myId = 0;
    public TCP tcp;

    // public TMP_Text X;
    // public TMP_Text Y;

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

        //transform.rotation = Quaternion.Euler(MessageToString.GetX(), 0, MessageToString.GetY());
    }

    public void ConnectToServer()
    {
        tcp.Connect();
    }
    public static class MessageToString
    {
        public static string Coordinates;

        public static float GetX()
        {
            if (string.IsNullOrEmpty(Coordinates))
                return -100f;
            string substrX = Coordinates.Substring(4,6);
            Debug.Log(substrX);
            return float.Parse(substrX);
        }
        public static float GetY()
        {
            if (string.IsNullOrEmpty(Coordinates))
                return -100f;
            string substrY = Coordinates.Substring(12, 6);
            Debug.Log(substrY);
            return float.Parse(substrY);
        }
    }

    public class TCP
    {
        public TcpClient socket;

        private NetworkStream stream;
        private byte[] receiveBuffer;

        public void Connect()
        {
            socket = new TcpClient
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };

            receiveBuffer = new byte[dataBufferSize];
            socket.BeginConnect(instance.ip, instance.port, ConnectCallback, socket);
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
