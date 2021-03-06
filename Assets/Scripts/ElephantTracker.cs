﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElephantTracker : MonoBehaviour
{

	public Transform theLiphantis;
	CanvasGroup canvasGroup;

	void Update()
	{
		Vector3 v3Pos = Camera.main.WorldToViewportPoint(theLiphantis.transform.position);

		if (!(v3Pos.x >= -0.1f && v3Pos.x <= 1.1f && v3Pos.y >= -0.1f && v3Pos.y <= 1.1f))
		{		
			var xScreenDist = 2 - Mathf.Abs(v3Pos.x);
			var yScreenDist = 3 - Mathf.Abs(v3Pos.y);
			var sign = xScreenDist < 0 && yScreenDist < 0 ? -1 : 1;
			var distMultiplier = Mathf.Clamp01(xScreenDist * yScreenDist * sign);

			canvasGroup.alpha = 1;

			transform.localScale = Vector3.one * distMultiplier;

			v3Pos.x -= 0.5f;  // Translate to use center of viewport
			v3Pos.y -= 0.5f; 
			v3Pos.z = 0;      // I think I can do this rather than do a 
			//   a full projection onto the plane

			float fAngle = Mathf.Atan2(v3Pos.x, v3Pos.y);

			transform.eulerAngles = theLiphantis.eulerAngles;
			//transform.localEulerAngles = new Vector3(0.0f, 0.0f, -fAngle * Mathf.Rad2Deg);

			v3Pos.x = 0.45f * Mathf.Sin(fAngle) + 0.5f;  // Place on ellipse touching 
			v3Pos.y = 0.42f * Mathf.Cos(fAngle) + 0.5f;  //   side of viewport
			v3Pos.z = Camera.main.nearClipPlane + 0.01f;  // Looking from neg to pos Z;
			transform.position = Camera.main.ViewportToWorldPoint(v3Pos);
		}
		else
		{
			canvasGroup.alpha = 0;
		}

	}

	void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
	}
}
