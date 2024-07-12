using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float health = 100.0f;         // Health of the monster
    public float damagePerSecond = 10.0f; // Damage dealt to the player per second
    public float attackRange = 2.0f;      // Range at which the monster will attack the player
    public string playerTag = "Player";   // Tag of the player

    private GameObject player;            // Reference to the player
    private bool isAttacking = false;     // Is the monster currently attacking?
    private bool isDead = false;          // Is the monster dead?

    void Start()
    {
        player = GameObject.FindGameObjectWithTag(playerTag);
    }

    void Update()
    {
        if (isDead)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= attackRange && !isAttacking)
        {
            StartAttacking();
        }
        else if (distanceToPlayer > attackRange && isAttacking)
        {
            StopAttacking();
        }
    }

    void StartAttacking()
    {
        isAttacking = true;
        // animator.SetBool("isAttacking", true);
        InvokeRepeating("DealDamage", 0f, 1f);
    }

    void StopAttacking()
    {
        isAttacking = false;
        // animator.SetBool("isAttacking", false);
        CancelInvoke("DealDamage");
    }

    void DealDamage()
    {
        if (player != null)
        {
            player.SendMessage("ChangeHealth", -damagePerSecond, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void ChangeHealth(float amount)
    {
        health += amount;

        if (health <= 0 && !isDead)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        StopAttacking();
        // animator.SetTrigger("isDead");
    }
}