using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static UnityEngine.Networking.UnityWebRequest;
//using LitJson;

/// <summary>
/// 城市天气
/// </summary>
public class CityWeather : MonoBehaviour
{
    private Image imgWeather;    //天气图片
    private TextMeshProUGUI textWeather;    //天气
    private TextMeshProUGUI textTemperature;//温度

    private TextMeshProUGUI mText_nowDate;
    private TextMeshProUGUI mText_nowTime;

    private DateTime mytime;
    private string[] weekStr = { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
    private int day;
    void Start()
    {
        imgWeather = GetComponentInChildren<Image>();
        textWeather = transform.XuYiFindChild("天气").GetComponent<TextMeshProUGUI>();
        textTemperature = transform.XuYiFindChild("温度").GetComponent<TextMeshProUGUI>();
        mText_nowDate = transform.XuYiFindChild("日期").GetComponent<TextMeshProUGUI>();
        mText_nowTime = transform.XuYiFindChild("时间").GetComponent<TextMeshProUGUI>();

        UpdataDate();
        StartCoroutine(GetRuntimeWeather());
    }

    /// <summary>
    /// 温度、湿度、辐照度、风力、风向、降雨量
    /// </summary>

    IEnumerator GetRuntimeWeather()
    {
        //1.获取本地公网IP
        UnityWebRequest wwwWebIp = UnityWebRequest.Get(@"http://icanhazip.com/");
        yield return wwwWebIp.SendWebRequest();
        if ((wwwWebIp.result == Result.ConnectionError) || (wwwWebIp.result == Result.ProtocolError))
        {
            yield break;
        }
        //2.根据IP查询城市（心知天气提供接口，需要申请key）***这里别忘记修改
        string urlQueryCity = "https://api.seniverse.com/v3/location/search.json?key=SAkfeWBi2ISayjDqx&q=" + wwwWebIp.downloadHandler.text;
        UnityWebRequest wwwQueryCity = UnityWebRequest.Get(urlQueryCity);
        yield return wwwQueryCity.SendWebRequest();
        if ((wwwQueryCity.result == Result.ConnectionError) || (wwwQueryCity.result == Result.ProtocolError))
        {
            yield break;
        }

        JObject cityData = JsonConvert.DeserializeObject<JObject>(wwwQueryCity.downloadHandler.text);
        string cityId = cityData["results"][0]["id"].ToString();
        //textCity.text = cityData["results"][0]["name"].ToString(); //城市

        //3.根据城市查询天气（心知天气提供接口，需要申请key）***这里别忘记修改
        string urlWeather = string.Format("https://api.seniverse.com/v3/weather/now.json?key=SAkfeWBi2ISayjDqx&location={0}&language=zh-Hans&unit=c", cityId);
        UnityWebRequest wwwWeather = UnityWebRequest.Get(urlWeather);
        yield return wwwWeather.SendWebRequest();

        if ((wwwWeather.result == Result.ConnectionError) || (wwwWeather.result == Result.ProtocolError))
        {
            Debug.Log(wwwWeather.error);
        }

        //4.解析天气
        try
        {
            JObject weatherData = JsonConvert.DeserializeObject<JObject>(wwwWeather.downloadHandler.text);
            string spriteName = string.Format("Weather/{0}@2x", weatherData["results"][0]["now"]["code"].ToString());

            //天气文字
            textWeather.text = weatherData["results"][0]["now"]["text"].ToString();

            //图片，可以在心知天气上下载
            imgWeather.sprite = Resources.Load<Sprite>(spriteName);
            imgWeather.enabled = true;

            //温度
            textTemperature.text = string.Format("{0} °C", weatherData["results"][0]["now"]["temperature"].ToString());
            
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
    private void Update()
    {
        // 根据当前系统时间判定：
        mytime = DateTime.Now;
        mText_nowTime.text =  mytime.ToString("HH:mm:ss");
        if (mytime.Day!=day)
        {
            UpdataDate();
        }
    }
    private void QuitGameObject()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif

    }
    private void UpdataDate()
    {
        mytime = DateTime.Now;
        mText_nowDate.text = mytime.ToString("yyyy年MM月dd日") + " | " + weekStr[Convert.ToInt16(mytime.DayOfWeek)]+" | ";
        day = mytime.Day;
    }
}
