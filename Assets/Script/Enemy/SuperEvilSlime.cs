using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GreenSlime.Calculator;
using MyExtension;

public class SuperEvilSlime : SlimeBase
{
    public enum AttackState { Wait, Attack1, Attack2 }
    [Header("Super Evil Slime Setting")]
    public AttackState currentAttack;

    private int lastSkill;
    private int currentSkill;

    [Header("Other Effect")]
    public GameObject explodeEffect;

    [Header("Jump Attack Skill")]
    public Transform shootPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 7f;

    private float jumpStep;
    [Header("Shooting Skill Setting")]
    public GameObject purpleBulletBall;
    public GameObject enemyToSummon;
    public float jellyBallSpeed = 5f;

    [Header("Attack Phase 2")]
    public GameObject bigCircleBulletPrefab;

    [Header("Attack Phase 3")]
    public GameObject homingBullet;
    bool startLastPhase;
    float cdToShootBullet;
    float cdToShootBigBullet;
    private Vector2 summonPos;

    [Header("Boss")]
    int currentPhase = 1;
    int maxPhase;
    BossData bossData;

    BossBarUI bossBarUI;


    private int shootCount;
    private float shootCD;
    private bool onGround = true;

    void Start()
    {
        BasicStart();
        RandomSkill();
        currentState = EnemyState.chase;
        summonPos = transform.position;
    }

    void Update()
    {
        if (currentState != EnemyState.death)
        {
            JumpAttack1();
            ShootingAndSummon();

            if (jumpStep != 3)
                JumpMovement(moveSpeed);
            else
                JumpMovement(moveSpeed * 2);


            LastPhaseAttack();
            //Flip Image
            FlipImage(transform, player);
        }
    }



    #region State Machine Controller

    void RandomSkill()
    {
        do
        {
            int r = Random.Range(1, 3);
            currentSkill = r;
        } while (lastSkill == currentSkill);
        lastSkill = currentSkill;

        if (currentSkill == 1)
        {
            StartCoroutine(SelectSkill(AttackState.Attack1));
        }
        else if (currentSkill == 2)
        {
            StartCoroutine(SelectSkill(AttackState.Attack2));
        }
    }

    IEnumerator SelectSkill(AttackState skillChoose)
    {
        yield return new WaitForSeconds(1.25f);
        currentAttack = skillChoose;
    }
    #endregion

    #region Set Jump And Fall Pos
    protected override void SetJumpAndFallPoint(float maxHeight, Transform target = null)
    {
        //Get the start point
        startPoint = transform.position;
        anim.SetBool("isJump", true);
        //Calcular Fall Point
        if (target == null)
        {
            fallPoint = Vector2.MoveTowards(transform.position, player.position, 3);
        }
        else
        {
            fallPoint = target.position;
        }

        if (fallPoint.x == startPoint.x)
        {
            fallPoint = new Vector2(fallPoint.x + 0.001f, fallPoint.y);
        }

        //Set Value To Make Sure That This "If" Only Activate One Time
        currentPoint = 0;
        jumpingPoints.Clear();

        GetJumpingPoint(maxHeight);
    }
    protected override void SetJumpAndFallPoint(float maxHeight, Vector2 target)
    {
        //Get the start point
        startPoint = transform.position;
        anim.SetBool("isJump", true);
        //Calcular Fall Point
        fallPoint = target;

        if (fallPoint.x == startPoint.x)
        {
            fallPoint = new Vector2(fallPoint.x + 0.001f, fallPoint.y);
        }

        //Set Value To Make Sure That This "If" Only Activate One Time
        currentPoint = 0;
        jumpingPoints.Clear();

        GetJumpingPoint(maxHeight);
    }
    #endregion

    #region Jump Attack 1 and 2
    void JumpAttack1()
    {
        if(currentAttack == AttackState.Attack1)
        {
            if(atkCD <= 0 && onGround)
            {
                if (jumpStep != 3)
                {
                    SetJumpAndFallPoint(jumpHeight);
                }

                onGround = false;
                atkCD = attackSpeed;
            }
            else
            {
                atkCD -= Time.deltaTime;
            }
        }
    }

    void JumpToPlayer()
    {
        //Get Jump and Fall Point
        if (isJumping == false)
        {
            SetJumpAndFallPoint(jumpHeight * 1.5f, player);
        }
 
    }

    public void SpreadBullet()  //Deal damage to player
    {
        GM.ShakeTheCamera();

        switch (currentPhase)
        {
            case 1:
                JumpingPhase1();
                break;
            case 2:
                JumpingPhase2();
                break;
            case 3:
                JumpingPhase3();
                break;
        }

        /*if(currentPhase == 2)
        {
            jumpStep++;
            if (jumpStep == 3)
            {
                JumpToPlayer();
            }

            int numberOfBullet = 15;


            for (int i = 0; i < numberOfBullet; i++)
            {
                float angle = Random.Range(0, 360);
                shootPoint.rotation = Quaternion.Euler(0, 0, angle);
                GameObject _bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                float _bulletSpeed = Random.Range(bulletSpeed - 1f, bulletSpeed + 1f);
                _bullet.GetComponent<EnemyBullet>().BulletData(shootPoint.right, _bulletSpeed, damage, BulletColor.Purple, Element.None);
            }

            if (jumpStep == 4 && currentPhase != 3)
            {
                jumpStep = 0;
                RandomSkill();
                currentAttack = AttackState.Wait;
            }
        }
        else
        {
            int numberOfBullet = (currentPhase == 1 || jumpStep == 4)? 10 : 16;
            float angle = 0;
            float angleStep = 360 / numberOfBullet;

            for (int i = 0; i < numberOfBullet; i++)
            {
                GameObject _bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                _bullet.GetComponent<EnemyBullet>().BulletData(angle.ToDirection(), bulletSpeed, damage, BulletColor.Purple, Element.None);

                angle += angleStep;
            }

            if(currentPhase == 1)
            {
                jumpStep++;
                if (jumpStep == 3)
                {
                    JumpToPlayer();
                }

                if (jumpStep == 4 && currentPhase != 3)
                {
                    jumpStep = 0;
                    RandomSkill();
                    currentAttack = AttackState.Wait;
                }
            }
        }*/


        if (Vector2.Distance(transform.position, player.position) <= 1)
        {
            player.GetComponent<Player>().TakeDamage(damage);
        }


        onGround = true;

    }
    void JumpingPhase1()
    {
        int numberOfBullet = 10;
        float angle = 0;
        float angleStep = 360 / numberOfBullet;

        for (int i = 0; i < numberOfBullet; i++)
        {
            GameObject _bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            _bullet.GetComponent<EnemyBullet>().BulletData(angle.ToDirection(), bulletSpeed, damage, BulletColor.Purple, Element.None);

            angle += angleStep;
        }

        jumpStep++;
        if (jumpStep == 3)
        {
            JumpToPlayer();
        }

        if (jumpStep == 4)
        {
            jumpStep = 0;
            RandomSkill();
            currentAttack = AttackState.Wait;
        }
    }

    void JumpingPhase2()
    {
        jumpStep++;
        if (jumpStep == 3)
        {
            JumpToPlayer();
        }

        int numberOfBullet = 15;


        for (int i = 0; i < numberOfBullet; i++)
        {
            float angle = Random.Range(0, 360);
            shootPoint.rotation = Quaternion.Euler(0, 0, angle);
            GameObject _bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            float _bulletSpeed = Random.Range(bulletSpeed - 1f, bulletSpeed + 1f);
            _bullet.GetComponent<EnemyBullet>().BulletData(shootPoint.right, _bulletSpeed, damage, BulletColor.Purple, Element.None);
        }

        if (jumpStep == 4)
        {
            jumpStep = 0;
            RandomSkill();
            currentAttack = AttackState.Wait;
        }
    }
    void JumpingPhase3()
    {
        int numberOfBullet = 16;
        float angle = 0;
        float angleStep = 360 / numberOfBullet;

        for (int i = 0; i < numberOfBullet; i++)
        {
            GameObject _bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            _bullet.GetComponent<EnemyBullet>().BulletData(angle.ToDirection(), bulletSpeed, damage, BulletColor.Purple, Element.None);

            angle += angleStep;
        }
    }
    #endregion

    #region Shooting And Summon Attack
    void ShootingAndSummon()
    {
        if(currentAttack == AttackState.Attack2)
        {
            if(currentPhase == 1)
            {
                if (shootCount >= 4)
                {
                    currentAttack = AttackState.Wait;
                    shootCount = 0;
                    RandomSkill();
                }

                if (shootCD <= 0)
                {
                    anim.SetTrigger("shooting");
                    shootCD = 0.75f;
                }
                else
                {
                    shootCD -= Time.deltaTime;
                }
            }
            else
            {
                ShootTrailBullet();
                currentAttack = AttackState.Wait;
                RandomSkill();
            }
        }
    }
    public void ShootTrailBullet()
    {
        float startAngle = FloatValue.AngleRotate2Target(transform.position, player.position) - 30;

        for (int i = 0; i < 3; i++)
        {
            GameObject bullet = Instantiate(bigCircleBulletPrefab, transform.position, Quaternion.Euler(0, 0, startAngle));
            EnemyBullet enemyBullet = bullet.GetComponent<EnemyBullet>();
            enemyBullet.BulletData(startAngle.ToDirection(), 10, damage, BulletColor.Purple, Element.None, false, false);
            enemyBullet.OnTrailEvent(bulletPrefab, 0.5f, 0.05f, 5, damage, BulletColor.Purple, Element.None, bullet.transform.right, 0.5f);

            startAngle += 30;
        }

    }
    public void Shooting_Attack()
    {
        shootCount++;

        GameObject _purpleJellyBall = Instantiate(purpleBulletBall, transform.position, Quaternion.identity);

        _purpleJellyBall.GetComponent<BulletModify>().SetBulletData( player.position, jellyBallSpeed, bulletSpeed, damage, 6, enemyToSummon, bulletPrefab, BulletColor.Purple);
    }

    #endregion

    #region Last Phase
    void StartSpread()
    {
        startLastPhase = true;
    }

    void LastPhase()
    {
        GameObject bullet = Instantiate(bigCircleBulletPrefab, transform.position, Quaternion.identity);
        EnemyBullet enemyBullet = bullet.GetComponent<EnemyBullet>();
        enemyBullet.BulletData(Vector2.zero, 0, damage, BulletColor.Purple, Element.None, false, false);
        enemyBullet.MoveToPosEvent(summonPos, 10f, StartSpread);
    }

    void LastPhaseAttack()
    {
        if (startLastPhase)
        {
            ShootRandom();
            LastPhaseJumpAttack();
        }
    }

    void LastPhaseJumpAttack()
    {
        if (atkCD <= 0 && onGround)
        {
            Vector2 minPos = new Vector2(summonPos.x - 3, summonPos.y - 3);
            Vector2 maxPos = new Vector2(summonPos.x + 3, summonPos.y + 3);
            Vector2 jumpPos = VectorValue.RandomVector(minPos, maxPos);

            SetJumpAndFallPoint(jumpHeight, jumpPos);
            
            onGround = false;
            atkCD = attackSpeed;
        }
        else
        {
            atkCD -= Time.deltaTime;
        }
    }

    float angleShooting;

    void ShootRandom()
    {
        if (cdToShootBullet <= 0)
        {
            angleShooting += 25;

            GameObject bullet = Instantiate(homingBullet, summonPos, Quaternion.Euler(0, 0, angleShooting));
            EnemyBullet enemyBullet = bullet.GetComponent<EnemyBullet>();
            enemyBullet.BulletData(angleShooting.ToDirection(), 7.5f, damage, BulletColor.Red, Element.Fire);

            cdToShootBullet = 0.1f;
        }
        else
        {
            cdToShootBullet -= Time.deltaTime;
        }
    }
    #endregion

    #region Boss Class
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

    void OnChangePhase()
    {
        SummonPlashEffect();
        
        if(currentPhase == 3)
        {
            LastPhase();
            currentAttack = AttackState.Wait;
        }
    }
    void SummonPlashEffect()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector2 minPos = new Vector2(transform.position.x - 2, transform.position.y);
            Vector2 maxPos = new Vector2(transform.position.x + 2, transform.position.y + 3);
            Vector2 finalPos = VectorValue.RandomVector(minPos, maxPos);

            Instantiate(explodeEffect, finalPos, Quaternion.identity);
        }
    }

    protected override void OnDeath()
    {
        bossBarUI.HideBossBar();
    }
    #endregion
}
