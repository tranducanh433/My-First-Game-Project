using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GreenSlime
{
    public enum KindOfMovement { topDown, platformer }
    public class PlayerUtilities : MonoBehaviour
    {
        

        //Movement
        protected float PlatformerMovementInput()
        {
            float change = Input.GetAxisRaw("Horizontal");

            return change;
        }
        protected Vector2 TopDownMovementInput()
        {
            Vector2 change = Vector3.zero;
            change.x = Input.GetAxisRaw("Horizontal");
            change.y = Input.GetAxisRaw("Vertical");

            change.Normalize();

            return change;
        }

        protected void Player2DMove(Rigidbody2D rb, Vector2 moveInput, float speed)
        {
            rb.MovePosition(rb.position + moveInput * speed * Time.fixedDeltaTime);
        }
        protected void Player2DMove(Rigidbody2D rb, float moveInput, float speed)
        {
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        }
        protected void Player2DJump(Rigidbody2D rb, KeyCode jumpKey, float force)
        {
            if (Input.GetKeyDown(jumpKey))
            {
                rb.velocity = Vector2.up * force;
            }
        }

        //Quick Add Component
        [ContextMenu("Add Component for 2D GameObject")]
        void AddComponentFor2DGameObject()
        {
            gameObject.AddComponent<Rigidbody2D>();
            gameObject.AddComponent<BoxCollider2D>();
            gameObject.AddComponent<BoxCollider2D>();
            gameObject.AddComponent<Animator>();

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.freezeRotation = true;
        }
    }
}