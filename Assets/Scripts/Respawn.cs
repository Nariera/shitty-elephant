﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{

	public PlayerControl player;
	
	// Update is called once per frame
	void Update()
	{
		if ((Input.GetKeyUp(KeyCode.R) || Input.GetKeyUp(KeyCode.Space)) && player != null && !player.isActiveAndEnabled)
		{
			player.Respawn();
		}
	}
}
