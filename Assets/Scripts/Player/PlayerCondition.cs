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
    private SaveManager saveManager;

    void Awake()
    {
        stageManager = StageManager.Instance;
        saveManager = SaveManager.Instance;
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

        // 이벤트 활용 고려
        saveManager.LoadGame();
        stageManager.RespawnPlayer(player.gameObject);
        health.curValue = health.maxValue;
        foreach (var p in FindObjectsOfType<Portal>(false))
            p.RemovePortal();



        while (alpha.a > 0)
        {
            currentTime += Time.deltaTime / fadeoutTime;
            alpha.a = Mathf.Lerp(1, 0, currentTime);
            Panel.color = alpha;
            yield return null;
        }
        



        Panel.gameObject.SetActive(false);
        
    }

    public void TakePhysicalDamage(float Damage)
    {
        health.TakeDamage();
        health.Subtract(Damage);
        onTakeDamage?.Invoke();
    }
}
