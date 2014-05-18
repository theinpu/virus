﻿using UnityEditor;
using UnityEngine;
using System.Collections;

public class GameGlobalScript : MonoBehaviour {

	public int PlayerCount = 2;
    public PlayerSetting[] PlayerSetting;

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}

    public void InitPlayerSettings()
    {
        PlayerSetting = new PlayerSetting[PlayerCount];
        for (var i = 0; i < PlayerCount; i++)
        {
            PlayerSetting[i] = new PlayerSetting();
        }
    }
}