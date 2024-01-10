using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CleanupMissingScripts : MonoBehaviour
{

    [MenuItem("GameObject/去除无效脚本", false, -100)]//设置在右键上显示
    static void Fun()
	{
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            //删除当前选择的物体以及子物体、孙子物体等所有物体身上的空脚本
            var gameObject = Selection.gameObjects[i];
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);
            Transform[] transforms = gameObject.GetComponentsInChildren<Transform>(true);
            for (int j = 0; j < transforms.Length; j++)
            {

                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(transforms[j].gameObject);
            }
        }
    }
  
}
