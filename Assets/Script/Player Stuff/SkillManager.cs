using GreenSlime.Calculator;
using MyExtension;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [Header("Setting")]
    public Transform weaponViot;

    private Player player;

    [Header("Skill Game Object")]
    public GameObject wavePrefab;
    public GameObject thunder;

    GameManager GM;

    private void Start()
    {
        GM = GameManager.instance;
        player = GetComponent<Player>();
    }
    public void ActivateSkill(SpellCard spellCard)
    {
        string skillName = spellCard.itemName;

        switch (skillName)
        {
            case "Thunder Stomp":
                ThunderSpellBook(spellCard);
                break;
        }
    }

    void ThunderSpellBook(SpellCard spellCard)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float currentAngle = FloatValue.AngleRotate2Target(weaponViot.position, mousePos);

        float startAngle = currentAngle - 10;
        float angleStep = 10;

        int damage = GameManager.instance.playerStat.ATK;

        for (int i = 0; i < 1; i++)
        {
            GameObject obj = Instantiate(spellCard.bullet, transform.position, Quaternion.identity);
            obj.GetComponent<TrailBullet>().SetData(thunder, damage, spellCard.element);
            obj.GetComponent<Rigidbody2D>().velocity = currentAngle.ToDirection() * spellCard.bulletSpeed;

            startAngle += angleStep;
        }
    }
}
