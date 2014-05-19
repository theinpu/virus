﻿using UnityEngine;
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

	public VirusCell[] virusGrid;

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

		virusGrid = new VirusCell[Height* Width];

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
	}
    
	public Neighbours GetNeighbours(int x, int y)
	{
		int i, j;
		Neighbours neighbours = new Neighbours{};
		var freeCells = new ArrayList();
		var enemyCells = new ArrayList();

		for(i = x - 1; i <= x + 1; i++){
			for(j = y - 1; j <= y + 1; j++){
				if(i == x && j == y) continue;
				if(i >= 0 && i < Width && j >= 0 && j < Height) {
					var testedPoint = virusGrid [j*Width + i];
					Point point = new Point(i, j);
					if(testedPoint == null) {
						freeCells.Add(point);
					}
					else {
						if(testedPoint.PlayerNumber != virusGrid[y*Width + x].PlayerNumber) {
							enemyCells.Add(point);
						}
					}
				}
			}
		}

		//-----------------------------------------------------------------------------//
		// TODO ИЗБАВИТЬСЯ ОТ ЭТОЙ ГРЁБАНОЙ КОНВЕРТАЦИИ из ArrayList в Point[], 
		// В ИДЕАЛЕ ВООБЩЕ РАБОТАТЬ НЕ С ПОИНТАМИ А С ИНТОМ-Смещением ТИПА X * WIDTH + Y 
		// НО ДЛЯ REPRODUCE НАМ НУЖНЫ КООРД-ТЫ x y z для Vector3d - не смог это пофиксить
		// 
		// конструкция типа Point point = new Point(i, j); в теле цикла жутко убога
		// и маст дай. ЗЫ вроде выиграл пару фпс :)
		//-----------------------------------------------------------------------------//

		neighbours.FreeCells = (Point[]) freeCells.ToArray(typeof(Point));
		neighbours.EnemyCells = (Point[]) enemyCells.ToArray(typeof(Point));

		return neighbours;
	}

	public void AddCell (VirusCell cell)
	{
		cell.GameField = this;
		virusGrid[cell.Y*Width + cell.X] = cell;
		var i = (int)cell.PlayerNumber;
		PlayerCellCount[i]++;
		Info[i].text = "Population: " + PlayerCellCount[i];
	}

	public void decreaseCellCount (int playerNumber)
	{
		PlayerCellCount[playerNumber]--;
		Info[playerNumber].text = "Population: " + PlayerCellCount[playerNumber];
	}

	private void InitFirstPopulation() {
		int x, y;
		for(x = 0; x < Width; x++) {
			for(y = 0; y < Height; y++) {
			    virusGrid[y*Width + x]   = null;
			}
		}

	    CreatePlayerCell((int)PlayerNumber.One);
        CreatePlayerCell((int)PlayerNumber.Two);
	}

    private void CreatePlayerCell(int id)
    {
        var playerSetting = gameGlobal.PlayerSetting[id];

        for (int i = 0; i < GameGlobalScript.MaxCells; i++)
        {

            var x = playerSetting.StartingPosition[i].X;
            var y = playerSetting.StartingPosition[i].Y;
            var cell =
                (VirusCell)
                    ((GameObject) Instantiate(Cell, new Vector3(x - halfWidth, 0f, y - halfHeight), Quaternion.identity))
                        .GetComponent(typeof (VirusCell));

            cell.PlayerNumber = (PlayerNumber) id;
            cell.X = x;
            cell.Y = y;

            cell.Strength = playerSetting.Strength;
            cell.Endurance = playerSetting.Endurance;
            cell.Dexterity = playerSetting.Dexterity;

            AddCell(cell);

            virusGrid[y*Width + x] = cell;
        }
    }
}