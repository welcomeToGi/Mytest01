using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DPI.Tools
{
    public class Tool 
    {
        public static void CreatePaht(string conUrl)
        {
            string[] strA = conUrl.Split('?');
            string[] strArray;
            if (strA.Length == 2)
            {
                strArray = strA[0].Split('/');
            }
            else
            {
                strArray = conUrl.Split('/');
            }
            string paht = "";
            for (int i = 0; i < strArray.Length - 1; i++)
                paht = paht + strArray[i] + "/";
            while (Directory.Exists(paht) == false)
                Directory.CreateDirectory(paht);
        }
        public static void SaveText(string path, string jd)
        {
            StreamWriter sw;
            FileInfo t = new FileInfo(path);
            CreatePaht(path);
            if (!t.Exists)
                sw = t.CreateText();
            else
            {
                //如果此文件存在则打开
                FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(fs);
            }
            sw.Flush();
            //以行的形式写入信息
            sw.Write(jd);
            //关闭流
            sw.Close();
            //销毁流
            sw.Dispose();
        }



        public static void SaveFile(byte[] data, string conUrl)
        {
            CreatePaht(conUrl);
            FileInfo fileInfo = new FileInfo(conUrl);
            FileStream file = fileInfo.Create();
            file.Write(data, 0, data.Length);
            file.Flush();
            file.Close();
            file.Dispose();
        }
        public static void SetEventTrigger(GameObject obj, EventTriggerType eventTriggerType, Action<BaseEventData> action)
        {
            UnityAction<BaseEventData> unityAction = new UnityAction<BaseEventData>(action);
            EventTrigger.Entry eventTriggerEntry = new EventTrigger.Entry();
            eventTriggerEntry.eventID = eventTriggerType;
            eventTriggerEntry.callback.AddListener(unityAction);
            EventTrigger eventTrigger = obj.transform.GetComponent<EventTrigger>();
            if (eventTrigger == null)
            {
                eventTrigger = obj.gameObject.AddComponent<EventTrigger>();
            }
            eventTrigger.triggers.Add(eventTriggerEntry);
        }

    }
}
