using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	public static PlayerHealth instance;
	public float health;
	public float maxHealth;
	public bool isDamaged;
	public bool isDead;
	public float stunTime;
	public float knockbackForceX;
	public float knockbackForceY;
	Blink materials;

	public GameObject gameOver;

	SpriteRenderer sr;
	Rigidbody2D rb;

	void Awake()
	{
		if(instance == null)
			instance = this;
	}

    // Start is called before the first frame update
    void Start()
    {
		gameOver.SetActive(false);
        health = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        materials = GetComponent<Blink>();

		maxHealth = PlayerPrefs.GetFloat("maxHealth", maxHealth);

		if(!isDead)
		{
			gameOver.GetComponent<CanvasGroup>().alpha = 0;
		}
    }

    // Update is called once per frame
    void Update()
    {
		UiManager.instance.PlayerHealthBar(health, maxHealth);
		
		IsDead();
        if(health > maxHealth)
        	health = maxHealth;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
    	if(coll.CompareTag("enemy") && !isDamaged)
    	{
    		float damage = coll.GetComponent<Enemy>().forceDamage;
			health -= damage;
    		UiManager.instance.PlayerHealthDamage(health, damage);
    		StartCoroutine(Imunity());

    		if(coll.transform.position.x > transform.position.x)
    		{
    			rb.AddForce(new Vector2(-knockbackForceX, knockbackForceY), ForceMode2D.Force);
    		}
    		else
    		{
    			rb.AddForce(new Vector2(knockbackForceX, knockbackForceY), ForceMode2D.Force);
    		}

    		if(health <= 0)
    		{
				AudioManager.instance.PlayAudio(AudioManager.instance.playerDead);
				isDead = true;
    			
            	AudioManager.instance.PlayAudio(AudioManager.instance.gameOver);
    		}
    	}
    }

	public void IsDead()
	{
		if(isDead)
		{
			Time.timeScale = 0;
			gameOver.SetActive(true);
			AudioManager.instance.bgMusic.Stop();

			if(gameOver.GetComponent<CanvasGroup>().alpha < 1)
			{
				gameOver.GetComponent<CanvasGroup>().alpha += 0.005f;
			}
		}
	}

    IEnumerator Imunity()
    {
    	isDamaged = true;
        for (int i = 0; i < stunTime; i++)
        {
            sr.material = materials.blink;
            yield return new WaitForSeconds(0.1f);
            sr.material = materials.original;
            yield return new WaitForSeconds(0.1f);
        }
        isDamaged = false;
    }
}
