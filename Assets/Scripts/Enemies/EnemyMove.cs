using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
	float speed;
	Rigidbody2D rb;
	Animator anim;

	public bool isStatic;
	public bool isWalker;
	public bool isPatrol;
	public bool walkRight;
	public bool isWaiting;
	public bool shouldWait;
	public float waitTime;

	public Transform wallCheck, pitCheck, groundCheck;
	public bool wallDetect, pitDetect, groundDetect;
	public float detectRadius;
	public LayerMask groundLayer;

	public Transform pointA, pointB;
	bool goToA, goToB;


    // Start is called before the first frame update
    void Start()
    {
    	goToA = true;
        speed = GetComponent<Enemy>().speed;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Detections();
    }

    void Detections()
    {
    	pitDetect = !Physics2D.OverlapCircle(pitCheck.position, detectRadius, groundLayer);
    	wallDetect = Physics2D.OverlapCircle(wallCheck.position, detectRadius, groundLayer);
    	groundDetect = Physics2D.OverlapCircle(groundCheck.position, detectRadius, groundLayer);
    	if((pitDetect || wallDetect) && groundDetect)
    		Flip();
    }

    private void FixedUpdate()
    {
    	if(isStatic)
    	{
    		anim.SetBool("idle", true);
    		rb.constraints = RigidbodyConstraints2D.FreezeAll;
    	}

    	if(isWalker)
    	{
    		rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    		anim.SetBool("idle", false);
    		if(!walkRight)
    		{
    			rb.velocity = new Vector2(-speed * Time.deltaTime, rb.velocity.y);
    		}
    		else
    		{
    			rb.velocity = new Vector2(speed * Time.deltaTime, rb.velocity.y);
    		}
    	}

    	if(isPatrol)
    	{


    		if(goToA)
    		{
				if(!isWaiting)
				{
					anim.SetBool("idle", false);
					rb.velocity = new Vector2(-speed * Time.deltaTime, rb.velocity.y);
				}

    			if(Vector2.Distance(transform.position, pointA.position) < 0.2f)
    			{
					if(shouldWait)
					{
						StartCoroutine(Waiting());
					}

    				Flip();
    				goToA = false;
    				goToB = true;
    			}
    		}

    		if(goToB)
    		{
				if(!isWaiting)
				{
					anim.SetBool("idle", false);
					rb.velocity = new Vector2(speed * Time.deltaTime, rb.velocity.y);
				}

    			if(Vector2.Distance(transform.position, pointB.position) < 0.2f)
    			{

					if(shouldWait)
					{
						StartCoroutine(Waiting());
					}

    				Flip();
    				goToA = true;
    				goToB = false;
    			}
    		}
    	}
    }

	IEnumerator Waiting()
	{
		anim.SetBool("idle", true);
		isWaiting = true;
		Flip();
		yield return new WaitForSeconds(waitTime);
		isWaiting = false;
		anim.SetBool("idle", false);
		Flip();
	}

    public void Flip()
    {
    	walkRight = !walkRight;
    	transform.localScale *= new Vector2(-1, transform.localScale.y);
    }
}
