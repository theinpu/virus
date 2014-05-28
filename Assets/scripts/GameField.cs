﻿using System.Xml;
using UnityEngine;
using System.Collections;

public class GameField : MonoBehaviour {

	public int PlayerCount = 2;

	public GUIText[] Info;
	public int[] PlayerCellCount;

	public GameObject Cell;
	public int Width;
	public int Height;
	
	public int halfWidth;
	public int halfHeight;

	public float TimeScale = 1f;

	public VirusCell[] VirusGrid;

	private GameGlobalScript gameGlobal;

	// Use this for initialization
	void Start () {

		var gameGlobalObject = GameObject.Find("GameGlobal");
		gameGlobal = (GameGlobalScript)gameGlobalObject.GetComponent(typeof(GameGlobalScript));

	    Width = gameGlobal.GameSettings.FieldWidth;
	    Height = gameGlobal.GameSettings.FieldHeight;

		PlayerCellCount = new int[PlayerCount];
		for(int i = 0; i < 4; i++) {
			if(i >= PlayerCount) {
				Info[i].enabled = false;
			}
		}

		VirusGrid = new VirusCell[Height* Width];

		halfWidth = Width / 2;
		halfHeight = Height / 2;

		InitFirstPopulation();
		Time.timeScale = TimeScale;
	}
	
	// Update is called once per frame
	void Update () {
		int losers = 0;        
		for(int i = 0; i < PlayerCount; i++) {
			if(PlayerCellCount[i] == 0) losers++;
		}
		if(losers == PlayerCount - 1) {
			Application.LoadLevel("win");
		}
	}

	void OnGUI() {
		Time.timeScale = GUI.HorizontalSlider(new Rect(Screen.width/2 - 100, 10, 200, 50), Time.timeScale, 0.5F, 50.0F);
	    for (int i = 0, c = gameGlobal.PlayerCount; i < c; i++)
	    {
	        Info[i].text = "Population: " + PlayerCellCount[i]
	                       + "\nS:" + gameGlobal.PlayerSetting[i].Strength
	                       + "\nE:" + gameGlobal.PlayerSetting[i].Endurance
	                       + "\nD:" + gameGlobal.PlayerSetting[i].Dexterity;
	    }
	}

    public Point GetNeighbours(int x, int y, Point[] emptyCells, Point[] enemyCells, int player)
	{
		int i, j;
        Point neighbours;
        int enemyCellCount = 0;
        int emptyCellCount = 0;
		for(i = x - 1; i <= x + 1; i++){
			for(j = y - 1; j <= y + 1; j++){
				if(i == x && j == y) continue;
				if(i >= 0 && i < Width && j >= 0 && j < Height) {
					var testedCell = VirusGrid [j*Width + i];

                    if (testedCell.PlayerNumber == PlayerNumber.None)
                    {
                        emptyCells[emptyCellCount].X = testedCell.X;
                        emptyCells[emptyCellCount].Y = testedCell.Y;
                        emptyCellCount++;
                    }
                    else if ((int) testedCell.PlayerNumber != player)
                    {
                        enemyCells[enemyCellCount].X = testedCell.X;
                        enemyCells[enemyCellCount].Y = testedCell.Y;
                        enemyCellCount++;
                    }
				}
			}
		}

        neighbours.X = emptyCellCount;
        neighbours.Y = enemyCellCount;

        return neighbours;
	}

	public void AddCell (VirusCell cell)
	{
		var i = (int)cell.PlayerNumber;
		PlayerCellCount[i]++;
	}

	public void DecreaseCellCount (int i)
	{
		PlayerCellCount[i]--;
    }

	private void InitFirstPopulation() {
		int x, y;
		for(x = 0; x < Width; x++) {
			for(y = 0; y < Height; y++) {
			    NewCell(x, y);
			}
		}

	    CreatePlayerCell((int)PlayerNumber.One);
        CreatePlayerCell((int)PlayerNumber.Two);
	}

    private void NewCell(int x, int y)
    {
        var cell =
                (VirusCell)
                    ((GameObject)Instantiate(Cell, new Vector3(x - halfWidth, 0f, y - halfHeight), Quaternion.identity))
                        .GetComponent(typeof(VirusCell));

        cell.IsAlive = false;
        cell.PlayerNumber = PlayerNumber.None;
        cell.X = x;
        cell.Y = y;

        cell.Strength = 0;
        cell.Endurance = 0;
        cell.Dexterity = 0;

        cell.minReproductiveAge = 0;
        cell.maxReproductiveAge = 0;

        cell.GameField = this;

        VirusGrid[y * Width + x] = cell;
    }

    private void CreatePlayerCell(int id)
    {
        var playerSetting = gameGlobal.PlayerSetting[id];

        for (int i = 0; i < GameGlobalScript.MaxCells; i++)
        {

            var x = playerSetting.StartingPosition[i].X;
            var y = playerSetting.StartingPosition[i].Y;

            var cell = VirusGrid[y*Width + x];

            cell.PlayerNumber = (PlayerNumber) id;
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
