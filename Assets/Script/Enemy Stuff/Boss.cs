using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    protected int currentPhase = 1;
    protected int maxPhase;
    protected BossData bossData;

    protected BossBarUI bossBarUI;

    //Start Func
    protected override void BasicStart()
    {
        base.BasicStart();
        bossData = enemyData as BossData;
        maxPhase = bossData.HPEachPhase.Length;

        bossBarUI = BossBarUI.instance;
        bossBarUI.SetStartValue(bossData.HPEachPhase[0], maxPhase);
        currentHp = bossData.HPEachPhase[0];
    }

    //Take Damage
    public override void TakeDamage(int value, Element attackElement)
    {
        Color textColor = Color.white;
        int damage = DamageCaculator(value, attackElement, out textColor);
        int finalDamage = Random.Range(damage - 1, damage + 2);

        currentHp -= finalDamage;

        GameObject text = Instantiate(textDamage, transform.position, Quaternion.identity);
        text.GetComponent<DamageText>().TextContent(transform.position, finalDamage, textColor);

        if (currentHp <= 0 && currentPhase < maxPhase)
        {
            StartCoroutine(TakeDamageEffectCO());
            currentPhase++;
            currentHp = bossData.HPEachPhase[currentPhase - 1];

            bossBarUI.SetMaxValue(currentHp);
            OnChangePhase();
            bossBarUI.RemoveOneLife();
        }
        else if (currentHp <= 0 && currentPhase == maxPhase)
        {
            StartCoroutine(DeathEffectCO());
        }
        else
        {
            StartCoroutine(TakeDamageEffectCO());
        }

        bossBarUI.SetValue(currentHp);
    }
    protected virtual void OnChangePhase()
    {

    }
    protected override void OnDeath()
    {
        bossBarUI.HideBossBar();
    }
}
