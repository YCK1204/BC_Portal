using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Image hpBar;
    public Image delayHpBar;
    public float startValue = 100;
    public float curValue = 100;
    public float passiveValue = 100;
    public float maxValue = 1;


    public float Speed = 5f;
    public float delaySpeed = 2f;

    public Animator _hp;
    public Animator _damage;

    private float targetFill;
    private Color _color = Color.white;

    void Update()
    {
        //테스트용 데미지 or 포탈
        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    TakeDamage();
        //}

        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    curValue = 100;
        //}

        targetFill = curValue / maxValue;

        //실제체력바
        hpBar.fillAmount = Mathf.Lerp(hpBar.fillAmount, targetFill, Time.deltaTime * Speed);

        //딜레이체력바
        delayHpBar.fillAmount = Mathf.Lerp(delayHpBar.fillAmount, targetFill, Time.deltaTime * delaySpeed);

        //체력바 색조변경
        float value = Mathf.Lerp(180f, 240f, (Mathf.Sin(Time.time * 2f) + 1f) * 0.5f);
        float normalized = value / 255f;

        _color.r = normalized;
        _color.g = normalized;
        _color.b = normalized;
        _color.a = 1f;
        hpBar.color = _color;
    }

    public void Add(float value)
    {
        curValue = Mathf.Min(curValue + value, maxValue);
    }

    public void Subtract(float value)
    {
        curValue = Mathf.Max(curValue - value, 0);
    }

    public void SetHp(float value)
    {
        curValue = Mathf.Clamp(value, 0, maxValue);
    }

    public void TakeDamage()
    {
        _hp.SetTrigger("damage");
        _damage.SetTrigger("damage");
    }
}