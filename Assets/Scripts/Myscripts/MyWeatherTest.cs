/****************************************************
    文件：ScriptsName.cs
    作者：XXX
    日期：#20230329#
    功能：XXX
*****************************************************/
using GlobalSnowEffect;
using System;
using System.Collections;
using UniStorm;
using UnityEngine;
using UnityEngine.UI;

public class MyWeatherTest : MonoBehaviour
{
    public NM_Wind myWind;
    private GlobalSnow glowbleSnow;
    public float snowTime = 120f;
    private float snowCount = 0;
    private Toggle[] toWeaToggles;   
    // Start is called before the first frame update
    void Start()
    {
        glowbleSnow = Camera.main.GetComponent<GlobalSnow>();

        StartCoroutine(WaitForInilialization());
    }

    // Update is called once per frame
    void Update()
    {
        if (glowbleSnow.enabled)
        {
            snowCount += Time.deltaTime;
            glowbleSnow.snowAmount = (snowCount / snowTime);
        }
    }
    IEnumerator WaitForInilialization()
    {
        yield return new WaitUntil(() => UniStormSystem.Instance.UniStormInitialized);
        CreateUniStormMenu();
    }
    void CreateUniStormMenu()
    {
        //UniStormSystem.Instance.m_TimeFloat = 0;
        toWeaToggles = GetComponentsInChildren<Toggle>();
        foreach (Toggle toggle in toWeaToggles)
        {
            toggle.onValueChanged.AddListener(ison => ChangeWeatherUI(ison, toggle.name));
        }
        //var canvas = transform.GetComponentInParent<Canvas>();
        //if (canvas != null)
        //{
        //    var weather_first = canvas.transform.GetComponentInChildren<CityWeather>();
        //    weather_first.OnGetWeather += () =>
        //    {
        //        //myWeatherText02 = canvas.transform.XuYiFindChild("天气").GetComponent<TextMeshProUGUI>();
        //    };
        //}
    }
    /// <summary>
    /// 天气选择
    /// </summary>
    /// <param name="weatherbool"></param>
    /// <param name="weatherValue"></param>
    public void ChangeWeatherUI(bool weatherbool,string c_wetherName)
    {
        if (weatherbool)
        {
            myWind.WindSpeed = 45;
            glowbleSnow.enabled = false;
            RenderSettings.fog = false;
            glowbleSnow.snowAmount = 0;
            //glowbleSnow.snowfallSystem.Stop();
            int weatherIndex = 0;
            switch (c_wetherName)
            {
                case "大雪":
                    {
                        weatherIndex = 3;
                        glowbleSnow.enabled = true;
                        glowbleSnow.snowAmount = 0;
                        snowCount = 0;
                        //glowbleSnow.snowfallSystem.Play();
                        break;
                    }
                case "大风":
                    {
                        myWind.WindSpeed = 250;
                        weatherIndex = 1;
                        break ;
                    }
                case "大雾":
                    {
                        weatherIndex = 2;
                        RenderSettings.fog = true;
                        break;
                    }
                case "大雨":
                    {
                        weatherIndex = 4;
                        break;
                    }
            }
            UniStormManager.Instance.ChangeWeatherWithTransition(UniStormSystem.Instance.AllWeatherTypes[weatherIndex]);
        }
    }
    //时间实时
    private void RealTimeUpdateTime()
    {
        UniStormSystem.Instance.RealWorldTime = UniStormSystem.EnableFeature.Enabled;
        UniStormSystem.Instance.m_TimeFloat = (DateTime.Now.Hour * 60.0f + DateTime.Now.Minute) / (60.0f * 24.0f);
    }
}

