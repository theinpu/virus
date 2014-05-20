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
            var minAgeRep = gameGlobal.PlayerSetting[playerSettingsCurrent].MinReproductiveAge;
            var maxAgeRep = gameGlobal.PlayerSetting[playerSettingsCurrent].MaxReproductiveAge;

            var lifeCycle = Mathf.Floor((maxAgeRep - minAgeRep)/5 + 0.5f);
            var unspentPoints = PlayerSetting.MaxPoint - lifeCycle - strength - endurance - dexterity;

            GUI.Label(new Rect(10, 30, 100, 20), "Points: " + (int)unspentPoints);
            GUI.Label(new Rect(10, 50, 150, 30), "Power: " + (int)strength);
            GUI.Label(new Rect(10, 90, 150, 30), "Vitality: " + (int)endurance);
            GUI.Label(new Rect(10, 130, 150, 30), "Speed: " + (int)dexterity);
            GUI.Label(new Rect(10, 170, 150, 30), "Rep age min: " + (int)minAgeRep + "%");
            GUI.Label(new Rect(10, 210, 150, 30), "Rep age max: " + (int)maxAgeRep + "%");

            var newStrength = (int)GUI.HorizontalSlider(new Rect(10, 70, 100, 30), (int)strength, 1f, PlayerSetting.MaxPoint - 18f);
            var newEndurance = (int)GUI.HorizontalSlider(new Rect(10, 110, 100, 30), (int)endurance, 1f, PlayerSetting.MaxPoint - 18f);
            var newDexterity = (int)GUI.HorizontalSlider(new Rect(10, 150, 100, 30), (int)dexterity, 1f, PlayerSetting.MaxPoint - 18f);
            var newMinAge = GUI.HorizontalSlider(new Rect(10, 190, 100, 30), (int)minAgeRep, 10f, 90f);
            var newMaxAge = GUI.HorizontalSlider(new Rect(10, 230, 100, 30), (int)maxAgeRep, 10f, 90f);

            if (Math.Abs(strength - newStrength) > Epsilon) gameGlobal.PlayerSetting[playerSettingsCurrent].Strength = newStrength;
            if (Math.Abs(endurance - newEndurance) > Epsilon) gameGlobal.PlayerSetting[playerSettingsCurrent].Endurance = newEndurance;
            if (Math.Abs(dexterity - newDexterity) > Epsilon) gameGlobal.PlayerSetting[playerSettingsCurrent].Dexterity = newDexterity;
            if (Math.Abs(minAgeRep - newMinAge) > Epsilon) gameGlobal.PlayerSetting[playerSettingsCurrent].MinReproductiveAge = newMinAge;
            if (Math.Abs(maxAgeRep - newMaxAge) > Epsilon) gameGlobal.PlayerSetting[playerSettingsCurrent].MaxReproductiveAge = newMaxAge;

            if (GUI.Button(new Rect(10, 250, 100, 30), "Next"))
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
