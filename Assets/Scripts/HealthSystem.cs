using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;
    public event EventHandler OnDamaged;
    [SerializeField] private int health = 100;

    private int _healthMax;

    private void Awake()
    {
        _healthMax = health;
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        if (health < 0 )
        {
            health = 0;
        }

        if (health == 0)
        {
            Die();
        }
        OnDamaged?.Invoke(this, EventArgs.Empty);
    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return (float)health / _healthMax;
    }
}
