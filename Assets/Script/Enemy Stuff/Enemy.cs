using System.Collections;
using UnityEngine;
using GreenSlime.Calculator;
using MyExtension;

public class Enemy : Creature
{
    public enum EnemyState { idle, chase, attack, death}

    [Header("Status")]
    public EnemyData enemyData;
    public EnemyState currentState;

    [Header("Effect")]
    public Material whiteMaterial;
    public GameObject deathEffect;
    public GameObject EXPPrefap;
    public GameObject plashEffect;
    public GameObject enemySoul;

    private Material defaultMaterial;

    [Header("Sound Effect")]
    public AudioClip hurtSound;
    public AudioClip explotionSound;
    public GameObject soundController;
    [Header("Other")]
    public LayerMask barrier;
    public GameObject itemDrop;

    [Header("Path Finder")]
    protected FindingPath findingPath;
    protected Vector2[] paths;
    protected int currentPath;

    //Protected Status
    protected int damage;
    protected float attackSpeed;
    protected int currentDEF;
    protected float moveSpeed;

    protected float chaseRadius;
    protected float attackRadius;
    protected bool canSeePlayer;

    //Storage Variable
    protected float atkCD;
    protected bool isInChaseRadius;
    protected bool isInAttackRadius;
    protected bool isAttacking;
    protected float takeDamageAtTime = -99;

    protected Transform player;

    protected Rigidbody2D rb;
    protected Animator anim;
    protected SpriteRenderer sr;
    protected GameManager GM;
    protected AudioSource audioSource;

    //For Path finding

    //Start Function
    protected virtual void BasicStart()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        GM = GameManager.instance;

        player = GameObject.Find("Player").transform;

        defaultMaterial = sr.material;

        ReadEnemyData();
        findingPath = GetComponent<FindingPath>();
    }
    protected void EnablePathFinding()
    {
        InvokeRepeating("UpdatePath", 0, 0.5f);
    }
    protected void UpdatePath()
    {
        if(canSeePlayer == false)
        {
            paths = findingPath.GetPathToTarget(transform.position, player.position, barrier);
            currentPath = 0;
        }
    }
    protected bool PathHaveValue()
    {
        if (paths == null) 
            return false;
        else if (paths.Length == 0)
            return false;

        return true;
    }
    protected void ClearPath()
    {
        currentPath = 0;
        paths = null;
    }

    protected void ReadEnemyData()
    {
        currentHp = enemyData.HP;
        damage = enemyData.ATK;
        attackSpeed = enemyData.ATKSpeed;
        currentDEF = enemyData.DEF;
        moveSpeed = enemyData.moveSpeed;

        chaseRadius = enemyData.chaseRarius;
        attackRadius = enemyData.attackRadius;

        creatureElement = enemyData.element;
    }
    //Other Function
    protected void CheckPlayer()
    {
        isInChaseRadius = BoolValue.IsInRadius(transform.position, player.position, chaseRadius);
        isInAttackRadius = BoolValue.IsInRadius(transform.position, player.position, attackRadius);
        canSeePlayer = !Physics2D.Linecast(transform.position, player.position, barrier);
    }
    protected void FlipImage(Transform current, Transform target)
    {
        if (current.position.x > target.position.x)
        {
            current.eulerAngles = new Vector3(0, -180, 0);
        }
        else
        {
            current.eulerAngles = new Vector3(0, 0, 0);
        }
    }
    protected void MoveToTarget(Transform current, Vector2 target, float speed)
    {
        current.position = Vector2.MoveTowards(current.position, target, speed * Time.deltaTime);
    }

    //Take Damage System
    public override void TakeDamage(int value, Element attackElement)
    {
        //Sound.PlaySound(audioSource, hurtSound);
        base.TakeDamage(value, attackElement);

        if (currentHp <= 0)
            StartCoroutine(DeathEffectCO());
        else
            StartCoroutine(TakeDamageEffectCO());
    }

    protected IEnumerator TakeDamageEffectCO()
    {
        sr.material = whiteMaterial;
        yield return new WaitForSeconds(0.1f);
        sr.material = defaultMaterial;
    }

    protected IEnumerator DeathEffectCO()
    {
        sr.material = whiteMaterial;
        currentState = EnemyState.death;
        rb.velocity = Vector2.zero;
        Vector2 dir = transform.position - player.position;
        dir.Normalize();
        rb.velocity = dir * 20f;
        yield return new WaitForSeconds(0.25f);

        Instantiate(deathEffect, transform.position, Quaternion.identity);

        GameObject sound = Instantiate(soundController, transform.position, Quaternion.identity);
        sound.GetComponent<SoundController>().PlayTheSound(explotionSound);

        GM.ShakeTheCamera();
        DropItem();
        OnDeath();
        Destroy(gameObject);
    }
    protected virtual void OnDeath()
    {

    }
    void DropItem()
    {
        //Item
        if(enemyData.lootTable != null)
        {
            int[] lootAmount = enemyData.GetLootArray();

            for (int i = 0; i < lootAmount.Length; i++)
            {
                for (int j = 0; j < lootAmount[i]; j++)
                {
                    GameObject item = Instantiate(itemDrop, transform.position, Quaternion.identity);
                    ItemDrop m_itemDrop = item.GetComponent<ItemDrop>();
                    m_itemDrop.SetItem(enemyData.GetItem(i));
                }
            }
        }

        //EXP
        for (int i = 0; i < enemyData.numberOfEXPCell; i++)
        {
            float minAngle = (transform.position - player.position).ToAngle() - 15;
            float maxAngle = (transform.position - player.position).ToAngle() + 15;
            float EXPAngle = Random.Range(minAngle, maxAngle);

            GameObject exp = Instantiate(EXPPrefap, transform.position, Quaternion.Euler(0, 0, EXPAngle));
            exp.GetComponent<EXP>().EXPAmount = enemyData.EXPPerCell;
        }

        float minAngleSoul = (transform.position - player.position).ToAngle() - 15;
        float maxAngleSoul = (transform.position - player.position).ToAngle() + 15;
        float SoulAngle = Random.Range(minAngleSoul, maxAngleSoul);
        Instantiate(enemySoul, transform.position, Quaternion.Euler(0, 0, SoulAngle));
    }
    /*public static Enemy operator +(Enemy a, Enemy b)
    {
        return a;
    }*/

    //Attack
    protected virtual void AttackCountdown(System.Action AttackFunc, bool canAttack = true)
    {
        if (atkCD <= 0 && canAttack)
        {
            AttackFunc();
            atkCD = enemyData.ATKSpeed;
        }
        else
        {
            atkCD -= Time.deltaTime;
        }
    }
    private void Reset()
    {
        whiteMaterial = Resources.Load("Material/Plash White") as Material;
        deathEffect = Resources.Load("Prefab/Death Effect") as GameObject;
        textDamage = Resources.Load("Prefab/Damage Text") as GameObject;

        hurtSound = Resources.Load("Sound/Enemy Hurt") as AudioClip;
        explotionSound = Resources.Load("Sound/Death Explosion") as AudioClip;
        soundController = Resources.Load("Prefab/Sound Controller") as GameObject;
        itemDrop = Resources.Load("Prefab/Item Drop") as GameObject;
        EXPPrefap = Resources.Load("Prefab/EXP") as GameObject;
        enemySoul = Resources.Load("Prefab/Item/Enemy Soul") as GameObject;

        plashEffect = Resources.Load("Prefab/Effect/Plash Effect") as GameObject;
    }
}
