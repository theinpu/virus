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

	private float age;
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

	#endregion

	// Use this for initialization
	void Start () {
		if(parentCount > 0) {
			Strength = (float)Strength/parentCount;
			Endurance /= (float)Endurance/parentCount;
			Dexterity /= (float)Dexterity/parentCount;
		}

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
			if(reproductiveTimer > ReproductiveSpeed) {
				reproductiveTimer = 0;
				CanReproduce = true;
			} else {
				CanReproduce = false;
			}
			reproductiveTimer += Time.deltaTime;
		} else {
			CanReproduce = false;
		}
	}

	public void Mutate (VirusCell parentCell)
	{
		parentCount++;
		PlayerNumber = parentCell.PlayerNumber;
		mutationFactor = parentCell.mutationFactor;
		if(Random.Range(0f, 1f) > 0.995f) {
			mutationFactor += Random.Range(-mutationFactor/2f, mutationFactor/2f);
		}

		if(Strength == 1) 
			Strength = parentCell.Strength;
		else
			Strength += parentCell.Strength * mutationFactor;

		if(Endurance == 1) 
			Endurance = parentCell.Endurance;
		else
			Endurance += parentCell.Endurance * mutationFactor;

		if(Dexterity == 1) 
			Dexterity = parentCell.Dexterity;
		else
			Dexterity += parentCell.Dexterity * mutationFactor;
	}
	
	public void Die ()
	{
		Destroy(gameObject);
	}

	private void SetColor(Color color) {
		renderer.materials[0].color = color;
	}
}
