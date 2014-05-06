using UnityEngine;
using System.Collections;

public class GameScript : MonoBehaviour {

	public GameObject Cell;
	public int Width = 64;
	public int Height = 64;
	private GameObject[,] map;

	// Use this for initialization
	void Start () {
		map = new GameObject[Width, Height];
		var halfWidth = Width / 2f;
		var halfHeight = Height / 2f;
		for(int x = 0; x < Width; x++) {
			for(int y = 0; y < Height; y++) {
				var gameObject = (GameObject)Instantiate (Cell, new Vector3 (x - halfWidth, 0f, y - halfHeight), Quaternion.identity);
				gameObject.renderer.materials[0].color = new Color((float)x/Width, (float)y/Height, ((float)(x + y)/(Width + Height))/1.5f);
				map [x, y] = gameObject;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
