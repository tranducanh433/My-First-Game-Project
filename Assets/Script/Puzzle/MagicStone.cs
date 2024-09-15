using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicStone : MonoBehaviour
{
    bool notMoving = true;

    public void MoveRight()
    {
        if(CanMove(Vector2.right) && notMoving)
        {
            StartCoroutine(MoveTo(transform.position + Vector3.right));
        }
    }
    public void MoveLeft()
    {
        if (CanMove(Vector2.left) && notMoving)
        {
            StartCoroutine(MoveTo(transform.position + Vector3.left));
        }
    }
    public void MoveUp()
    {
        if (CanMove(Vector2.up) && notMoving)
        {
            StartCoroutine(MoveTo(transform.position + Vector3.up));
        }
    }
    public void MoveDown()
    {
        if (CanMove(Vector2.down) && notMoving)
        {
            StartCoroutine(MoveTo(transform.position + Vector3.down));
        }
    }

    bool CanMove(Vector2 dir)
    {
        return !Physics2D.Linecast(transform.position, (Vector2)transform.position + dir);
    }

    IEnumerator MoveTo(Vector2 target)
    {
        while ((Vector2)transform.position != target)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, 10 * Time.deltaTime);
            yield return null;
        }
        notMoving = true;
    }
}
