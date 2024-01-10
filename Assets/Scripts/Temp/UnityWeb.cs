using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class UnityWeb : MonoBehaviour
{

    [DllImport("__Internal")]
    private static extern void SayHello();

    [DllImport("__Internal")]
    private static extern void ReportReady();

    private void Start()
    {
        ReportReady();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SayHello();
        }
    }

    public void GetInfo(string s)
    {
        transform.XuYiFindChild("Text").GetComponent<Text>().text = s;
    }
}