using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Image hpBar;
    public Image delayHpBar;
    public float hp = 100;
    public float maxHp = 100;
    public float Speed = 5f;
    public float delaySpeed = 2f;

    public Animator _hp;
    public Animator _damage;
    public Animator _potal;

    private float targetFill;
    private Color _color = Color.white;

    void Update()
    {
        //테스트용 데미지 or 포탈
        if (Input.GetKeyDown(KeyCode.Z))
        {
            TakeDamage(20f);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            PotalA();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            PotalB();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            hp = 100;
        }

        targetFill = hp / maxHp;

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

    public void SetHp(float value)
    {
        hp = Mathf.Clamp(value, 0, maxHp);
    }

    public void TakeDamage(float damage)
    {
        SetHp(hp - damage);
        _hp.SetTrigger("damage");
        _damage.SetTrigger("damage");
    }

    public void PotalA()
    {
        _potal.SetTrigger("blue");
    }
    public void PotalB()
    {
        _potal.SetTrigger("orange");
    }
}