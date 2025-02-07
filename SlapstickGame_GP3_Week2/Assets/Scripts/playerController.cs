using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour
{
    public Animator noMoreJumpAnim;
    public Rigidbody2D RB;

    public float Speed = 5;
    public float jumpForce = 10;
    public float rotationSpeed = 50f;

    public Vector2 boxSize;
    public float castDistance;
    public LayerMask groundLayer;

    Vector2 movement;

    public float numberOfJumps;
    private float jumps;
    int buildIndex;

    private void Awake()
    {
        //noMoreJumpAnim.enabled = false;
        jumps = numberOfJumps;

        RB = GetComponent<Rigidbody2D>();
        buildIndex = SceneManager.GetActiveScene().buildIndex;
    }

    void Update()
    {
        playerMovement();
    }

    private void playerMovement()
    {
        //Horizontal Movement
        RB.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * Speed, RB.linearVelocity.y);

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.D))
        {
            //transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
            RB.MoveRotation(90);
        }

        //Jumping
        if (Input.GetKeyDown(KeyCode.Space) && jumps > 0)
        {
            jumps--;
            Debug.Log("Jump key pressed");
            RB.linearVelocity = new Vector2(RB.linearVelocity.x, jumpForce);

            //Play animation when out of jumps
            if (jumps <= 0 && !isGrounded())
            {
                noMoreJumpAnim.enabled = true;
                noMoreJumpAnim.Play("noMoreJumps");
            }
        }

        //Replenish jumps when on ground 
        if (isGrounded())
        {
            //Offset with - 1 because the boxcast will detect still on ground sometime after jumping
            jumps = numberOfJumps - 1;
        }
    }
    public bool isGrounded()
    {
        //Boxcast for detecting when player is on ground
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        //Makes BoxCast visible in Unity Editor
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag == "Exit")
        {
            //Loads next scene in buildIndex
            SceneManager.LoadScene(buildIndex + 1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.tag == "Hazard")
        {
            Destroy(gameObject);

            //Reloads Current Scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
