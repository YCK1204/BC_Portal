using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public Condition health;
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
}
