using System;
using UnityEngine;

public class GameLobby : MonoBehaviour
{
    public GUISkin Skin;

    private GameGlobalScript gameGlobal;
    private GridOverlay grid;

    private int state = 0;
    private int playerSettingsCurrent = 0;
    private int playerClicks = 0;

    private const float Epsilon = 0.0001f;

    private int gameSpeed = 1;

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
        if (state != 1) return;
        if (Input.GetMouseButtonUp(0) && grid.IsInField())
        {
            if (grid.Occupy(playerColors[playerSettingsCurrent]))
            {
                if (grid.RemovePoint())
                {
                    playerClicks--;
                }
            }
            else
            {
                if (playerClicks < GameGlobalScript.MaxCells)
                {
                    if (grid.AddPoint(playerColors[playerSettingsCurrent]))
                    {
                        gameGlobal.PlayerSetting[playerSettingsCurrent].StartingPosition[playerClicks] = grid.FieldPoint;
                        playerClicks++;
                    }
                }
            }
        }
    }

    void OnGUI()
    {
        //#if UNITY_ANDROID
        GUI.skin = Skin;
        //#endif
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
        GUILayout.BeginArea(new Rect(10, 10, 200, Screen.height - 20));
        GUILayout.BeginVertical();
        GUILayout.Label("Players: " + gameGlobal.PlayerCount);
        gameGlobal.PlayerCount = (int)GUILayout.HorizontalSlider(gameGlobal.PlayerCount, 2, 4);

        GUILayout.Label("Game field width: " + gameGlobal.GameSettings.FieldWidth);
        gameGlobal.GameSettings.FieldWidth = (int)GUILayout.HorizontalSlider(gameGlobal.GameSettings.FieldWidth, 8, 42);

        GUILayout.Label("Game field height: " + gameGlobal.GameSettings.FieldHeight);
        gameGlobal.GameSettings.FieldHeight = (int)GUILayout.HorizontalSlider(gameGlobal.GameSettings.FieldHeight, 8, 42);

        var speed = "Normal";
        if (gameSpeed == 1) speed = "Fast";
        if (gameSpeed == 2) speed = "Faster";
        if (gameSpeed == 3) speed = "Fastest";
        GUILayout.Label("Game speed: " + speed);
        gameSpeed = (int)GUILayout.HorizontalSlider(gameSpeed, 0, 3);
        gameGlobal.GameSettings.TimeScale = (gameSpeed + 1) * 10f;

        var dist = Mathf.Max(gameGlobal.GameSettings.FieldWidth, gameGlobal.GameSettings.FieldHeight) + 3f;
        Camera.main.transform.position = new Vector3(0, dist, 0);
        grid.Reset();
        var middle = 210f + (Screen.width - 210) / 2f;
        var p = Camera.main.ScreenToWorldPoint(new Vector3(middle, Screen.height / 2f, dist));
        Camera.main.transform.position = new Vector3(-p.x, dist, 0);

        if (GUILayout.Button("Next"))
        {
            gameGlobal.InitPlayerSettings();

            state++;
            grid.Visible = true;

            grid.RectColor = playerColors[0];
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    void DrawPlayerParamsScreen()
    {
        GUILayout.BeginArea(new Rect(10, 10, 200, Screen.height - 20));
        GUILayout.BeginVertical();
        if (playerSettingsCurrent < gameGlobal.PlayerCount)
        {
            GUILayout.Label("Player " + (playerSettingsCurrent + 1) + " settings");
            var strength = gameGlobal.PlayerSetting[playerSettingsCurrent].Strength;
            var endurance = gameGlobal.PlayerSetting[playerSettingsCurrent].Endurance;
            var dexterity = gameGlobal.PlayerSetting[playerSettingsCurrent].Dexterity;
            var minAgeRep = gameGlobal.PlayerSetting[playerSettingsCurrent].MinReproductiveAge;
            var maxAgeRep = gameGlobal.PlayerSetting[playerSettingsCurrent].MaxReproductiveAge;

            var lifeCycle = Mathf.Floor((maxAgeRep - minAgeRep) / PlayerSetting.BoundsMultiplier + 0.5f);
            var unspentPoints = PlayerSetting.MaxPoint - lifeCycle - strength - endurance - dexterity;

            GUILayout.Label("Cells left: " + (5 - playerClicks));

            GUILayout.Label("Power: " + (int)strength);
            var newStrength = (int)GUILayout.HorizontalSlider((int)strength, 1f, PlayerSetting.MaxStat);

            GUILayout.Label("Vitality: " + (int)endurance);
            var newEndurance = (int)GUILayout.HorizontalSlider((int)endurance, 1f, PlayerSetting.MaxStat);

            GUILayout.Label("Speed: " + (int)dexterity);
            var newDexterity = (int)GUILayout.HorizontalSlider((int)dexterity, 1f, PlayerSetting.MaxStat);

            GUILayout.Label("Rep age min: " + (int)minAgeRep + "%");
            var newMinAge = (int)GUILayout.HorizontalSlider((int)minAgeRep, 10f, 90f);

            GUILayout.Label("Rep age max: " + (int)maxAgeRep + "%");
            var newMaxAge = (int)GUILayout.HorizontalSlider((int)maxAgeRep, 10f, 90f);

            if (Math.Abs(strength - newStrength) > Epsilon) gameGlobal.PlayerSetting[playerSettingsCurrent].Strength = newStrength;
            if (Math.Abs(endurance - newEndurance) > Epsilon) gameGlobal.PlayerSetting[playerSettingsCurrent].Endurance = newEndurance;
            if (Math.Abs(dexterity - newDexterity) > Epsilon) gameGlobal.PlayerSetting[playerSettingsCurrent].Dexterity = newDexterity;
            if (Math.Abs(minAgeRep - newMinAge) > Epsilon) gameGlobal.PlayerSetting[playerSettingsCurrent].MinReproductiveAge = newMinAge;
            if (Math.Abs(maxAgeRep - newMaxAge) > Epsilon) gameGlobal.PlayerSetting[playerSettingsCurrent].MaxReproductiveAge = newMaxAge;

            gameGlobal.PlayerSetting[playerSettingsCurrent].Normalize();

            var nextButtonText = "Next";
            if (playerSettingsCurrent == gameGlobal.PlayerCount - 1)
            {
                nextButtonText = "Start Game";
            }

            if (playerClicks < 5) GUI.enabled = false;
            if (GUILayout.Button(nextButtonText))
            {
                if (playerClicks < 5) return;

                playerSettingsCurrent++;
                playerClicks = 0;
                if (playerSettingsCurrent < gameGlobal.PlayerCount)
                {
                    grid.RectColor = playerColors[playerSettingsCurrent];
                }
            }
            GUI.enabled = true;
        }
        else
        {
            grid.Visible = false;
            Application.LoadLevel("gameScene");
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    void DrawStartPositionSelect()
    {
        if (GUI.Button(new Rect(10, 10, 100, 30), "Start game"))
        {
            Application.LoadLevel("gameScene");
        }
    }
}
