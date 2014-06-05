﻿using System;
using UnityEngine;

public class GameLobby : MonoBehaviour
{
    private GameGlobalScript gameGlobal;
    private GridOverlay grid;

    private int state = 0;
    private int playerSettingsCurrent = 0;
    private int playerClicks = 0;

    private const float Epsilon = 0.0001f;

    private Color[] playerColors =
    {
        new Color(1f, 0, 0, 0.75f), 
        new Color(0, 0, 1f, 0.75f), 
        new Color(0, 1f, 0, 0.75f), 
        new Color(1f, 1f, 0, 0.75f)
    };

    // Use this for initialization
    void Start()
    {
        var gameGlobalObject = GameObject.Find("GameGlobal");
        gameGlobal = (GameGlobalScript)gameGlobalObject.GetComponent(typeof(GameGlobalScript));
        grid = (GridOverlay)Camera.main.GetComponent(typeof(GridOverlay));
    }

    void Update()
    {
        if (state == 1)
        {
            if (Input.GetMouseButtonUp(0) && grid.IsInField())
            {
                if (grid.AddPoint(playerColors[playerSettingsCurrent]) && playerClicks < GameGlobalScript.MaxCells)
                {
                    gameGlobal.PlayerSetting[playerSettingsCurrent].StartingPosition[playerClicks] = grid.FieldPoint;
                    playerClicks++;
                }
                else
                {
                    if (grid.Occupy(playerColors[playerSettingsCurrent]))
                    {
                        if (grid.RemovePoint())
                        {
                            playerClicks--;
                        }
                    }
                }
            }
        }
    }

    void OnGUI()
    {
        switch (state)
        {
            case 0:
                DrawDefaultScreen();
                break;
            case 1:
                DrawPlayerParamsScreen();
                break;
            case 2:
                DrawStartPositionSelect();
                break;
        }
        if (GUI.Button(new Rect(Screen.width - 110, 10, 100, 30), "Exit"))
        {
            Application.Quit();
        }
    }

    void DrawDefaultScreen()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "Players: " + gameGlobal.PlayerCount);
        gameGlobal.PlayerCount = (int)GUI.HorizontalSlider(new Rect(10, 30, 100, 30), gameGlobal.PlayerCount, 2, 4);

        GUI.Label(new Rect(10, 60, 150, 20), "Game field width: " + gameGlobal.GameSettings.FieldWidth);
        GUI.Label(new Rect(10, 110, 150, 20), "Game field height: " + gameGlobal.GameSettings.FieldHeight);

        gameGlobal.GameSettings.FieldWidth = (int)GUI.HorizontalSlider(new Rect(10, 80, 100, 30), gameGlobal.GameSettings.FieldWidth, 8, 42);
        gameGlobal.GameSettings.FieldHeight = (int)GUI.HorizontalSlider(new Rect(10, 130, 100, 30), gameGlobal.GameSettings.FieldHeight, 8, 42);

        var dist = Mathf.Max(gameGlobal.GameSettings.FieldWidth, gameGlobal.GameSettings.FieldHeight);
        Camera.main.transform.position = new Vector3(0, dist + 3f, 0);
        grid.Reset();

        if (GUI.Button(new Rect(10, 200, 100, 30), "Next"))
        {
            gameGlobal.InitPlayerSettings();

            state++;
            grid.Visible = true;

            grid.RectColor = playerColors[0];
        }
    }

    void DrawPlayerParamsScreen()
    {
        if (playerSettingsCurrent < gameGlobal.PlayerCount)
        {
            GUI.Label(new Rect(10, 10, 100, 20), "Player " + (playerSettingsCurrent + 1) + " settings");
            var strength = gameGlobal.PlayerSetting[playerSettingsCurrent].Strength;
            var endurance = gameGlobal.PlayerSetting[playerSettingsCurrent].Endurance;
            var dexterity = gameGlobal.PlayerSetting[playerSettingsCurrent].Dexterity;
            var minAgeRep = gameGlobal.PlayerSetting[playerSettingsCurrent].MinReproductiveAge;
            var maxAgeRep = gameGlobal.PlayerSetting[playerSettingsCurrent].MaxReproductiveAge;

            var lifeCycle = Mathf.Floor((maxAgeRep - minAgeRep) / PlayerSetting.BoundsMultiplier + 0.5f);
            var unspentPoints = PlayerSetting.MaxPoint - lifeCycle - strength - endurance - dexterity;

            GUI.Label(new Rect(10, 30, 100, 20), "Cells left: " + (5 - playerClicks));
            GUI.Label(new Rect(10, 50, 150, 30), "Power: " + (int)strength);
            GUI.Label(new Rect(10, 90, 150, 30), "Vitality: " + (int)endurance);
            GUI.Label(new Rect(10, 130, 150, 30), "Speed: " + (int)dexterity);
            GUI.Label(new Rect(10, 170, 150, 30), "Rep age min: " + (int)minAgeRep + "%");
            GUI.Label(new Rect(10, 210, 150, 30), "Rep age max: " + (int)maxAgeRep + "%");

            var newStrength = (int)GUI.HorizontalSlider(new Rect(10, 70, 100, 30), (int)strength, 1f, PlayerSetting.MaxStat);
            var newEndurance = (int)GUI.HorizontalSlider(new Rect(10, 110, 100, 30), (int)endurance, 1f, PlayerSetting.MaxStat);
            var newDexterity = (int)GUI.HorizontalSlider(new Rect(10, 150, 100, 30), (int)dexterity, 1f, PlayerSetting.MaxStat);
            var newMinAge = (int)GUI.HorizontalSlider(new Rect(10, 190, 100, 30), (int)minAgeRep, 10f, 90f);
            var newMaxAge = (int)GUI.HorizontalSlider(new Rect(10, 230, 100, 30), (int)maxAgeRep, 10f, 90f);

            if (Math.Abs(strength - newStrength) > Epsilon) gameGlobal.PlayerSetting[playerSettingsCurrent].Strength = newStrength;
            if (Math.Abs(endurance - newEndurance) > Epsilon) gameGlobal.PlayerSetting[playerSettingsCurrent].Endurance = newEndurance;
            if (Math.Abs(dexterity - newDexterity) > Epsilon) gameGlobal.PlayerSetting[playerSettingsCurrent].Dexterity = newDexterity;
            if (Math.Abs(minAgeRep - newMinAge) > Epsilon) gameGlobal.PlayerSetting[playerSettingsCurrent].MinReproductiveAge = newMinAge;
            if (Math.Abs(maxAgeRep - newMaxAge) > Epsilon) gameGlobal.PlayerSetting[playerSettingsCurrent].MaxReproductiveAge = newMaxAge;

            gameGlobal.PlayerSetting[playerSettingsCurrent].Normalize();

            if (GUI.Button(new Rect(10, 260, 100, 30), "Next"))
            {
                if (playerClicks < 5) return;

                playerSettingsCurrent++;
                playerClicks = 0;
                if (playerSettingsCurrent < gameGlobal.PlayerCount)
                {
                    grid.RectColor = playerColors[playerSettingsCurrent];
                }
            }
        }
        else
        {
            state++;

            grid.Visible = false;
        }
    }

    void DrawStartPositionSelect()
    {
        if (GUI.Button(new Rect(10, 10, 100, 30), "Start game"))
        {
            Application.LoadLevel("gameScene");
        }
    }
}
