using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Point2dPos))]
public class Point2dPosEditor : Editor
{
	private Point2dPos _point2d;
	void OnEnable()
	{
		_point2d = target as Point2dPos;
	}
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
        if (GUILayout.Button("重绘"))
        {
			_point2d.DrawLine();
        }
    }
}
