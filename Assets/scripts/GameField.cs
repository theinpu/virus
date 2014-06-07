using System;
using UnityEngine;
using System.Collections;

public class GameField : MonoBehaviour
{

    public int PlayerCount = 2;

    public GUIText[] Info;
    public int[] PlayerCellCount;

    public GameObject Cell;
    public int Width;
    public int Height;

    public float halfWidth;
    public float halfHeight;

    public VirusCell[] VirusGrid;

    private GameGlobalScript gameGlobal;

    // Use this for initialization
    void Start()
    {

        var gameGlobalObject = GameObject.Find("GameGlobal");
        gameGlobal = (GameGlobalScript)gameGlobalObject.GetComponent(typeof(GameGlobalScript));

        Width = gameGlobal.GameSettings.FieldWidth;
        Height = gameGlobal.GameSettings.FieldHeight;

        var dist = Mathf.Max(Width, Height);
        Camera.main.transform.position = new Vector3(0, dist, 0);

        PlayerCount = gameGlobal.PlayerCount;
        PlayerCellCount = new int[PlayerCount];
        for (int i = 0; i < 4; i++)
        {
            if (i >= PlayerCount)
            {
                Info[i].enabled = false;
            }
        }

        VirusGrid = new VirusCell[Height * Width];

        halfWidth = Width / 2f;
        halfHeight = Height / 2f;

        InitFirstPopulation();
        Time.timeScale = gameGlobal.GameSettings.TimeScale;
    }

    // Update is called once per frame
    void Update()
    {
        int losers = 0;
        for (int i = 0; i < PlayerCount; i++)
        {
            if (PlayerCellCount[i] == 0) losers++;
        }
        if (losers == PlayerCount - 1)
        {
            Application.LoadLevel("win");
        }
    }

    void OnGUI()
    {
        for (int i = 0, c = gameGlobal.PlayerCount; i < c; i++)
        {
            Info[i].text = "Population: " + PlayerCellCount[i];
            /*+ "\nS:" + gameGlobal.PlayerSetting[i].Strength
                           + "\nE:" + gameGlobal.PlayerSetting[i].Endurance
                           + "\nD:" + gameGlobal.PlayerSetting[i].Dexterity;*/
        }
    }

    public Neighbours GetNeighbours(int x, int y)
    {
        int i, j;
        int free = 0;
        int enemy = 0;

        Neighbours neighbours;

        neighbours.FreeCells = new Point[8];
        neighbours.EnemyCells = new Point[8];

        for (i = x - 1; i <= x + 1; i++)
        {
            for (j = y - 1; j <= y + 1; j++)
            {
                if (i == x && j == y) continue;
                if (i >= 0 && i < Width && j >= 0 && j < Height)
                {
                    var testedPoint = VirusGrid[j * Width + i];
                    var point = new Point(i, j);
                    if (testedPoint.PlayerNumber == PlayerNumber.None)
                    {
                        neighbours.FreeCells[free] = point;
                        free++;
                    }
                    else
                    {
                        if (testedPoint.PlayerNumber != VirusGrid[y * Width + x].PlayerNumber)
                        {
                            neighbours.EnemyCells[enemy] = point;
                            enemy++;
                        }
                    }
                }
            }
        }
        neighbours.freeCells = free;
        neighbours.enemyCells = enemy;

        return neighbours;
    }

    public void AddCell(VirusCell cell)
    {
        var i = (int)cell.PlayerNumber;
        PlayerCellCount[i]++;
    }

    public void DecreaseCellCount(int i)
    {
        PlayerCellCount[i]--;
    }

    private void InitFirstPopulation()
    {
        int x, y;
        for (x = 0; x < Width; x++)
        {
            for (y = 0; y < Height; y++)
            {
                NewCell(x, y);
            }
        }

        for (var i = 0; i < gameGlobal.PlayerCount; i++)
        {
            CreatePlayerCell(i);
        }
    }

    private void NewCell(float x, float y)
    {
        var cell =
                (VirusCell)
                    ((GameObject)Instantiate(Cell, new Vector3(x - halfWidth + 0.5f, 0f, y - halfHeight + 0.5f), Quaternion.identity))
                        .GetComponent(typeof(VirusCell));

        cell.IsAlive = false;
        cell.PlayerNumber = PlayerNumber.None;
        cell.X = (int)x;
        cell.Y = (int)y;

        cell.Strength = 0;
        cell.Endurance = 0;
        cell.Dexterity = 0;

        cell.minReproductiveAge = 0;
        cell.maxReproductiveAge = 0;

        cell.GameField = this;

        VirusGrid[cell.Y * Width + cell.X] = cell;
    }

    private void CreatePlayerCell(int id)
    {
        var playerSetting = gameGlobal.PlayerSetting[id];

        for (int i = 0; i < GameGlobalScript.MaxCells; i++)
        {

            var x = playerSetting.StartingPosition[i].X;
            var y = playerSetting.StartingPosition[i].Y;

            var cell = VirusGrid[y * Width + x];

            cell.PlayerNumber = (PlayerNumber)id;
            cell.X = x;
            cell.Y = y;

            cell.Strength = playerSetting.Strength;
            cell.Endurance = playerSetting.Endurance;
            cell.Dexterity = playerSetting.Dexterity;

            cell.minReproductiveAge = playerSetting.MinReproductiveAge;
            cell.maxReproductiveAge = playerSetting.MaxReproductiveAge;

            AddCell(cell);
            cell.IsAlive = true;
            cell.ResetParams();
        }
    }
}
