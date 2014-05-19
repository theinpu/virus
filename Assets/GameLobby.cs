using System;
using UnityEngine;

public class GameLobby : MonoBehaviour
{
    private GameGlobalScript gameGlobal;
    private GridOverlay grid;

    private int state = 0;
    private int playerSettingsCurrent = 0;
    private int playerClicks = 0;

    private const float Epsilon = 0.01f;

    // Use this for initialization
    void Start()
    {
        var gameGlobalObject = GameObject.Find("GameGlobal");
        gameGlobal = (GameGlobalScript)gameGlobalObject.GetComponent(typeof(GameGlobalScript));

        grid = (GridOverlay) Camera.main.GetComponent(typeof (GridOverlay));
    }

    void Update()
    {
        if (state == 1)
        {
            if (Input.GetMouseButtonUp(0) && grid.IsInField() && playerClicks < GameGlobalScript.MaxCells)
            {
                gameGlobal.PlayerSetting[playerSettingsCurrent].StartingPosition[playerClicks] = grid.FieldPoint;
                playerClicks++;
                grid.AddPoint();
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
    }

    void DrawDefaultScreen()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "Players: " + gameGlobal.PlayerCount);
        gameGlobal.PlayerCount = (int)GUI.HorizontalSlider(new Rect(10, 30, 100, 50), gameGlobal.PlayerCount, 2, 4);
        if (GUI.Button(new Rect(10, 80, 100, 30), "Next"))
        {
            gameGlobal.InitPlayerSettings();
            state++;
            grid.Visible = true;
            
            grid.RectColor = new Color(1f, 0f, 0f, .75f);
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

            GUI.Label(new Rect(10, 30, 150, 30), "Strength: " + (int)strength);
            GUI.Label(new Rect(10, 70, 150, 30), "Endurance: " + (int)endurance);
            GUI.Label(new Rect(10, 110, 150, 30), "Dexterity: " + (int)dexterity);

            var newStrength = (int)GUI.HorizontalSlider(new Rect(10, 50, 100, 30), (int)strength, 1f, PlayerSetting.MaxPoint - 2f);
            var newEndurance = (int)GUI.HorizontalSlider(new Rect(10, 90, 100, 30), (int)endurance, 1f, PlayerSetting.MaxPoint - 2f);
            var newDexterity = (int)GUI.HorizontalSlider(new Rect(10, 130, 100, 30), (int)dexterity, 1f, PlayerSetting.MaxPoint - 2f);

            if (Math.Abs(strength - newStrength) > Epsilon) gameGlobal.PlayerSetting[playerSettingsCurrent].Strength = newStrength;
            if (Math.Abs(endurance - newEndurance) > Epsilon) gameGlobal.PlayerSetting[playerSettingsCurrent].Endurance = newEndurance;
            if (Math.Abs(dexterity - newDexterity) > Epsilon) gameGlobal.PlayerSetting[playerSettingsCurrent].Dexterity = newDexterity;

            if (GUI.Button(new Rect(10, 200, 100, 30), "Next"))
            {
                playerSettingsCurrent++;
                playerClicks = 0;
                grid.RectColor = new Color(0f, 0f, 1f, .75f);
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
