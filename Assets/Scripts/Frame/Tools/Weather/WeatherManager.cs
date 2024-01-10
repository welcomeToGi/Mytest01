
using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using static UnityEngine.Networking.UnityWebRequest;

public class WeatherManager : MonoBehaviour
{

    //JsonUtility s;

    
   
    public string lbTime;
    public string lbHour;
    public string lbMinute;
    public string lbColon;
    private System.DateTime dNow;
    private string[] dow = { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
    private float deltaTime = 0.0f; //update Date  
    //private float deltaTimeforWeather = 0.0f; // update Weather  
    //private float howlongToUpdate = 3600f;

    public Text timeText;
    public Text weatherText;

    private string weather="";
    void Start()
    {
        StartCoroutine(RequestWeather(101120201));
        
    }
    void Update()
    {
        //Date  
        dNow = System.DateTime.Now;
        lbTime = dNow.ToString("yyyy/MM/dd | HH:mm:ss  | ") + dow[System.Convert.ToInt16(dNow.DayOfWeek)];
        lbHour = dNow.ToString("HH");
        lbMinute = dNow.ToString("mm");
        deltaTime += Time.deltaTime;
        if (deltaTime > 1.0f)
        {
            deltaTime = 0.0f;
            timeText.text = lbTime;
            weatherText.text = weather;
        }
      
    }


    //void OnEnable()
    //{

    //    UpdateWeather();
    IEnumerator RequestWeather(int id)
    {
        while (true)
        {
            string Weatherurl = "http://t.weather.sojson.com/api/weather/city/";
            using (UnityWebRequest webRequest = UnityWebRequest.Get(Weatherurl + id.ToString()))
            {
                yield return webRequest.SendWebRequest();

                //yield return webRequest.Send();
                if ((webRequest.result == Result.ConnectionError) || (webRequest.result == Result.ProtocolError))
                {
                    Debug.Log(webRequest.error);

                }
                else
                {
                    UTF8Encoding utf8 = new UTF8Encoding();
                    string data =webRequest.downloadHandler.text;
                    //string data = utf8.GetString(encodedBytes);

                   // Debug.Log(data);

                    Root cityInfo = JsonUtility.FromJson<Root>(data);
                    cityInfo = JsonConvert.DeserializeObject<Root>(data);
                    Debug.Log(cityInfo.data.forecast[0].high);

                    weather = " | " + cityInfo.data.forecast[0].low + "~" + cityInfo.data.forecast[0].high + " |";
                    LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetComponent<RectTransform>());
                }


            }
            yield return new WaitForSeconds(1000f);
        }
        
    }



}

//如果好用，请收藏地址，帮忙分享。
[System.Serializable]
public class CityInfo
{
    /// <summary>
    /// 东莞市
    /// </summary>
    public string city { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string citykey { get; set; }
    /// <summary>
    /// 广东
    /// </summary>
    public string parent { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string updateTime { get; set; }
}
[System.Serializable]
public class ForecastItem
{
    /// <summary>
    /// 
    /// </summary>
    public string date { get; set; }
    /// <summary>
    /// 高温 21℃
    /// </summary>
    public string high { get; set; }
    /// <summary>
    /// 低温 13℃
    /// </summary>
    public string low { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string ymd { get; set; }
    /// <summary>
    /// 星期二
    /// </summary>
    public string week { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string sunrise { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string sunset { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int aqi { get; set; }
    /// <summary>
    /// 东北风
    /// </summary>
    public string fx { get; set; }
    /// <summary>
    /// 2级
    /// </summary>
    public string fl { get; set; }
    /// <summary>
    /// 多云
    /// </summary>
    public string type { get; set; }
    /// <summary>
    /// 阴晴之间，谨防紫外线侵扰
    /// </summary>
    public string notice { get; set; }
}
[System.Serializable]
public class Yesterday
{
    /// <summary>
    /// 
    /// </summary>
    public string date { get; set; }
    /// <summary>
    /// 高温 22℃
    /// </summary>
    public string high { get; set; }
    /// <summary>
    /// 低温 12℃
    /// </summary>
    public string low { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string ymd { get; set; }
    /// <summary>
    /// 星期一
    /// </summary>
    public string week { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string sunrise { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string sunset { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int aqi { get; set; }
    /// <summary>
    /// 东北风
    /// </summary>
    public string fx { get; set; }
    /// <summary>
    /// 2级
    /// </summary>
    public string fl { get; set; }
    /// <summary>
    /// 晴
    /// </summary>
    public string type { get; set; }
    /// <summary>
    /// 愿你拥有比阳光明媚的心情
    /// </summary>
    public string notice { get; set; }
}
[System.Serializable]
public class Data
{
    /// <summary>
    /// 
    /// </summary>
    public string shidu { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string pm25 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string pm10 { get; set; }
    /// <summary>
    /// 优
    /// </summary>
    public string quality { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string wendu { get; set; }
    /// <summary>
    /// 各类人群可自由活动
    /// </summary>
    public string ganmao { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<ForecastItem> forecast { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public Yesterday yesterday { get; set; }
}
[System.Serializable]
public class Root
{
    /// <summary>
    /// success感谢又拍云(upyun.com)提供CDN赞助
    /// </summary>
    public string message { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string status { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string date { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string time { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public CityInfo cityInfo { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public Data data { get; set; }
}

