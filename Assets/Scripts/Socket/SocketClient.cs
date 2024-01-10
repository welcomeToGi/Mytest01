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
    private TcpClient _client;//TcpClient ��һ������ͨ���߳�
    byte[] data;
    public TMP_InputField ip;

    //��ȡ�˺� ��Ϣ����
    public TMP_InputField user;
    public TMP_InputField messager;
    public TextMeshProUGUI server_Msg;//������������Ϣ

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
    /// �ͻ���ʵ����Socket����
    /// </summary>
    private void InitClientSocket()
    {
        _client = new TcpClient();
        try
        {
            //��ǰ�ͻ������ӵķ�������ַ��Զ�̶˿�
            _client.Connect(IPAddress.Parse(ip.text), portNo);
            //��ʼ���շ�������Ϣ���߳�
            receiveThread = new Thread(SocketReceiver);
            receiveThread.Start();
            Debug.Log("�ͻ���-->��������,����������Ϣ�߳�");
        }
        catch (Exception ex)
        {
            Debug.Log("�ͻ������ӷ������쳣: " + ex.Message);
        }
        Debug.Log("���ӵ������� ���ص�ַ�˿�:" + _client.Client.LocalEndPoint + "  Զ�̷������˿�:" + _client.Client.RemoteEndPoint);
    }
    private byte[] resultBuffer = new byte[1024];
    private string resultStr;

    /// <summary>
    /// �ͻ��˼���յ���������Ϣ���߳�
    /// </summary>
    private void SocketReceiver()
    {
        if (_client != null)
        {
            while (true)
            {
                if (_client.Client.Connected == false)
                    break;
                //��ѭ���У�
                _client.Client.Receive(resultBuffer);
                resultStr = Encoding.UTF8.GetString(resultBuffer);
                Debug.Log("�ͻ����յ���������Ϣ : " + resultStr);
                Loom.QueueOnMainThread((a) => {
                    server_Msg.text = "�ͻ����յ���������Ϣ : " + resultStr;
                }, null);
            }
        }
    }

    /// <summary>
    /// �ͻ��˷�����Ϣ��������
    /// </summary>
    private void SendMessageToServer(string msg)
    {
        try
        {
            string clientStr = msg;
            //��ȡ��ǰ�ͻ��˵�������Ȼ��Ҫ���͵��ַ���ת��Ϊbyte[]д�뷢��
            NetworkStream stream = _client.GetStream();
            byte[] buffer = Encoding.UTF8.GetBytes(clientStr);
            stream.Write(buffer, 0, buffer.Length);
        }
        catch (Exception ex)
        {
            Debug.Log("������Ϣʱ�����������쳣: " + ex.Message);
        }
    }
    public void Btn_Msg()
    {
        //Btn_Login();
        //SocketSendMessage(messager.text);
        SendMessageToServer(messager.text);
    }
}