using UnityEngine;
using System.Collections;

public class VirusCell : MonoBehaviour {

	#region Public fields

	public float Endurance = 1;
	public float Strength = 1;
	public float Dexterity = 1;
	public float ReproductiveSpeed = 1f;

	public bool CanReproduce = false;
	public bool CanChangeHealth = true;
	public bool CanAttack = false;

	#endregion

	#region Fields

	private float health;
	private float healthChangeRate = 0;

	private float age = 0;
	private float maxAge;
	private float agingChangeRate = 1f;
	private Vector2 reproductiveAgeBounds;

	private float mutationFactor = 1f;

	private float reproductiveTimer = 0;
	private float attackTimer = 0;
	private float healthChangeTimer = 0;
	private float agingTimer = 0;

	private PlayerNumber playerNumber;

	private int parentCount = 0;

	#endregion

	#region Properties

	public float Health { get{ return health; }}
	public float Age { get { return age; }}

	public PlayerNumber PlayerNumber { 
		get { 
			return playerNumber; 
		} 
		set { 
			playerNumber = value;
			switch(playerNumber) {
				case PlayerNumber.One:
					SetColor(Color.red);
					break;
				case PlayerNumber.Two:
					SetColor(Color.blue);
					break;
			case PlayerNumber.Three:
					SetColor(Color.green);
					break;
				case PlayerNumber.Four:
					SetColor(Color.yellow);
					break;
			}
		} 
	}

	public GameScript GameField {get;set;}
	public int X {get;set;}
	public int Y {get;set;}

	#endregion

	// Use this for initialization
	void Start () {
		maxAge = Strength + Endurance * 2f;
		health = Strength * 0.5f + Endurance * 3f;
		reproductiveAgeBounds = new Vector2(1f, maxAge - 1f);
	}
	
	// Update is called once per frame
	void Update () {
		if(age > maxAge) {
			Die();
			return;
		}
		if(agingTimer > 1f) {
			agingTimer = 0;
			age += agingChangeRate;
		}
		agingTimer += Time.deltaTime;

		var freeCells = GameField.GetFreeCells(X, Y);
		//Debug.Log(freeCells.Length);
		if(freeCells.Length > 5 && freeCells.Length < 8) {
			if(reproductiveTimer > ReproductiveSpeed && age > reproductiveAgeBounds.x && age < reproductiveAgeBounds.y) {
				reproductiveTimer = 0;
				Reproduce(freeCells);
			}
			reproductiveTimer += Time.deltaTime;
		}
	}

	void Reproduce (Point[] freeCells)
	{
		var id = Random.Range(0, freeCells.Length);
		var point = freeCells[id];
		var halfWidth = GameField.Width / 2;
		var halfHeight = GameField.Height / 2;

		var cell = (VirusCell)((GameObject)Instantiate (GameField.Cell, new Vector3 (point.X - halfWidth, 0f, point.Y - halfHeight), Quaternion.identity)).GetComponent (typeof(VirusCell));
		cell.PlayerNumber = playerNumber;
		cell.X = point.X;
		cell.Y = point.Y;
		GameField.AddCell(cell);
	}

	public void Die ()
	{
		Destroy(gameObject);
	}

	private void SetColor(Color color) {
		renderer.materials[0].color = color;
	}
}
