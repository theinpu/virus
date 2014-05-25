using UnityEngine;
using System.Collections;

public class TestVirusCell : MonoBehaviour
{

    #region Consts

    const float MaxEndurance = 30f;
    const float MaxStrength = 30f;
    const float MaxDexterity = 30f;

    private const float AgingChangeRate = 1f;

    #endregion

    #region Public fields

    public float Endurance = 1;
    public float Strength = 1;
    public float Dexterity = 1;
    public float ReproductiveSpeed = 1f;
    public float minReproductiveAge = 20f;
    public float maxReproductiveAge = 80f;

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
    private Vector2 reproductiveAgeBounds;

    private float mutationFactor = 1f;

    private float reproductiveTimer = 0;
    private float attackTimer = 0;
    private float agingTimer = 0;

    private float damage;
    private float attackSpeed;

    private PlayerNumber playerNumber;
    private Neighbours neighbours;

    private float reproductiveAges;
    private int count;

    #endregion

    #region Properties

    public float Health { get { return health; } }
    public float Age { get { return age; } }

    public int X { get; set; }
    public int Y { get; set; }

    #endregion

    // Use this for initialization
    void Start()
    {
        maxAge = (Endurance * 2f + Strength + Dexterity) / 3f;
        health = Endurance * 5f + (Strength / Dexterity);
        maxHealth = health;

        attackSpeed = 1f / (Strength / 10f + Dexterity);

        reproductiveAgeBounds = new Vector2(maxAge * (minReproductiveAge / 100), maxAge * (maxReproductiveAge / 100));
        reproductiveAges = reproductiveAgeBounds.y - reproductiveAgeBounds.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (age >= maxAge)
        {
            Debug.Log("Die");
            return;
        }
        age += Time.deltaTime;

        if (attackTimer > attackSpeed)
        {
            count++;
            attackTimer = 0;
        }
        attackTimer += Time.deltaTime;
    }

    private void TryReproduce()
    {
        reproductiveTimer += Time.deltaTime;

        var ageCoef = Mathf.Clamp(age, reproductiveAgeBounds.x, reproductiveAgeBounds.y) - reproductiveAgeBounds.x;
        var timeValue = ageCoef / reproductiveAges;
        var reproductiveAge = (maxReproductiveAge - minReproductiveAge) / 100f;
        ReproductiveSpeed = reproductiveAge / (Dexterity * 5f);
        ReproductiveSpeed *= maxAge;
        ReproductiveSpeed *= 1f/(health/maxHealth);
        ReproductiveSpeed /= ReproductionStrengthCurve.Evaluate(timeValue);

        if (reproductiveTimer > ReproductiveSpeed)
        {
            count++;            
            reproductiveTimer = 0f;
        }
    }
}