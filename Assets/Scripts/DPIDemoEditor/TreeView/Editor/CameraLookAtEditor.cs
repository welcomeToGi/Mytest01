using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraLookAt))]
public class CameraLookAtEditor : Editor
{
	private CameraLookAt _CmaeraLookAt;
	void OnEnable()
	{
		_CmaeraLookAt = target as CameraLookAt;
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
