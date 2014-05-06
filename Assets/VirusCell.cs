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
	private Vector2 reproductiveAgeBounds;

	private float mutationFactor;

	private float reproductiveTimer = 0;
	private float attackTimer = 0;
	private float healthChangeTimer = 0;

	#endregion

	#region Properties

	public float Health { get{ return health; }}
	public float Age { get { return age; }}

	#endregion

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
