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

    private TextMeshProUGUI mText_CurTime;
    private TextMeshProUGUI mText_CurTimeHour;
    private TextMeshProUGUI mText_Xingqi;

    //private Toggle weatherOpenButton;
    private Button quit;
    //private GameObject weatherPanel;

    public Action OnGetWeather;
    public Weather myweather;
    //public Material waterMat;
    //public Material snowMat;

    void Start()
    {
        imgWeather = GetComponentInChildren<Image>();
        textWeather = transform.XuYiFindChild("天气").GetComponent<TextMeshProUGUI>();
        textTemperature = transform.XuYiFindChild("温度").GetComponent<TextMeshProUGUI>();
        //textCity = transform.XuYiFindChild("地点").GetComponent<TextMeshProUGUI>();


        mText_CurTime = transform.XuYiFindChild("日期").GetComponent<TextMeshProUGUI>();
        mText_CurTimeHour = transform.XuYiFindChild("时间").GetComponent<TextMeshProUGUI>();

        //weatherOpenButton= transform.XuYiFindChild("多种天气按钮").GetComponent<Toggle>();
        quit = transform.XuYiFindChild("退出").GetComponent<Button>();
        //var canvas = transform.GetComponentInParent<Canvas>();
        //if(canvas!=null)
        //weatherPanel = canvas.transform.XuYiFindChild("天气具体信息").gameObject;
        mText_Xingqi= transform.XuYiFindChild("星期").GetComponent<TextMeshProUGUI>();


        //waterMat.SetFloat("_StandingWaterAmount", 0.6f);
        //snowMat.SetFloat("_SnowAmount", 0.6f);
        //weatherOpenButton.onValueChanged.AddListener((ison) =>
        //{
        //    weatherPanel.SetActive(ison);
        //});
        quit.onClick.AddListener(QuitGameObject);
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
            //Debug.Log(spriteName);

            //温度
            textTemperature.text = string.Format("{0} °C", weatherData["results"][0]["now"]["temperature"].ToString());
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }

        myweather.wendu = textTemperature.text;
        OnGetWeather?.Invoke();
    }
    private void Update()
    {
        //DateTime.Now.ToString('%Y %m %d %H:%M:%S')  DateTime.Now.ToString("yyyy-MM-dd");
        mText_CurTime.text = DateTime.Now.ToString("yyyy年MM月dd日");
        // 根据当前系统时间判定：
        string weekstr = DateTime.Now.DayOfWeek.ToString();
        switch (weekstr)
        {
            case "Monday": weekstr = "星期一"; break;
            case "Tuesday": weekstr = "星期二"; break;
            case "Wednesday": weekstr = "星期三"; break;
            case "Thursday": weekstr = "星期四"; break;
            case "Friday": weekstr = "星期五"; break;
            case "Saturday": weekstr = "星期六"; break;
            case "Sunday": weekstr = "星期日"; break;
        }
        mText_CurTimeHour.text = DateTime.Now.ToString("HH:mm:ss");
        mText_Xingqi.text = weekstr;
    }
    private void QuitGameObject()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif

    }
}
public struct Weather
{
    public string wendu;
    public string shidu;
    public string fengli;
    public string fengxiang;
    public string jiangyuliang;
}

//      {
//          "code":"200",
//          "updateTime":"2023-11-13T11:37+08:00",
//          "fxLink":"https://www.qweather.com/weather/beijing-101010100.html",
//          "now":
//              {
//              "obsTime":"2023-11-13T11:26+08:00",
//              "temp":"5",
//              "feelsLike":"1",
//              "icon":"101",
//              "text":"多云",
//              "wind360":"158",
//              "windDir":"南风",
//              "windScale":"1",
//              "windSpeed":"2",
//              "humidity":"26",
//              "precip":"0.0",
//              "pressure":"1031",
//              "vis":"30",
//              "cloud":"10",
//              "dew":"-11"
//              },
//          "refer":
//              {
//              "sources":["QWeather"],
//              "license":["CC BY-SA 4.0"]
//              }
//      }    

/*{ "address":"CN|\u5317\u4eac\u5e02|\u5317\u4eac\u5e02|None|None|100|100",
 * "content":{ 
 * "address":"\u5317\u4eac\u5e02",
 * "address_detail":{
 * "adcode":"110000",
 * "city":"\u5317\u4eac\u5e02",
 * "city_code":131,"district":"",
 * "province":"\u5317\u4eac\u5e02",
 * "street":"",
 * "street_number":""},
 * "point":{
 * "x":"116.41338370","y":"39.91092455"}
 * },"status":0}*/
