using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPReceive : MonoBehaviour
{

    Thread receiveThread;
    public UdpClient client;
    public bool isError = false;
    public int port = 5052;
    public bool startRecieving = true;
    public bool printToConsole = false;
    public string data;


    public void Start()
    {
        try 
        { 
            client = new UdpClient(port);
            receiveThread = new Thread(
            new ThreadStart(ReceiveData));
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }
        catch
        {
            client = null;
            data = null;
            isError = true;
        }
    }


    // receive thread
    private void ReceiveData()
    {
        if(client == null)
        {
            isError = true;
            startRecieving = false;

            return;
        }
        while (startRecieving)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] dataByte = client.Receive(ref anyIP);
                data = Encoding.UTF8.GetString(dataByte);
                if (printToConsole) { print(data); }
                isError = false;
            }
            catch (Exception err)
            {
                print(err.ToString());
                isError = true;
            }
        }
    }

}
