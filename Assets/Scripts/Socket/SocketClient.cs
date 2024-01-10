using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine.UI;
using TMPro;
using System.Net;
public delegate void GetMessage(string msg);
public class SocketClient : MonoBehaviour
{
    public int portNo = 4302;
    private TcpClient _client;//TcpClient 是一个独立通信线程
    byte[] data;
    public TMP_InputField ip;

    //获取账号 消息内容
    public TMP_InputField user;
    public TMP_InputField messager;
    public TextMeshProUGUI server_Msg;//服务器返回信息

    [SerializeField] Button login;
    [SerializeField] Button send;
    [SerializeField] Button clearBtn;

    private Thread receiveThread;
    private GetMessage GetMessage;

    public void Awake()
    {
        //instance = this;
        login.onClick.AddListener(InitClientSocket);
        send.onClick.AddListener(Btn_Msg);
        clearBtn.onClick.AddListener(ClearMessage);
        //InitClientSocket();
    }

    private void ClearMessage()
    {
        server_Msg.text = string.Empty;
    }
    /// <summary>
    /// 客户端实例化Socket连接
    /// </summary>
    private void InitClientSocket()
    {
        _client = new TcpClient();
        try
        {
            //当前客户端连接的服务器地址与远程端口
            _client.Connect(IPAddress.Parse(ip.text), portNo);
            //开始接收服务器消息子线程
            receiveThread = new Thread(SocketReceiver);
            receiveThread.Start();
            Debug.Log("客户端-->服务端完成,开启接收消息线程");
        }
        catch (Exception ex)
        {
            Debug.Log("客户端连接服务器异常: " + ex.Message);
        }
        Debug.Log("连接到服务器 本地地址端口:" + _client.Client.LocalEndPoint + "  远程服务器端口:" + _client.Client.RemoteEndPoint);
    }
    private byte[] resultBuffer = new byte[1024];
    private string resultStr;

    /// <summary>
    /// 客户端检测收到服务器信息子线程
    /// </summary>
    private void SocketReceiver()
    {
        if (_client != null)
        {
            while (true)
            {
                if (_client.Client.Connected == false)
                    break;
                //在循环中，
                _client.Client.Receive(resultBuffer);
                resultStr = Encoding.UTF8.GetString(resultBuffer);
                Debug.Log("客户端收到服务器消息 : " + resultStr);
                Loom.QueueOnMainThread((a) => {
                    server_Msg.text = "客户端收到服务器消息 : " + resultStr;
                }, null);
            }
        }
    }

    /// <summary>
    /// 客户端发送消息到服务器
    /// </summary>
    private void SendMessageToServer(string msg)
    {
        try
        {
            string clientStr = msg;
            //获取当前客户端的流对象，然后将要发送的字符串转化为byte[]写入发送
            NetworkStream stream = _client.GetStream();
            byte[] buffer = Encoding.UTF8.GetBytes(clientStr);
            stream.Write(buffer, 0, buffer.Length);
        }
        catch (Exception ex)
        {
            Debug.Log("发送消息时服务器产生异常: " + ex.Message);
        }
    }
    public void Btn_Msg()
    {
        //Btn_Login();
        //SocketSendMessage(messager.text);
        SendMessageToServer(messager.text);
    }
}