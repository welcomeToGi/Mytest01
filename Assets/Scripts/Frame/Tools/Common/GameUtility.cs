using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class GameUtility 
{
    /// <summary>        /// 等待事件        /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <param name="delay"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static Coroutine SetDelay<T>(this T t, float delay, UnityAction action) where T : MonoBehaviour
    {
        ProgramExtensions.Create();
        return ProgramExtensions.instance.WaitForCompletion(delay, action);
    }

    /// <summary>
    /// 物体渐显
    /// </summary>
    /// <param name="go"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static GameObject FadeIn(this GameObject go, TweenCallback action = null)
    {
        if (go.GetComponent<DissolveObj>() == null)
        {
            go.AddComponent<DissolveObj>();
        }
        go.GetComponent<DissolveObj>().FadeIn(action);
        return go;
    }
    /// <summary>
    /// 物体渐隐
    /// </summary>
    /// <param name="go"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static GameObject FadeOut(this GameObject go, TweenCallback action = null)
    {
        if (go.GetComponent<DissolveObj>() == null)
        {
            go.AddComponent<DissolveObj>();
        }
        go.GetComponent<DissolveObj>().FadeOut(action);
        return go;
    }

    /// <summary>
    /// 高亮
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    public static GameObject OnHightligher(this GameObject go)
    {
        //Highlighter high = go.GetComponent<Highlighter>();
        //if (high == null)
        //    high = go.AddComponent<Highlighter>();
        //high.constantColor = Color.red;
        //high.enabled = true;
        //high.constant = true;
        ////high.TweenStart();
        return go;
    }

    /// <summary>
    /// 取消高亮
    /// </summary>
    /// <param name="go"></param>
    /// <param name="isDele"></param>
    /// <returns></returns>
    public static GameObject OffHightligher(this GameObject go, bool isDele = false)
    {
        //Highlighter high = go.GetComponent<Highlighter>();
        //if (high == null)
        //    high = go.AddComponent<Highlighter>();
        //high.TweenStop();

        //if (isDele)
        //    UnityEngine.Object.Destroy(high);
        return go;
    }
}
