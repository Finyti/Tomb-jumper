using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    bool inJump = false;
    public float jumpSpeed;
    Rigidbody2D rb;

    public GameObject camera;

    public LayerMask WallLayer;
    public LayerMask TrapLayer;
    public LayerMask CollectableLayer;

    public GameObject playerVisual;
    public Animator animator;

    public GameManager gameManager;

    public GameObject collectParticles;
    public GameObject collectStarParticles;

    public AudioClip dotAudio;
    public AudioClip coinAudio;
    public AudioClip starAudio;
    public AudioClip jumpAudio;
    public AudioClip dieAudio;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = playerVisual.GetComponent<Animator>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (inJump) return;
        if (Input.GetKeyDown(KeyCode.W) && !inJump)
        {
            inJump = true;
            PlayerJump(new Vector2(0, 1));

        }
        if (Input.GetKeyDown(KeyCode.A) && !inJump)
        {
            inJump = true;
            PlayerJump(new Vector2(-1, 0));
        }
        if (Input.GetKeyDown(KeyCode.S) && !inJump)
        {
            inJump = true;
            PlayerJump(new Vector2(0, -1));
        }
        if (Input.GetKeyDown(KeyCode.D) && !inJump)
        {
            inJump = true;
            PlayerJump(new Vector2(1, 0));
        }

    }


    public void PlayerJump(Vector2 direction)
    {
        inJump = true;
        animator.Play("PlayerFly");


        float flyAngle = 0;

        if(direction.y == -1) flyAngle = 0;
        else if (direction.y == 1) flyAngle = 180;
        
        if (direction.x == -1) flyAngle = -90;
        else if (direction.x == 1) flyAngle = 90;
        

        transform.eulerAngles = new Vector3(0, 0, flyAngle);
        transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));

        rb.AddForce(direction * 9.8f * jumpSpeed);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        SoundManager.Play(jumpAudio, 1f, 1f);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!inJump) return;

        var layerWall = LayerMask.NameToLayer("Walls");
        var layerTrap = LayerMask.NameToLayer("Trap");


        if (collision.gameObject.layer == layerWall)
        {
            animator.Play("Idle");

            inJump = false;
            transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));


            camera.transform.DOShakePosition(0.01f, 0.1f, 50);
            
        }
        if (collision.gameObject.layer == layerTrap)
        {
            gameManager.Die();
            SoundManager.Play(dieAudio, 1f, 1f);
            animator.Play("Idle");

            inJump = false;
            transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!inJump) return;

        if (collision.gameObject.CompareTag("Dot"))
        {
            SoundManager.Play(dotAudio, 1f, 1f);
            gameManager.CollectDot();
            Instantiate(collectParticles, collision.transform.position, Quaternion.identity);
            Destroy(collision.GameObject());

        }

        if (collision.gameObject.CompareTag("Coin"))
        {
            SoundManager.Play(coinAudio, 1f, 1f);
            gameManager.CollectCoin();
            Instantiate(collectParticles, collision.transform.position, Quaternion.identity);
            Destroy(collision.GameObject());
        }

        if (collision.gameObject.CompareTag("Star"))
        {
            SoundManager.Play(starAudio, 1f, 1f);
            gameManager.CollectStar();
            Instantiate(collectStarParticles, collision.transform.position, Quaternion.identity);
            Destroy(collision.GameObject());
        }

        if(collision.gameObject.CompareTag("Teleporter"))
        {
            gameManager.Win();
        }
    }
}
