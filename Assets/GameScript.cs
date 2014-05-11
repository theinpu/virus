using UnityEngine;
using System.Collections;

public class GameScript : MonoBehaviour {
	
	public GameObject Cell;
	public int Width;
	public int Height;

	public float TimeScale = 1f;

	private VirusCell[,] virusGrid;

	// Use this for initialization
	void Start () {
		virusGrid = new VirusCell[Width, Height];
		InitFirstPopulation();
		Time.timeScale = TimeScale;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnGUI() {
		Time.timeScale = GUI.HorizontalSlider(new Rect(10, 10, 200, 30), Time.timeScale, 0.5F, 30.0F);
	}
    
    public Point[] GetFreeCells(int x, int y) {
		var points = new ArrayList();
		int i, j;
		for(i = x - 1; i <= x + 1; i++){
			for(j = y - 1; j <= y + 1; j++){
				if(i == x && j == y) continue;
				if(i >= 0 && i < Width && j >= 0 && j < Height) {
					if(virusGrid[i, j] == null) {
						points.Add(new Point(i, j));
					}
				}
			}
		}
		return (Point[])points.ToArray(typeof(Point));
	}

	public VirusCell[] GetEnemyCells(VirusCell damageDealer) {
		var cells = new ArrayList();
		var x = damageDealer.X;
		var y = damageDealer.Y;
		int i, j;
		for(i = x - 1; i <= x + 1; i++){
			for(j = y - 1; j <= y + 1; j++){
				if(i == x && j == y) continue;
				if(i >= 0 && i < Width && j >= 0 && j < Height) {
					var testedPoint = virusGrid [i, j];
					if(testedPoint != null) {
						if(testedPoint.PlayerNumber != damageDealer.PlayerNumber) {
							cells.Add (testedPoint);
						}
					}
				}
			}
		}
		return (VirusCell[])cells.ToArray(typeof(VirusCell));
	}

	public void AddCell (VirusCell cell)
	{
		cell.GameField = this;
		virusGrid[cell.X, cell.Y] = cell;
	}

	private void InitFirstPopulation() {
		var halfWidth = Width / 2;
		var halfHeight = Height / 2;
		for(int x = 0; x < Width; x++) {
			for(int y = 0; y < Height; y++) {
				VirusCell cell = null;
				if(x <= 0 && y == halfHeight) {
					cell = (VirusCell)((GameObject)Instantiate (Cell, new Vector3 (x - halfWidth, 0f, 0f), Quaternion.identity)).GetComponent (typeof(VirusCell));
					cell.PlayerNumber = PlayerNumber.One;
				}
				if(x >= Width - 1 && y == halfHeight) {
					cell = (VirusCell)((GameObject)Instantiate (Cell, new Vector3 (x - halfWidth - 1, 0f, 0f), Quaternion.identity)).GetComponent (typeof(VirusCell));
					cell.PlayerNumber = PlayerNumber.Two;
				}
				if(cell != null) {
					cell.GameField = this;
					cell.X = x;
					cell.Y = y;
				}
				virusGrid[x, y] = cell;
			}
		}
	}
}
