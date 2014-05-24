using UnityEngine;
using System.Collections;

public class GameTestPresets : MonoBehaviour
{

    private GameGlobalScript gameGlobal;

    private int playerOne = 0;
    private int playerTwo = 0;

    private string[] selStrings = { "Силовик", "Витальщик", "Декстерщик" };

    private PlayerSetting strengthPlayer;
    private PlayerSetting edurancePlayer;
    private PlayerSetting dexterityPlayer;

    private Point[] bottomLeftCorner = new Point[5];
    private Point[] topRightCorner = new Point[5];
    private int w = 15;
    private int h = 15;

    void Start()
    {
        var gameGlobalObject = GameObject.Find("GameGlobal");
        gameGlobal = (GameGlobalScript)gameGlobalObject.GetComponent(typeof(GameGlobalScript));

        strengthPlayer = new PlayerSetting()
        {
            Strength = 24,
            Endurance = 3,
            Dexterity = 3,
            MinReproductiveAge = 20,
            MaxReproductiveAge = 80
        };

        edurancePlayer = new PlayerSetting()
        {
            Strength = 3,
            Endurance = 24,
            Dexterity = 3,
            MinReproductiveAge = 20,
            MaxReproductiveAge = 80
        };

        dexterityPlayer = new PlayerSetting()
        {
            Strength = 3,
            Endurance = 3,
            Dexterity = 24,
            MinReproductiveAge = 20,
            MaxReproductiveAge = 80
        };

        bottomLeftCorner[0] = new Point(0, 0);
        bottomLeftCorner[1] = new Point(1, 0);
        bottomLeftCorner[2] = new Point(0, 1);
        bottomLeftCorner[3] = new Point(2, 1);
        bottomLeftCorner[4] = new Point(1, 2);

        topRightCorner[0] = new Point(w, h);
        topRightCorner[1] = new Point(w - 1, h);
        topRightCorner[2] = new Point(w, h - 1);
        topRightCorner[3] = new Point(w - 2, h - 1);
        topRightCorner[4] = new Point(w - 1, h - 2);
    }

    void OnGUI()
    {
        playerOne = GUI.SelectionGrid(new Rect(25, 25, 150, 150), playerOne, selStrings, 1);

        playerTwo = GUI.SelectionGrid(new Rect(200, 25, 150, 150), playerTwo, selStrings, 1);

        if (GUI.Button(new Rect(25, 200, 325, 50), "Play"))
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        gameGlobal.PlayerCount = 2;
        gameGlobal.GameSettings.FieldWidth = w + 1;
        gameGlobal.GameSettings.FieldHeight = h + 1;

        gameGlobal.InitPlayerSettings();

        switch (playerOne)
        {
            case 0:
                gameGlobal.PlayerSetting[0] = (PlayerSetting)strengthPlayer.Clone();
                break;
            case 1:
                gameGlobal.PlayerSetting[0] = (PlayerSetting)edurancePlayer.Clone();
                break;
            case 2:
                gameGlobal.PlayerSetting[0] = (PlayerSetting)dexterityPlayer.Clone();
                break;
        }

        switch (playerTwo)
        {
            case 0:
                gameGlobal.PlayerSetting[1] = (PlayerSetting)strengthPlayer.Clone();
                break;
            case 1:
                gameGlobal.PlayerSetting[1] = (PlayerSetting)edurancePlayer.Clone();
                break;
            case 2:
                gameGlobal.PlayerSetting[1] = (PlayerSetting)dexterityPlayer.Clone();
                break;
        }

        gameGlobal.PlayerSetting[0].StartingPosition = bottomLeftCorner;
        gameGlobal.PlayerSetting[1].StartingPosition = topRightCorner;

        Application.LoadLevel("gameScene");
    }
}