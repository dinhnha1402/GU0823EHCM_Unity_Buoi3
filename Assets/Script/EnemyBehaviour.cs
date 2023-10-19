using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform target;

    [SerializeField]
    private float speed = 1f;
    private float rotationSpeed = 0.025f;

    Animator animator;

    private Rigidbody2D rb;

    private float distance;



    [SerializeField]
    private Transform hpBar;
    private Vector2 hpScale;

    public float Health
    {
        set
        {
            if (value < health)
            {
                animator.SetTrigger("Hit");
            }

            health = value;

            if (health <= 0)
            {
                Defeated();
            }
        }
        get
        { 
            return health; 
        }
    }

    float health;

    public float maxtHP;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        getTarget();

        hpScale = hpBar.localScale;
        Health = maxtHP;
    }

    void Update()
    {
        if (!target)
        {
            getTarget();
        }
        else
        {
            //RotationEnemyFace();
        }
    }

    private void FixedUpdate()
    {
        distance = Vector2.Distance(transform.position, target.position);
        Vector2 direction = (target.position - transform.position);

        transform.position = Vector2.MoveTowards(this.transform.position, target.position, speed * Time.deltaTime);

    }

    private void RotationEnemyFace()
    {
        Vector2 targetDirection = target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90F;
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, q, rotationSpeed);
    }

    private void getTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Defeated()
    {
        animator.SetTrigger("Defeated");
    }

    public void RemoveEnemy()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerBehavior playerBehavior = collision.gameObject.GetComponent<PlayerBehavior>();
            playerBehavior.ReceiveDamage(20);
        }
    }

    public void ReceiveDamage(float damage)
    {
        Health = Health - damage;

        if (Health <= 0)
        {
            hpBar.localScale = new Vector2(0, hpScale.y);
            return;
        }

        float newScale = hpScale.x * (Health / maxtHP);
        hpBar.localScale = new Vector2(newScale, hpScale.y);


    }

}
