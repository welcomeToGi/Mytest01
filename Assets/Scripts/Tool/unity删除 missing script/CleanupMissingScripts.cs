using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CleanupMissingScripts : MonoBehaviour
{

    [MenuItem("GameObject/ȥ����Ч�ű�", false, -100)]//�������Ҽ�����ʾ
    static void Fun()
	{
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            //ɾ����ǰѡ��������Լ������塢��������������������ϵĿսű�
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
