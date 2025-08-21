using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  interface IDamagable
{
    void TakePhysicalDamage(float damage);
}

public class PlayerCondition : MonoBehaviour, IDamagable
{
    public HPBar health;

    public event Action onTakeDamage;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        health.Add(health.passiveValue * Time.deltaTime);

        if(health.curValue <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Die");
    }

    public void TakePhysicalDamage(float Damage)
    {
        health.TakeDamage();
        health.Subtract(Damage);
        onTakeDamage?.Invoke();
    }
}
