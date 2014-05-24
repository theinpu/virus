using System;
using UnityEngine;

public class PlayerSetting : ICloneable
{
    public const float MaxPoint = 42f;

    public float Strength
    {
        get { return strength; }
        set
        {
            var newValue = value;
            newValue = Mathf.Clamp(newValue, 1f, MaxPoint);

            var currentPoints = Mathf.Floor((maxReproductiveAge - minReproductiveAge) / 5 + 0.5f) + newValue + endurance + dexterity;
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

            var currentPoints = Mathf.Floor((maxReproductiveAge - minReproductiveAge) / 5 + 0.5f) + newValue + strength + dexterity;
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

            var currentPoints = (maxReproductiveAge - minReproductiveAge) / 5 + newValue + endurance + strength;
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

    public float MinReproductiveAge
    {
        get { return minReproductiveAge; }
        set
        {
            var newValue = value;
            newValue = Mathf.Clamp(newValue, 10f, maxReproductiveAge - 10f);

            var currentPoints = Mathf.Floor( (maxReproductiveAge - newValue) / 5 + 0.5f) + endurance + strength + dexterity;
            if (currentPoints > MaxPoint)
            {
                var diff = currentPoints - MaxPoint;
                if (isBiggest(endurance))
                {
                    endurance -= diff;
                    endurance = Mathf.Clamp(endurance, 1f, MaxPoint);
                }
                else if (isBiggest(strength))
                {
                    strength -= diff;
                    strength = Mathf.Clamp(strength, 1f, MaxPoint);
                }
                else if (isBiggest(dexterity))
                {
                    dexterity -= diff;
                    dexterity = Mathf.Clamp(dexterity, 1f, MaxPoint);
                }
            }
            else
            {
                var diff = MaxPoint - currentPoints;
                if (isSmallest(endurance))
                {
                    endurance += diff;
                    endurance = Mathf.Clamp(endurance, 1f, MaxPoint);
                }
                else if (isSmallest(strength))
                {
                    strength += diff;
                    strength = Mathf.Clamp(strength, 1f, MaxPoint);
                }
                else if (isSmallest(dexterity))
                {
                    dexterity += diff;
                    dexterity = Mathf.Clamp(dexterity, 1f, MaxPoint);
                }
            }

            minReproductiveAge = newValue;
        }
    }

    public float MaxReproductiveAge
    {
        get { return maxReproductiveAge; }
        set
        {
            var newValue = value;
            newValue = Mathf.Clamp(newValue, minReproductiveAge + 10f, 90f);

            var currentPoints = Mathf.Floor( (newValue - minReproductiveAge) / 5 + 0.5f) + endurance + strength + dexterity;
            if (currentPoints > MaxPoint)
            {
                var diff = currentPoints - MaxPoint;
                if (isBiggest(endurance))
                {
                    endurance -= diff;
                    endurance = Mathf.Clamp(endurance, 1f, MaxPoint);
                }
                else if (isBiggest(strength))
                {
                    strength -= diff;
                    strength = Mathf.Clamp(strength, 1f, MaxPoint);
                }
                else if (isBiggest(dexterity))
                {
                    dexterity -= diff;
                    dexterity = Mathf.Clamp(dexterity, 1f, MaxPoint);
                }
            }
            else
            {
                var diff = MaxPoint - currentPoints;
                if (isSmallest(endurance))
                {
                    endurance += diff;
                    endurance = Mathf.Clamp(endurance, 1f, MaxPoint);
                }
                else if (isSmallest(strength))
                {
                    strength += diff;
                    strength = Mathf.Clamp(strength, 1f, MaxPoint);
                }
                else if (isSmallest(dexterity))
                {
                    dexterity += diff;
                    dexterity = Mathf.Clamp(dexterity, 1f, MaxPoint);
                }
            }

            maxReproductiveAge = newValue;
        }
    }

    private float strength = 10f;
    private float endurance = 10f;
    private float dexterity = 10f;
    private float minReproductiveAge = 20f;
    private float maxReproductiveAge = 80f;

    public Point[] StartingPosition = new Point[GameGlobalScript.MaxCells];

    private bool isBiggest(float val)
    {
        if (val >= strength && val >= dexterity && val >= endurance)
            return true;

        else return false;
    }

    private bool isSmallest(float val)
    {
        if (val <= strength && val <= dexterity && val <= endurance)
            return true;

        else return false;
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}