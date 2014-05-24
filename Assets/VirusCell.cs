using UnityEngine;
using System.Collections;

public class VirusCell : MonoBehaviour
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

    #endregion

    #region Properties

    public float Health { get { return health; } }
    public float Age { get { return age; } }

    public PlayerNumber PlayerNumber
    {
        get
        {
            return playerNumber;
        }
        set
        {
            playerNumber = value;
            switch (playerNumber)
            {
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

    public GameField GameField { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    #endregion

    // Use this for initialization
    void Start()
    {
        maxAge = (Endurance * 3f + Strength * 2f + Dexterity);
        health = Endurance * 5f + (Strength / Dexterity);
        maxHealth = health;

        attackSpeed = 1f / (Strength / 10f + Dexterity);

        reproductiveAgeBounds = new Vector2(maxAge * (minReproductiveAge / 100), maxAge * (maxReproductiveAge / 100));
        reproductiveAges = reproductiveAgeBounds.y - reproductiveAgeBounds.x;
    }

    void Update()
    {
        if (age > maxAge || health <= 0f)
        {
            Die();
            return;
        }
        age += Time.deltaTime;

        neighbours = GameField.GetNeighbours(X, Y);
        var emptyLength = neighbours.FreeCells.Length;
        var enemyLength = neighbours.EnemyCells.Length;

        if (emptyLength >= 2 && emptyLength < 8)
        {
            if (age > reproductiveAgeBounds.x && age < reproductiveAgeBounds.y)
            {
                TryReproduce();
            }
        }
        /*if (emptyLength == 0)
        {
            Die();
            return;
        }*/

        if (enemyLength > 0)
        {
            if (attackTimer > attackSpeed)
            {
                Attack(neighbours.EnemyCells);
            }
            attackTimer += Time.deltaTime;
        }
    }

    void Reproduce(Point[] freeCells)
    {
        var id = Random.Range(0, freeCells.Length);
        var point = freeCells[id];

        //------------------------------------------------------------------------------
        // TODO нужно разобраться как хранить инфу о клетке в виде одного числа 
        // (смещения в векторе virusGrid) при этом умея инстанцировать его в new Vector3
        //------------------------------------------------------------------------------

        var cell = (VirusCell)((GameObject)Instantiate(GameField.Cell, new Vector3(point.X - GameField.halfWidth, 0f, point.Y - GameField.halfHeight), Quaternion.identity)).GetComponent(typeof(VirusCell));
        cell.PlayerNumber = playerNumber;
        cell.X = point.X;
        cell.Y = point.Y;

        //TODO заменить на мутацию
        cell.Strength = Strength;
        cell.Endurance = Endurance;
        cell.Dexterity = Dexterity;
        cell.minReproductiveAge = minReproductiveAge;
        cell.maxReproductiveAge = maxReproductiveAge;

        GameField.AddCell(cell);
    }

    void Attack(Point[] enemyCells)// здесь избавиться можно прямо сейчас
    {
        var point = enemyCells[Random.Range(0, enemyCells.Length)];
        var ageCoef = Mathf.Clamp(age, reproductiveAgeBounds.x, reproductiveAgeBounds.y) - reproductiveAgeBounds.x;
        var timeValue = ageCoef / reproductiveAges;

        var strengthCoef = ReproductionStrengthCurve.Evaluate(timeValue);
        damage = (Strength + Dexterity / 2.75f + Endurance / 11f) * strengthCoef;
        if (damage > 0f && !float.IsNaN(damage))
        {
            var gridPoint = point.Y * GameField.Width + point.X;
            GameField.virusGrid[gridPoint].TakeDamage(damage);
            /*if (GameField.virusGrid[gridPoint].Health < 0 && Random.value < .24f)
            {
                var points = new Point[1];
                points[0] = point;
                Reproduce(points);
            }*/
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    public void Die()
    {
        GameField.decreaseCellCount((int)playerNumber);
        Destroy(gameObject);
    }

    private void SetColor(Color color)
    {
        renderer.materials[0].color = color;
    }

    private void TryReproduce()
    {
        reproductiveTimer += Time.deltaTime;

        var ageCoef = Mathf.Clamp(age, reproductiveAgeBounds.x, reproductiveAgeBounds.y) - reproductiveAgeBounds.x;
        var timeValue = ageCoef / reproductiveAges;
        var reproductiveAge = (maxReproductiveAge - minReproductiveAge) / 100f;
        ReproductiveSpeed = reproductiveAge / (Dexterity * 5f);
        ReproductiveSpeed *= maxAge;
        ReproductiveSpeed *= 1f / (health / maxHealth);
        ReproductiveSpeed /= 2f * ReproductionStrengthCurve.Evaluate(timeValue);

        if (reproductiveTimer > ReproductiveSpeed)
        {
            Reproduce(neighbours.FreeCells);
            reproductiveTimer = 0f;
        }
    }
}