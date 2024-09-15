using GreenSlime.Calculator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GreenSlime.SoundUtilities;
using GreenSlime;
using MyMovement;
using MyExtension;

public class Player : PlayerUtilities
{
    public enum PlayerState { normal, death}
    [Header("Status")]
    public PlayerState currentState;

    private BarUI playerHPBar;
    private BarUI playerMPBar;

    private float currentSpeed;

    [Header("Weapon Setting")]
    public SpriteRenderer weaponRenderer;
    public Transform weaponViot;
    public LayerMask whatIsEnemy;
    public Transform orbViot;
    private float attackCD;

    private float angle;
    private Vector2 mousePos;

    [Header("Skill")]
    public SkillManager skillManager;
    SpellCard activateSpellCard;

    [Header("Effect")]
    public GameObject smokePrefab;
    private float smokeReleaseCD;
    public GameObject slashEffect;
    public GameObject activateWeaponEffect;

    [Header("Sound Effect")]
    public AudioClip slashSound;
    public AudioClip takeDamageSound;
    public AudioClip shootArrowSound;

    private Vector2 change;
    private float dashCD;

    private PlayerStat playerStat;

    public Animator swordAnim;
    public PlayerEffectManager PEM;
    private Rigidbody2D rb;
    private Animator anim;
    private AudioSource audioSource;
    private GameManager GM;
    private UIManager UIM;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        PEM = GetComponent<PlayerEffectManager>();
        GM = GameManager.instance;
        UIM = UIManager.instance;

        playerStat = GM.playerStat;

        playerHPBar = GameObject.Find("Player HP Bar").GetComponent<BarUI>();
        playerHPBar.SetMaxAndValue(playerStat.MaxHP);
        playerMPBar = GameObject.Find("Player MP Bar").GetComponent<BarUI>();
        playerMPBar.SetMaxAndValue(playerStat.MaxMP);

        currentSpeed = playerStat.MoveSpeed;

        UpdatePlayerData();
    }

    void Update()
    {
        if(currentState != PlayerState.death)
        {
            PlayerMovement();
            PlayerAnimation();
            Aimming();
            AttackInput();
            Dash();
        }
    }


    #region Movement
    void PlayerMovement()
    {
        change = TopDownMovementInput();
        Player2DMove(rb, change, currentSpeed);
    }

    void PlayerAnimation()
    {
        if(change != Vector2.zero)
        {
            anim.SetBool("move", true);

            if(smokeReleaseCD <= 0)
            {
                Instantiate(smokePrefab, transform.position, Quaternion.identity);

                smokeReleaseCD = 0.5f;
            }
            else
            {
                smokeReleaseCD -= Time.deltaTime;
            }

        }
        else
        {
            anim.SetBool("move", false);
        }

        if(mousePos.x >= transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
        }
    }
    #endregion

    #region Attack
    void AttackInput()
    {
        if (attackCD <= 0)
        {
            if (Input.GetMouseButton(0) && GM.GetSelectedWeapon() != null && GM.currentMP >= GM.GetSelectedWeapon().manaNeed)
            {
                Attack(GM.GetSelectedWeapon());
                GM.currentMP -= GM.GetSelectedWeapon().manaNeed;
                playerMPBar.SetValue(GM.currentMP);

                Instantiate(activateWeaponEffect, weaponViot.position, weaponViot.rotation, transform);

                attackCD = playerStat.AttackSpeed;
            }
        }
        else
        {
            attackCD -= Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(1) && SpellCardManager.instance.CanActivateSpellCard())
        {
            ActivateSpellCard();
        }
    }

    void Attack(WeaponItem weaponToAttack)
    {
        if (weaponToAttack != null)
        {
            WeaponItem weapon = weaponToAttack;

            weaponRenderer.sprite = weaponToAttack.itemImage;
            GameObject attackObj = Instantiate(weapon.bullet, weaponRenderer.transform.position, weaponViot.rotation);

            switch (weapon.attackType)
            {
                case AttackType.waveAttack:
                    attackObj.GetComponent<SlashAttack>().SetData(playerStat.ATK, weaponToAttack.element, this);
                    break;

                case AttackType.shootElementBall:
                    PlayerMagicBullet playerBullet = attackObj.GetComponent<PlayerMagicBullet>();
                    playerBullet.BulletData(weaponViot.right, weapon.bulletSpeed, playerStat.ATK, weaponToAttack.element, this);
                    break;
                case AttackType.orb:
                    float shootAngle = (weaponRenderer.transform.position - transform.position).ToAngle();
                    PlayerMagicBullet playerBullet2 = attackObj.GetComponent<PlayerMagicBullet>();
                    playerBullet2.BulletData(shootAngle.ToDirection(), weapon.bulletSpeed, playerStat.ATK, weaponToAttack.element, this);
                    break;
            }
        }
    }
    void ActivateSpellCard()
    {
        SpellCardManager.instance.ActivateSpellCard(out activateSpellCard);

        if(activateSpellCard != null)
        {
            skillManager.ActivateSkill(activateSpellCard);
        }
    }
    #endregion

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && dashCD <= 0)
        {
            gameObject.tag = "Immune";
            currentSpeed = playerStat.MoveSpeed * 3;
            PEM.PlayDashEffect();
            StartCoroutine(DashCO());
            dashCD = 0.5f;
        }
        else
        {
            dashCD -= Time.deltaTime;
        }
    }
    IEnumerator DashCO()
    {
        yield return new WaitForSeconds(0.2f);
        gameObject.tag = "Player";
        currentSpeed = playerStat.MoveSpeed;
    }
    void Aimming()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (GM.GetSelectedWeapon() == null) return;

        if(GM.GetSelectedWeapon().attackType == AttackType.orb)
        {
            weaponViot.localPosition = new Vector2(0f, 0.4f);
            weaponViot.eulerAngles = Vector3.zero;
            weaponRenderer.transform.position =  Movement2D.MoveAround(weaponRenderer.transform.position, weaponViot.position, 1.5f, 1);
        }
        else
        {
            weaponViot.localPosition = new Vector2(0.4f, 0.4f);
            angle = FloatValue.AngleRotate2Target(weaponViot.position, mousePos);
            weaponRenderer.transform.position = weaponViot.position;
            weaponViot.eulerAngles = new Vector3(0, 0, angle);
        }
    }

    public void TakeDamage(int damage)
    {
        if(currentState != PlayerState.death)
        {
            GM.currentHP -= damage;
            UIM.TakeDamageEffect();
            Sound.PlaySound(audioSource, takeDamageSound);

            playerHPBar.SetValue(GM.currentHP);
            StartCoroutine(TakeDamageCO());

            if (GM.currentHP <= 0)
            {
                anim.SetTrigger("death");
                currentState = PlayerState.death;
                StartCoroutine(UIM.OpenGameOverPanel());
            }
        }
    }

    public void GainHP(int value)
    {
        if(GM.currentHP < playerStat.MaxHP)
        {
            GM.currentHP += value;

            if (GM.currentHP > playerStat.MaxHP)
                GM.currentHP = playerStat.MaxHP;
        }

        playerHPBar.SetValue(GM.currentHP);
        UIM.HealEffect();
    }

    IEnumerator TakeDamageCO()
    {
        gameObject.tag = "Immune";
        yield return new WaitForSeconds(0.5f);
        gameObject.tag = "Player";
    }

    public void GainMP(int value)
    {
        GM.currentMP += value;
        if (GM.currentMP > playerStat.MaxHP)
            GM.currentMP = playerStat.MaxHP;
        playerMPBar.SetValue(GM.currentMP);
    }
    public void RevivePlayer()
    {
        anim.SetTrigger("revive");
        currentState = PlayerState.normal;
        transform.position = new Vector2(15, 10);
        gameObject.tag = "Player";

        GM.currentHP = playerStat.MaxHP;
        playerHPBar.SetValue(GM.currentHP);
        GM.currentMP = 100;
        playerMPBar.SetValue(GM.currentMP);
    }
    public void UpdatePlayerData()
    {
        if (GM == null)
            GM = GameManager.instance;

        if (GM.GetSelectedWeapon() != null)
            weaponRenderer.sprite = GM.GetSelectedWeapon().itemImage;
        else
            weaponRenderer.sprite = null;

    }
}
