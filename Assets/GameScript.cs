using UnityEngine;
using System.Collections;

public class GameScript : MonoBehaviour {

	public GameObject Cell;
	public int Width;
	public int Height;

	public float TimeScale = 1f;

	private VirusCell[,] map;

	// Use this for initialization
	void Start () {
		Debug.Log("Start");
		map = new VirusCell[Width, Height];
		InitFirstPopulation();
		Time.timeScale = TimeScale;
	}
	
	// Update is called once per frame
	void Update () {
		//if(map == null) return;
		for(int x = 0; x < Width; x++) {
			for(int y = 0; y < Height; y++) {
				if(map[x, y] != null) {
					var neighborhoodCount = GetNeighborhoodCount(x, y);
					Debug.Log(neighborhoodCount);
					/*if(map[x, y].CanReproduce && neighborhoodCount < 3) {
						TryReproduce(x, y, map[x, y]);
					}
					/*if(neighborhoodCount > 6) {
						map[x, y].Die();
						map[x, y] = null;
					}*/
				}
			}
		}
	}

	private void InitFirstPopulation() {
		var halfWidth = Width / 2f;
		var halfHeight = Height / 2f;
		for(int x = 0; x < Width; x++) {
			for(int y = 0; y < Height; y++) {
				map [x, y] = null;
				if(x == 0 && y == halfHeight) {
					map [x, y] = (VirusCell)((GameObject)Instantiate (Cell, new Vector3 (-halfWidth, 0f, 0f), Quaternion.identity)).GetComponent(typeof(VirusCell));
					map[x, y].PlayerNumber = PlayerNumber.One;
				}
				if(x == Width - 1 && y == halfHeight) {
					map [x, y] = (VirusCell)((GameObject)Instantiate (Cell, new Vector3 (halfWidth, 0f, 0f), Quaternion.identity)).GetComponent(typeof(VirusCell));
					map[x, y].PlayerNumber = PlayerNumber.Two;
				}
			}
		}
	}

	private void TryReproduce (int x, int y, VirusCell cell)
	{
		var freeCells = new ArrayList();
		//Debug.Log("try reproduce");
		for(int i = x - 1; i <= x + 1; i++) {
			for(int j = y - 1; j <= y + 1; j++) {
				if(i >= 0 && i < Width && j >= 0 && j < Height) {
					if(map[i, j] == null) {
						freeCells.Add(new Vector2(i, j));
					}
				}
			}
		}
		if(freeCells.Count > 0) {
			var cellId = Random.Range(0, freeCells.Count);
			var newX = (int)((Vector2)freeCells[cellId]).x;
			var newY = (int)((Vector2)freeCells[cellId]).y;
			BirnCell(newX, newY, cell);
		}
	}

	private int GetNeighborhoodCount(int x, int y) {
		var neighborhoodCount = 0;
		for(int i = x - 1; i <= x + 1; i++) {
			for(int j = y - 1; j <= y + 1; j++) {
				if(i >= 0 && i < Width && j >= 0 && j < Height) {
					if(map[i, j] != null) {
						neighborhoodCount++;
					}
				}
			}
		}
		return neighborhoodCount;
	}

	private void BirnCell (int x, int y, VirusCell parentCell)
	{
		//Debug.Log("birth at [" + x.ToString() + ", " + y.ToString() + "]");
		var halfWidth = Width / 2f;
		var halfHeight = Height / 2f;
		var childCell = (VirusCell)((GameObject)Instantiate (Cell, new Vector3 (x - halfWidth, 0f, y - halfHeight), Quaternion.identity)).GetComponent(typeof(VirusCell));
		childCell.Mutate(parentCell);
		map[x, y] = childCell;
	}
}
