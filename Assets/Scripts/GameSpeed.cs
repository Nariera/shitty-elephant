using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpeed : MonoBehaviour
{

	public float speed = 1;

	void Update()
	{
		Time.timeScale = speed;
	}
}
