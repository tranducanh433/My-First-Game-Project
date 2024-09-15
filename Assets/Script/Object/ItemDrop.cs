using MyExtension;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public SpriteRenderer sr;
    public RuntimeAnimatorController coinAnim;
    public Material defaultMaterial;

    bool canGet;
    bool follow;
    ItemBase item;
    Transform player;

    Rigidbody2D rb;
    GameManager GM;
    GetItemEvent GIE;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GM = GameManager.instance;
        GIE = GetItemEvent.instance;

        player = GameObject.Find("Player").transform;
        StartCoroutine(BoundingEffectCO());
    }

    private void Update()
    {
        if(Vector2.Distance(player.position, transform.position) <= 3 && canGet == true)
        {
            if (GM.IsFull(item) == false)
            {
                rb.gravityScale = 0;
                follow = true;
            }
            float distance = Vector2.Distance(player.position, transform.position);
            float scale = (((distance + 3) / 3) / 2).Limited(0.5f, 1);
            transform.localScale = new Vector2(scale, scale);
        }
        if(follow == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, 15f * Time.deltaTime);
            

            if(Vector2.Distance(player.position, transform.position) <= 0.2f)
            {
                if(GM.IsFull(item) == false)
                {
                    GIE.NewItemEvent(item);

                    if (item is CoinItem)
                        GM.GainCoin(1);
                    else
                        GM.AddItem(item);

                    Destroy(gameObject);
                }
                else
                {
                    follow = false;
                    StartCoroutine(BoundingEffectCO());
                }
            }
        }
    }

    public void SetItem(ItemBase _item)
    {
        item = _item;
        sr.sprite = _item.itemImage;

        if(_item is CoinItem)
        {
            Animator anim = GetComponent<Animator>();
            anim.runtimeAnimatorController = coinAnim;
            sr.material = defaultMaterial;
        }
    }

    IEnumerator BoundingEffectCO()
    {
        float force = Random.Range(3f, 6f);
        float waitTime = Random.Range(0.3f, 0.6f);
        Vector2 dir = Random.Range(45f, 135f).ToDirection();
        rb.velocity = dir * force;
        yield return new WaitForSeconds(waitTime);
        canGet = true;
        rb.velocity = dir * (force * 0.4f);
        yield return new WaitForSeconds(waitTime * 0.4f);
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
    }
}
