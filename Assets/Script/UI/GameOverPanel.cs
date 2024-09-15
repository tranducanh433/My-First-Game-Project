using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    public void RetryButton()
    {
        GameManager.instance.ResumeGame();
        GameObject.Find("Player").GetComponent<Player>().RevivePlayer();
        StartCoroutine(ReviveCO());
    }

    IEnumerator ReviveCO()
    {
        yield return new WaitForSeconds(0.05f);
        DestroyAllEnemyAndBullet();
        gameObject.SetActive(false);
    }

    void DestroyAllEnemyAndBullet()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");

        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }
    }
}
