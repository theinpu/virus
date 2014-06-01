using UnityEngine;
using System.Collections;

public class VirusCell : MonoBehaviour
{

    #region Consts

    const float MaxEndurance = 30f;
    const float MaxStrength = 30f;
    const float MaxDexterity = 30f;

    private const float AgingChangeRate = 1f;

    private const float TimeScale = 1f;

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

    public bool IsAlive
    {
        get { return isAlive; }
        set
        {
            isAlive = value;
            gameObject.SetActive(isAlive);
            //renderer.enabled = value;
        }
    }

    private bool isAlive = false;

    #endregion

    public void ResetParams()
    {
        age = 0;
        maxAge = (Endurance * 3f + Strength * 2f + Dexterity);
        health = Endurance * 5f + (Strength / Dexterity);
        maxHealth = health;

        attackSpeed = 1f / (Strength / 10f + Dexterity);

        reproductiveAgeBounds = new Vector2(maxAge * (minReproductiveAge / 100), maxAge * (maxReproductiveAge / 100));
        reproductiveAges = reproductiveAgeBounds.y - reproductiveAgeBounds.x;
    }

    void Start()
    {
        var mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        var size = 0.75f;
        var vertexes = new[]
        {
            new Vector3(-size, 0, -size),
            new Vector3(size, 0, -size),
            new Vector3(-size, 0, size),
            new Vector3(size, 0, size)
        };

        var triangles = new[]
        {
            0, 2, 1,
            2, 3, 1
        };

        var uv = new[]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1), 
            new Vector2(1, 1)
        };

        var normals = new[]
        {
            Vector3.up, Vector3.up, Vector3.up, Vector3.up
        };

        mesh.vertices = vertexes;
        mesh.normals = normals;
        mesh.triangles = triangles;
        mesh.uv = uv;
    }

    void Update()
    {
        if (!isAlive) return;
        if (age > maxAge || health <= 0f)
        {
            Die();
            return;
        }
        age += Time.deltaTime * TimeScale;
        reproductiveTimer += Time.deltaTime * TimeScale;
        attackTimer += Time.deltaTime * TimeScale;

        neighbours = GameField.GetNeighbours(X, Y);

        SetMaterial();

        if (neighbours.freeCells >= 2)
        {
            if (age > reproductiveAgeBounds.x && age < reproductiveAgeBounds.y)
            {
                TryReproduce();
            }
        }

        if (neighbours.enemyCells > 0)
        {
            if (attackTimer > attackSpeed)
            {
                Attack(neighbours.EnemyCells);
                attackTimer = 0f;
            }
        }
    }

    private void SetMaterial()
    {
        var radius = (age * 0.1f) / maxAge + 0.25f;
        //radius -= (Mathf.Sin(age * Random.value) / 20f);
        radius = Mathf.Clamp(radius, 0.15f, 0.35f);
        var grayVal = Mathf.Clamp(1f - (health) / maxHealth, 0, 1f);
        renderer.material.SetFloat("_Radius", radius);
        renderer.material.SetFloat("_GrayVal", grayVal);
    }

    private int GetCellType(int x, int y)
    {
        var i = 0;
        var cell = GameField.VirusGrid[y * GameField.Width + x];
        if (cell.PlayerNumber != PlayerNumber.None)
            i = (PlayerNumber == cell.PlayerNumber) ? 1 : 2;
        return i;
    }

    void Reproduce(Point[] freeCells)
    {
        //Debug.Log(freeCells.Length);
        var id = Random.Range(0, neighbours.freeCells);
        var point = freeCells[id];

        //------------------------------------------------------------------------------
        // TODO нужно разобраться как хранить инфу о клетке в виде одного числа 
        // (смещения в векторе virusGrid) при этом умея инстанцировать его в new Vector3
        //------------------------------------------------------------------------------

        var cell = GameField.VirusGrid[point.Y * GameField.Width + point.X];
        cell.PlayerNumber = playerNumber;

        //TODO заменить на мутацию
        cell.Strength = Strength;
        cell.Endurance = Endurance;
        cell.Dexterity = Dexterity;
        cell.minReproductiveAge = minReproductiveAge;
        cell.maxReproductiveAge = maxReproductiveAge;
        cell.ResetParams();
        cell.IsAlive = true;

        GameField.AddCell(cell);
    }

    void Attack(Point[] enemyCells)// здесь избавиться можно прямо сейчас
    {
        var point = enemyCells[Random.Range(0, neighbours.enemyCells)];
        var ageCoef = Mathf.Clamp(age, reproductiveAgeBounds.x, reproductiveAgeBounds.y) - reproductiveAgeBounds.x;
        var timeValue = ageCoef / reproductiveAges;

        var strengthCoef = ReproductionStrengthCurve.Evaluate(timeValue);
        damage = (Strength + Dexterity / 2.75f + Endurance / 11f) * strengthCoef;
        if (damage > 0f && !float.IsNaN(damage))
        {
            var gridPoint = point.Y * GameField.Width + point.X;
            GameField.VirusGrid[gridPoint].TakeDamage(damage);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    public void Die()
    {
        IsAlive = false;
        GameField.DecreaseCellCount((int)playerNumber);
        PlayerNumber = PlayerNumber.None;
    }

    private void SetColor(Color color)
    {
        renderer.materials[0].color = color;
    }

    private void TryReproduce()
    {
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