using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour
{
    [SerializeField] private ParticleSystem jumpEffect = default;
    [SerializeField] private ParticleSystem deathExplosion = default;
    [SerializeField] private ParticleSystem lastJumpEffect = default;
    public TextMeshProUGUI jumpCountDisplay;
    public TextMeshProUGUI scoreDisplay;

    public Rigidbody2D RB;
    public Transform target;

    public float Speed = 5;
    public float jumpForce = 10;
    int Score = 0;

    public Vector2 boxSize;
    public float castDistance;
    public LayerMask groundLayer;

    Vector2 movement;

    public float totalJumps;

    [HideInInspector]
    public float remainingJumps;

    int buildIndex;

    private void Awake()
    {
        //noMoreJumpAnim.enabled = false;
        remainingJumps = totalJumps;

        RB = GetComponent<Rigidbody2D>();
        buildIndex = SceneManager.GetActiveScene().buildIndex;
    }

    void Update()
    {
        playerMovement();

        if (jumpCountDisplay != null)
            jumpCountDisplay.text = "Jumps: " + remainingJumps.ToString();
        if (scoreDisplay != null)
            scoreDisplay.text = "Score: " + Score.ToString();

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }
    private void playerMovement()
    {
        //Horizontal Movement
        RB.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * Speed, RB.linearVelocity.y);

        //Jumping
        if (Input.GetKeyDown(KeyCode.Space) && remainingJumps > 0)
        {
            remainingJumps--;
            RB.linearVelocity = new Vector2(RB.linearVelocity.x, jumpForce);

            if (remainingJumps > 0)
                jumpEffect.Play();
            
            if (remainingJumps == 0)
                lastJumpEffect.Play();
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
        //give player an extra jump when colliding with a powerup
        if (collider2D.gameObject.tag == "PowerUp")
        {
            remainingJumps++;
            Score += 5;
            Destroy(collider2D.gameObject);
        }
        //go to next scene when colliding with exit object
        if (collider2D.gameObject.tag == "Exit")
        {
            SceneManager.LoadScene(0);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.tag == "Ground")
        {
            deathExplosion.Play();
            gameObject.SetActive(false);
        }
        if (collision2D.gameObject.tag == "Hazard")
        {
            deathExplosion.Play();
            gameObject.SetActive(false);
        }
    }
}
