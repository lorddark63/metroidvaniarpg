using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    Enemy enemy;
    public bool isDamaged;
    public GameObject deathEffect;
    Blink materials;
    SpriteRenderer sr;
    Rigidbody2D rb;

    public float originalHealth;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        materials = GetComponent<Blink>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        originalHealth = enemy.health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.CompareTag("weapon") && !isDamaged)
        {
            enemy.health -= coll.GetComponent<SwordStats>().DamageInput(enemy.defense, this.transform);
            EnemyDamaged(coll);
            
        }

        if(coll.CompareTag("bullet") && !isDamaged)
        {
            enemy.health -= coll.GetComponent<LongRangeWeaponStats>().DamageInput(enemy.defense, this.transform);
            EnemyDamaged(coll);
        }

        if(coll.CompareTag("specialAttack") && !isDamaged)
        {
            print("collidiu");
            enemy.health -= coll.GetComponent<SpecialAttackSettings>().damage;
            coll.GetComponent<SpecialAttackSettings>().DamageTxt(this.transform);
            EnemyDamaged(coll);
        }
    }

    void EnemyDamaged(Collider2D coll)
    {
        AudioManager.instance.PlayAudio(AudioManager.instance.hit);
        KnockBack(coll);
        StartCoroutine(Damager());
        if(enemy.health <= 0)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            Experience.instance.ExpModifier(GetComponent<Enemy>().exp);
            AudioManager.instance.PlayAudio(AudioManager.instance.enemyDead);
            
            if(enemy.shouldRespaw)
            {
                transform.GetComponentInParent<Respaw>().StartCoroutine(transform.GetComponentInParent<Respaw>().RespawEnemy());
            }
            else
            {
                Destroy(gameObject);
            }
            
        }
    }

    void KnockBack(Collider2D coll)
    {
        if(coll.transform.position.x < transform.position.x)
            rb.AddForce(new Vector2(enemy.knockbackForceX, enemy.knockbackForceY), ForceMode2D.Force);
        else
            rb.AddForce(new Vector2(-enemy.knockbackForceX, enemy.knockbackForceY), ForceMode2D.Force);
    }

    IEnumerator Damager()
    {
        isDamaged = true;
        for (int i = 0; i < enemy.stunTime; i++)
        {
            sr.material = materials.blink;
            yield return new WaitForSeconds(0.1f);
            sr.material = materials.original;
            yield return new WaitForSeconds(0.1f);
        }
        isDamaged = false;
    }
}


