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
		map = new VirusCell[Width, Height];
		InitFirstPopulation();
		Time.timeScale = TimeScale;
	}
	
	// Update is called once per frame
	void Update () {
		//if(map == null) return;
		for(int x = 0; x < Width; x++) {
			for(int y = 0; y < Height; y++) {
				var neighborhoodCount = GetNeighborhoodCount(x, y);
				if(map[x, y] == null) {
					if(neighborhoodCount < 3 && neighborhoodCount >= 1) {
						TryReproduce(x, y);
					}
				} 
				else {
					if(neighborhoodCount > 3) {
						map[x, y].Die();
						map[x, y] = null;
					}
				}
			}
		}
	}

	private void InitFirstPopulation() {
		var halfWidth = Width / 2;
		var halfHeight = Height / 2;
		for(int x = 0; x < Width; x++) {
			for(int y = 0; y < Height; y++) {
				map [x, y] = null;
				if(x <= 1 && y == halfHeight) {
					map [x, y] = (VirusCell)((GameObject)Instantiate (Cell, new Vector3 (x - halfWidth, 0f, 0f), Quaternion.identity)).GetComponent(typeof(VirusCell));
					map[x, y].PlayerNumber = PlayerNumber.One;
				}
				if(x >= Width - 2 && y == halfHeight) {
					map [x, y] = (VirusCell)((GameObject)Instantiate (Cell, new Vector3 (x - halfWidth - 1, 0f, 0f), Quaternion.identity)).GetComponent(typeof(VirusCell));
					map[x, y].PlayerNumber = PlayerNumber.Two;
				}
			}
		}
	}

	private void TryReproduce (int x, int y)
	{
		var parents = new ArrayList();
		for(int i = x - 1; i <= x + 1; i++) {
			for(int j = y - 1; j <= y + 1; j++) {
				if(i >= 0 && i < Width && j >= 0 && j < Height && i != x && j != y) {
					if(map[i, j] != null) {
						if(map[i, j].CanReproduce) {
							parents.Add(map[i, j]);
						}
					}
				}
			}
		}
		if(parents.Count > 0) {
			var newCell = BirnCell(x, y);
			for(int i = 0; i < parents.Count; i++) {
				newCell.Mutate((VirusCell)parents[i]);
			}
		}
	}

	private int GetNeighborhoodCount(int x, int y) {
		var neighborhoodCount = 0;
		for(int i = x - 1; i <= x + 1; i++) {
			for(int j = y - 1; j <= y + 1; j++) {
				if(i >= 0 && i < Width && j >= 0 && j < Height && i != x && j != y) {
					if(map[i, j] != null) {
						neighborhoodCount++;
					}
				}
			}
		}
		return neighborhoodCount;
	}

	private VirusCell BirnCell (int x, int y)
	{
		var halfWidth = Width / 2f;
		var halfHeight = Height / 2f;
		var childCell = (VirusCell)((GameObject)Instantiate (Cell, new Vector3 (x - halfWidth, 0f, y - halfHeight), Quaternion.identity)).GetComponent(typeof(VirusCell));
		map[x, y] = childCell;
		return childCell;
	}
}
