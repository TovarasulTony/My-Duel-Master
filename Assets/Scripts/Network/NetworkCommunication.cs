using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using System.Drawing;

public class NetworkCommunication : MonoBehaviour
{
    public static NetworkCommunication instance = null;
    //private IMailBox m_MailBoxRef;
    private TcpClient m_SocketConnection;
    private string m_ServerIP = "194.135.95.157";
    private int m_ServerPort = 55432;
    int m_ByteLength = 4096;
    Queue<string> m_SendingMessageQueue;
    long count = 0;
    double media = 0;
    double sum = 0;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("NetworkCommunication se instantiaza de 2 ori!");
            return;
        }
        instance = GetComponent<NetworkCommunication>();
        UnityEngine.Object.DontDestroyOnLoad(gameObject);
        m_SendingMessageQueue = new Queue<string>();
    }

    void Start()
    {
        try
        {
            m_SocketConnection = new TcpClient(m_ServerIP, m_ServerPort);
        }
        catch (Exception e)
        {
            Debug.Log("On client connect exception " + e);
        }
        ConnectToTcpServer();
    }

    private void ConnectToTcpServer()
    {
        try
        {
            Thread clientReceiveThread;
            clientReceiveThread = new Thread(new ThreadStart(ListenForData));
            clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();
        }
        catch (Exception e)
        {
            Debug.Log("On client connect exception " + e);
        }
        /*
		try
		{
			Thread clientReceiveThread;
			clientReceiveThread = new Thread(new ThreadStart(SendDataLoop));
			clientReceiveThread.IsBackground = true;
			clientReceiveThread.Start();
		}
		catch (Exception e)
		{
			Debug.Log("On client connect exception " + e);
		}*/
    }

    private void SendDataLoop()
    {
        while (true)
        {
            if (m_SendingMessageQueue.Count == 0)
            {
                continue;
            }
            while (m_SendingMessageQueue.Count != 0)
            {
                SendMessage(m_SendingMessageQueue.Dequeue());
            }
        }
    }

    private void ListenForData()
    {
        SendMessage("Salut lume!");
        Byte[] bytes = new Byte[m_ByteLength];
        while (true)
        {
            // Get a stream object for reading 				
            using (NetworkStream stream = m_SocketConnection.GetStream())
            {
                int length;
                // Read incomming stream into byte arrary.
                string serverMessage = "";
                while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    /*
		        	count++;
					media = media * (count - 1) / count;
					media += length / count;
					sum += length;
					Debug.Log(sum/count);
					*/
                    var incommingData = new byte[length];
                    Array.Copy(bytes, 0, incommingData, 0, length);

                    serverMessage += Encoding.ASCII.GetString(incommingData);
                    string[] messageList = serverMessage.Split('$');
                    int maxRange;
                    if (messageList[messageList.Length - 1] != "")
                    {
                        serverMessage = messageList[messageList.Length - 1];
                        maxRange = messageList.Length - 2;
                    }
                    else
                    {
                        serverMessage = "";
                        maxRange = messageList.Length - 1;
                    }
                    for (int i = 0; i <= maxRange; ++i)
                    {
                        if (messageList[i] == "")
                        {
                            continue;
                        }
                        Debug.Log(messageList[i]);
                        //m_MailBoxRef.AddMessageToQueue(messageList[i]);
                    }
                }
            }
        }
    }

    public void SendFile(string file, string ip)
    {
        if (m_SocketConnection == null)
        {
            return;
        }
        try
        {
            /*
            NetworkStream nwStream = m_SocketConnection.GetStream();
            MemoryStream ms = new MemoryStream();
            System.Drawing.Image x = System.Drawing.Image.FromFile(file);
            x.Save(ms, x.RawFormat);

            byte[] bytesToSend = ms.ToArray();

            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
            nwStream.Flush();*/
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }

    public void SendMessage(string _clientMessage)
    {
        Debug.Log("SendMessage");
        if (m_SocketConnection == null)
        {
            return;
        }
        try
        {
            NetworkStream stream = m_SocketConnection.GetStream();
            if (stream.CanWrite)
            {
                byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(_clientMessage);
                stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }

    public void AddMessageToQueue(string _clientMessage)
    {
        SendMessage(_clientMessage);
        //m_SendingMessageQueue.Enqueue(_clientMessage);
    }
    /*
    public void SetMailbox(IMailBox _mailbox)
    {
        m_MailBoxRef = _mailbox;
    }*/
}