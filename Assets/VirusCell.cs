using UnityEngine;
using System.Collections;

public class VirusCell : MonoBehaviour {

	#region Consts

	const float MaxEndurance = 30f;
	const float MaxStrength = 30f;
	const float MaxDexterity = 30f;

	#endregion

	#region Public fields

	public float Endurance = 1;
	public float Strength = 1;
	public float Dexterity = 1;
	public float ReproductiveSpeed = 1f;

	public bool CanReproduce = false;
	public bool CanChangeHealth = true;
	public bool CanAttack = false;

	public AnimationCurve ReproductionStrengthCurve;

	#endregion

	#region Fields

	private float health;
	private float maxHealth;

	private float age = 0;
	private float maxAge;
	private float reproductiveAgility;
	private float agingChangeRate = 1f;
	private Vector2 reproductiveAgeBounds;

	private float mutationFactor = 1f;

	private float reproductiveTimer = 0;
	private float attackTimer = 0;
	private float agingTimer = 0;

	private float damage;
	private float attackSpeed;

	private PlayerNumber playerNumber;

	private float reproductiveAges;

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
		maxHealth = health;

		attackSpeed = 1f;

		reproductiveAgility = (Dexterity / (maxAge + health));
		reproductiveAgeBounds = new Vector2(maxAge * reproductiveAgility, maxAge * (1f - reproductiveAgility));
		reproductiveAges = reproductiveAgeBounds.y - reproductiveAgeBounds.x;
	}
	
	// Update is called once per frame
	void Update () {
		if(age > maxAge || health <= 0f) {
			Die();
			return;
		}
		if(agingTimer > 1f) {
			agingTimer = 0f;
			age += agingChangeRate;

			var ageCoef = Mathf.Clamp(age, reproductiveAgeBounds.x, reproductiveAgeBounds.y) - reproductiveAgeBounds.x;
			var timeValue = ageCoef / reproductiveAges;
			ReproductiveSpeed = (ReproductionStrengthCurve.Evaluate(timeValue) * health/maxHealth);
		}
		agingTimer += Time.deltaTime;

		var freeCells = GameField.GetFreeCells(X, Y);
		if(freeCells.Length >= 3 && freeCells.Length < 8) {
			if(reproductiveTimer > (2f - ReproductiveSpeed) && age > reproductiveAgeBounds.x && age < reproductiveAgeBounds.y) {
				reproductiveTimer = 0;
				Reproduce(freeCells);
			}
			reproductiveTimer += Time.deltaTime;
		}

		var enemyCells = GameField.GetEnemyCells(this);
		if(enemyCells.Length > 0) {
			if(attackTimer > attackSpeed) {
				Attack(enemyCells);
			}
			attackTimer += Time.deltaTime;
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

	void Attack (VirusCell[] enemyCells)
	{
		var id = Random.Range(0, enemyCells.Length);
		var ageCoef = Mathf.Clamp(age, reproductiveAgeBounds.x, reproductiveAgeBounds.y) - reproductiveAgeBounds.x;
		var timeValue = ageCoef / reproductiveAges;
		var strengthCoef = ReproductionStrengthCurve.Evaluate(timeValue);
		damage = (Strength * 2f + Dexterity) * strengthCoef;
		if(damage > 0f && !float.IsNaN(damage)) {
			enemyCells[id].TakeDamage(damage);
		}
	}

	public void TakeDamage(float damage) {
		health -= damage;
	}

	public void Die ()
	{
		Destroy(gameObject);
	}

	private void SetColor(Color color) {
		renderer.materials[0].color = color;
	}
}