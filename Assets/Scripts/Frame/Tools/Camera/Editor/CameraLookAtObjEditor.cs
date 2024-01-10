using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraLookAtObj))]
public class CameraLookAtObjEditor : Editor
{
	private CameraLookAtObj _CmaeraLookAt;
	void OnEnable()
	{
		_CmaeraLookAt = target as CameraLookAtObj;
	}
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		if (GUILayout.Button("设置摄像机位置"))
		{
			_CmaeraLookAt.pos = SceneView.lastActiveSceneView.camera.transform.position;
			_CmaeraLookAt.Rot = SceneView.lastActiveSceneView.camera.transform.rotation;
		}
	}
}
