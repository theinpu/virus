using System;
using UnityEngine;

public class PlayerSetting : ICloneable
{
    public const float MaxPoint = 90f;
    public const float MaxStat = 24f;
    public const float BoundsMultiplier= 1f;

    public float Strength
    {
        get { return strength; }
        set
        {
            var newValue = value;
            newValue = Mathf.Clamp(newValue, 1f, MaxStat);

            var currentPoints = GetBounds() + newValue + endurance + dexterity;
            if (currentPoints > MaxPoint)
            {
                var diff = currentPoints - MaxPoint;
                if (endurance > dexterity)
                {
                    endurance -= diff;
                    endurance = Mathf.Clamp(endurance, 1f, MaxStat);
                }
                else if (dexterity > 1)
                {
                    dexterity -= diff;
                    dexterity = Mathf.Clamp(dexterity, 1f, MaxStat);
                }
                else
                {
                    maxReproductiveAge -= diff * BoundsMultiplier;
                    maxReproductiveAge = Mathf.Clamp(maxReproductiveAge, minReproductiveAge + 10f, 90f);
                }
            }
            else
            {
                var diff = MaxPoint - currentPoints;
                if (endurance < dexterity)
                {
                    endurance += diff;
                    endurance = Mathf.Clamp(endurance, 1f, MaxStat);
                }
                else
                {
                    dexterity += diff;
                    dexterity = Mathf.Clamp(dexterity, 1f, MaxStat);
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
            newValue = Mathf.Clamp(newValue, 1f, MaxStat);

            var currentPoints = GetBounds() + newValue + strength + dexterity;
            if (currentPoints > MaxPoint)
            {
                var diff = currentPoints - MaxPoint;
                if (strength > dexterity)
                {
                    strength -= diff;
                    strength = Mathf.Clamp(strength, 1f, MaxStat);
                }
                else if(dexterity > 1)
                {
                    dexterity -= diff;
                    dexterity = Mathf.Clamp(dexterity, 1f, MaxStat);
                }
                else
                {
                    maxReproductiveAge -= diff * BoundsMultiplier;
                    maxReproductiveAge = Mathf.Clamp(maxReproductiveAge, minReproductiveAge + 10f, 90f);
                }
            }
            else
            {
                var diff = MaxPoint - currentPoints;
                if (strength < dexterity)
                {
                    strength += diff;
                    strength = Mathf.Clamp(strength, 1f, MaxStat);
                }
                else
                {
                    dexterity += diff;
                    dexterity = Mathf.Clamp(dexterity, 1f, MaxStat);
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
            newValue = Mathf.Clamp(newValue, 1f, MaxStat);

            var currentPoints = GetBounds() + newValue + endurance + strength;
            if (currentPoints > MaxPoint)
            {
                var diff = currentPoints - MaxPoint;
                if (endurance > strength)
                {
                    endurance -= diff;
                    endurance = Mathf.Clamp(endurance, 1f, MaxStat);
                }
                else if(strength > 1)
                {
                    strength -= diff;
                    strength = Mathf.Clamp(strength, 1f, MaxStat);
                }
                else
                {
                    maxReproductiveAge -= diff * BoundsMultiplier;
                    maxReproductiveAge = Mathf.Clamp(maxReproductiveAge, minReproductiveAge + 10f, 90f);
                }
            }
            else
            {
                var diff = MaxPoint - currentPoints;
                if (endurance < strength)
                {
                    endurance += diff;
                    endurance = Mathf.Clamp(endurance, 1f, MaxStat);
                }
                else
                {
                    strength += diff;
                    strength = Mathf.Clamp(strength, 1f, MaxStat);
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
            newValue = Mathf.Clamp(newValue, 10f, maxReproductiveAge - 20f);

            var currentPoints = GetMinBounds(newValue) + endurance + strength + dexterity;
            if (currentPoints > MaxPoint)
            {
                var diff = currentPoints - MaxPoint;
                if (IsBiggest(endurance) && endurance > 1)
                {
                    endurance -= diff;
                    endurance = Mathf.Clamp(endurance, 1f, MaxStat);
                }
                else if (IsBiggest(strength) && strength > 1)
                {
                    strength -= diff;
                    strength = Mathf.Clamp(strength, 1f, MaxStat);
                }
                else if (IsBiggest(dexterity) && dexterity > 1)
                {
                    dexterity -= diff;
                    dexterity = Mathf.Clamp(dexterity, 1f, MaxStat);
                }
            }
            else
            {
                var diff = MaxPoint - currentPoints;
                if (IsSmallest(endurance))
                {
                    endurance += diff;
                    endurance = Mathf.Clamp(endurance, 1f, MaxStat);
                }
                else if (IsSmallest(strength))
                {
                    strength += diff;
                    strength = Mathf.Clamp(strength, 1f, MaxStat);
                }
                else if (IsSmallest(dexterity))
                {
                    dexterity += diff;
                    dexterity = Mathf.Clamp(dexterity, 1f, MaxStat);
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
            newValue = Mathf.Clamp(newValue, minReproductiveAge + 20f, 90f);

            var currentPoints = GetMaxBounds(newValue) + endurance + strength + dexterity;
            if (currentPoints > MaxPoint)
            {
                var diff = currentPoints - MaxPoint;
                if (IsBiggest(endurance) && endurance > 1)
                {
                    endurance -= diff;
                    endurance = Mathf.Clamp(endurance, 1f, MaxStat);
                }
                else if (IsBiggest(strength) && strength > 1)
                {
                    strength -= diff;
                    strength = Mathf.Clamp(strength, 1f, MaxStat);
                }
                else if (IsBiggest(dexterity) && dexterity > 1)
                {
                    dexterity -= diff;
                    dexterity = Mathf.Clamp(dexterity, 1f, MaxStat);
                }
            }
            else
            {
                var diff = MaxPoint - currentPoints;
                if (IsSmallest(endurance))
                {
                    endurance += diff;
                    endurance = Mathf.Clamp(endurance, 1f, MaxStat);
                }
                else if (IsSmallest(strength))
                {
                    strength += diff;
                    strength = Mathf.Clamp(strength, 1f, MaxStat);
                }
                else if (IsSmallest(dexterity))
                {
                    dexterity += diff;
                    dexterity = Mathf.Clamp(dexterity, 1f, MaxStat);
                }
            }

            maxReproductiveAge = newValue;
        }
    }

    private float GetMinBounds(float min)
    {
        return Mathf.Floor((maxReproductiveAge - min) / BoundsMultiplier + 0.5f);
    }

    private float GetMaxBounds(float max)
    {
        return Mathf.Floor((max - minReproductiveAge) / BoundsMultiplier + 0.5f);
    }

    private float strength = 10f;
    private float endurance = 10f;
    private float dexterity = 10f;
    private float minReproductiveAge = 20f;
    private float maxReproductiveAge = 80f;

    public Point[] StartingPosition = new Point[GameGlobalScript.MaxCells];

    private bool IsBiggest(float val)
    {
        return val >= strength && val >= dexterity && val >= endurance;
    }

    private bool IsSmallest(float val)
    {
        return val <= strength && val <= dexterity && val <= endurance;
    }

    private float GetBounds()
    {
        return Mathf.Floor((maxReproductiveAge - minReproductiveAge) / BoundsMultiplier + 0.5f);
    }

    public object Clone()
    {
        return MemberwiseClone();
    }

    public void Normalize()
    {
        var diff = GetBounds() + strength + endurance + dexterity - MaxPoint;

        if (Mathf.Abs(diff) < float.Epsilon) return;

        Strength = strength;
        Endurance = endurance;
        Dexterity = dexterity;

        MinReproductiveAge = minReproductiveAge;
        MaxReproductiveAge = maxReproductiveAge;
    }
}