using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public  interface IDamagable
{
    void TakePhysicalDamage(float damage);
}

public class PlayerCondition : MonoBehaviour, IDamagable
{
    public HPBar health;

    public event Action onTakeDamage;
    public Image Panel;
    float currentTime = 0;
    float fadeoutTime = 2;

    private StageManager stageManager;

    void Awake()
    {
        stageManager = StageManager.Instance;
    }

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
        StartCoroutine(fadeOutandIn());
        
        // 포탈 모두 초기화 필요


    }

    IEnumerator fadeOutandIn()
    {
        Player player = PlayerManager.Instance.Player;
        Panel.gameObject.SetActive(true);
        Color alpha = Panel.color;

        while (alpha.a < 1)
        {
            currentTime += Time.deltaTime / fadeoutTime;
            alpha.a = Mathf.Lerp(0, 1, currentTime);
            Panel.color = alpha;
            yield return null;
        }
        Debug.Log("fadeout 종료");
        

        stageManager.RespawnPlayer(player.gameObject);
        health.curValue = health.maxValue;
        Debug.Log("리스폰 종료");

        Debug.Log("fadeout 안착");
        while (alpha.a > 0)
        {
            currentTime += Time.deltaTime / fadeoutTime;
            alpha.a = Mathf.Lerp(1, 0, currentTime);
            Panel.color = alpha;
            yield return null;
        }
        Debug.Log("fadein 종료");



        Panel.gameObject.SetActive(false);
        
    }

    public void TakePhysicalDamage(float Damage)
    {
        health.TakeDamage();
        health.Subtract(Damage);
        onTakeDamage?.Invoke();
    }
}
