using UnityEngine;

public class PlayerSetting
{
    public const float MaxPoint = 30f;

    public float Strength
    {
        get { return strength; }
        set
        {
            var newValue = value;
            newValue = Mathf.Clamp(newValue, 1f, MaxPoint);

            var currentPoints = newValue + endurance + dexterity;
            if (currentPoints > MaxPoint)
            {
                var diff = currentPoints - MaxPoint;
                if (endurance > dexterity)
                {
                    endurance -= diff;
                    endurance = Mathf.Clamp(endurance, 1f, MaxPoint);
                }
                else
                {
                    dexterity -= diff;
                    dexterity = Mathf.Clamp(dexterity, 1f, MaxPoint);
                }
            }
            else
            {
                var diff = MaxPoint - currentPoints;
                if (endurance < dexterity)
                {
                    endurance += diff;
                    endurance = Mathf.Clamp(endurance, 1f, MaxPoint);
                }
                else
                {
                    dexterity += diff;
                    dexterity = Mathf.Clamp(dexterity, 1f, MaxPoint);
                }
            }

            strength = newValue;
        }
    }

    public float Endurance
    {
        get { return endurance; }
        set
        {
            var newValue = value;
            newValue = Mathf.Clamp(newValue, 1f, MaxPoint);

            var currentPoints = newValue + strength + dexterity;
            if (currentPoints > MaxPoint)
            {
                var diff = currentPoints - MaxPoint;
                if (strength > dexterity)
                {
                    strength -= diff;
                    strength = Mathf.Clamp(strength, 1f, MaxPoint);
                }
                else
                {
                    dexterity -= diff; 
                    dexterity = Mathf.Clamp(dexterity, 1f, MaxPoint);
                }                
            }
            else
            {
                var diff = MaxPoint - currentPoints;
                if (strength < dexterity)
                {
                    strength += diff;
                    strength = Mathf.Clamp(strength, 1f, MaxPoint);
                }
                else
                {
                    dexterity += diff;
                    dexterity = Mathf.Clamp(dexterity, 1f, MaxPoint);
                }
            }

            endurance = newValue;
        }
    }

    public float Dexterity
    {
        get { return dexterity; }
        set
        {
            var newValue = value;
            newValue = Mathf.Clamp(newValue, 1f, MaxPoint);

            var currentPoints = newValue + endurance + strength;
            if (currentPoints > MaxPoint)
            {
                var diff = currentPoints - MaxPoint;
                if (endurance > strength)
                {
                    endurance -= diff; 
                    endurance = Mathf.Clamp(endurance, 1f, MaxPoint);
                }
                else
                {
                    strength -= diff;
                    strength = Mathf.Clamp(strength, 1f, MaxPoint);
                }
            }
            else
            {
                var diff = MaxPoint - currentPoints;
                if (endurance < strength)
                {
                    endurance += diff;
                    endurance = Mathf.Clamp(endurance, 1f, MaxPoint);
                }
                else
                {
                    strength += diff;
                    strength = Mathf.Clamp(strength, 1f, MaxPoint);
                }
            }

            dexterity = newValue;
        }
    }

    private float strength = 10f;
    private float endurance = 10f;
    private float dexterity = 10f;
}