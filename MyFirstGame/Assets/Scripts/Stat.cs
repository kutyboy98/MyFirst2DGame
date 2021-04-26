using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField]
    private BarScript bar;
    [SerializeField]
    private float currentHealth;
    [SerializeField]
    private float maxHealth;
    public void Init()
    {
        MaxHealth = maxHealth;
        CurrentHealth = currentHealth;
    }
    public float CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            bar.value = currentHealth;
        }
    }
    public float MaxHealth
    {
        get { return maxHealth; }
        set
        {
            maxHealth = value;
            bar.maxHeath = maxHealth;
        }
    }

    public void Refresh()
    {
        this.bar.Refresh();
    }
}
