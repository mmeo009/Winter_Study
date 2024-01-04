using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Rigidbody theRB;
    public float moveSpeed;
    private Transform target;

    public float damage;

    public float hitWaitTime = 1f;
    private float hitCounter;

    public float health = 5f;

    public float knockBackTime = 0.5f;
    public float knockBackCounter;

    public int expToGive = 1;

    public int coinValue = 1;
    public float coinDropRate = 0.5f;

    private Transform player;

    void Start()
    {
        player = PlayerController.instance.transform;
        target = player;
    }

    void Update()
    {
        if(player.gameObject.activeSelf == true)
        {
            if(knockBackCounter > 0)
            {
                knockBackCounter -= Time.deltaTime;

                if(moveSpeed > 0)
                {
                    moveSpeed = -moveSpeed * 2f;
                }

                if(knockBackCounter < 0)
                {
                    moveSpeed = Mathf.Abs(moveSpeed * 0.5f);
                }
            }

            if (Vector3.Distance(target.position, transform.position) > 1.0f)
            {
                theRB.velocity = (target.position - transform.position).normalized * moveSpeed;
            }

            if(hitCounter > 0f)
            {
                hitCounter -= Time.deltaTime;
            }
        }
        else
        {
            theRB.velocity = Vector3.zero;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && hitCounter <= 0f)
        {
            //playerHealthController.instance.TakeDamage(damage);
            hitCounter = hitWaitTime;
        }
    }

    public void TakeDamage(float damageToTake)
    {
        health -= damageToTake;

        if(health <= 0f)
        {
            Destroy(gameObject);
            //TODO ExprienceLevelController 구현
        }
        else
        {
            //TODO SFXManager (데미지 받는 소리 등)
        }

        //TODO DamageNumberController 구현
    }

    public void TakeDamage(float damageToTake, bool shouldKnockBack)
    {
        TakeDamage(damageToTake);

        if(shouldKnockBack == true)
        {
            knockBackCounter = knockBackTime;
        }
    }
}
