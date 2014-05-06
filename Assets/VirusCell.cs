using UnityEngine;
using System.Collections;

public class VirusCell : MonoBehaviour {

	#region Public fields

	public float Endurance = 1;
	public float Strength = 1;
	public float Dexterity = 1;

	public bool CanReproduce = false;
	public bool CanChangeHealth = true;
	public bool CanAttack = false;

	#endregion

	#region Fields

	private float health;
	private float healthChangeRate = 0;

	private float age;
	private float maxAge;
	private float agingChangeRate = 1f;
	private Vector2 reproductiveAgeBounds;

	private float mutationFactor;

	private float reproductiveTimer = 0;
	private float attackTimer = 0;
	private float healthChangeTimer = 0;
	private float agingTimer = 0;

	private PlayerNumber playerNumber;

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

	#endregion

	// Use this for initialization
	void Start () {
		maxAge = Strength + Endurance * 2f;
		reproductiveAgeBounds = new Vector2(1f, maxAge - 1f);
	}
	
	// Update is called once per frame
	void Update () {
		if(age > maxAge) {
			Die();
		} else {
			if(agingTimer > 1f) {
				agingTimer = 0;
				age += agingChangeRate;
			}
			agingTimer += Time.deltaTime;
		}
		if(reproductiveAgeBounds.x <= age && age <= reproductiveAgeBounds.y) {
			CanReproduce = true;
		} else {
			CanReproduce = false;
		}
	}

	public void Mutate (VirusCell parentCell)
	{
		Strength = parentCell.Strength;
		Endurance = parentCell.Endurance;
		Dexterity = parentCell.Dexterity;
		PlayerNumber = parentCell.PlayerNumber;
	}
	
	public void Die ()
	{
		Destroy(gameObject);
	}

	private void SetColor(Color color) {
		renderer.materials[0].color = color;
	}
}
