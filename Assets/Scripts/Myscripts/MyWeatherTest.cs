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
    public float snowTime = 120f;
    //public NM_Wind myWind;
    [SerializeField] WeatherDataConfig weatherData; //天气数据 我们配置的文件拖入即可

    private GlobalSnow glowbleSnow;
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
            //根据传过来的天气名称找到对应的插件天气下标
            int index = 0;
            for (int i = 0; i < weatherData.weatherData.Count; i++)
            {
                //名称相同 获取天气下标
                if (weatherData.weatherData[i].weatherName.Equals(c_wetherName))
                {
                    index = weatherData.weatherData[i].uniStormWeatherIndex;
                    break;
                }
            }

            //根据插件天气下标，获取天气
            WeatherType weather = UniStormSystem.Instance.AllWeatherTypes[index];
            UniStormManager.Instance.ChangeWeatherInstantly(weather); //直接切换

            glowbleSnow.enabled = false;
            glowbleSnow.snowAmount = 0;

            if (c_wetherName=="大雪")
            {
                glowbleSnow.enabled = true;
                glowbleSnow.snowAmount = 0;
                snowCount = 0;
            }
        }
    }
    //时间实时
    private void RealTimeUpdateTime()
    {
        UniStormSystem.Instance.RealWorldTime = UniStormSystem.EnableFeature.Enabled;
        UniStormSystem.Instance.m_TimeFloat = (DateTime.Now.Hour * 60.0f + DateTime.Now.Minute) / (60.0f * 24.0f);
    }
}

