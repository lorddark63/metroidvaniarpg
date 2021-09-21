using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjMovement : MonoBehaviour
{
    public Transform pointA, pointB;
    public float speed;
    public bool canMove;
    public bool canWait;
     public bool isLever;
     public bool willDestroy;

    public float waitTime;
    public bool startCd;
    public float timeToDestroy;
    public LeverActivation leverOn;

    [HideInInspector] 
    public float destroyCd;
    bool continueToMove;
    bool moveToA;
    bool moveToB;


    void Start()
    {
        moveToA = true;
        moveToB = false;
        continueToMove = true;
        destroyCd = timeToDestroy;
    }

    void Update()
    {
        if(canMove)
            MoveObject();

        if(startCd)
            DestroyPlatform();
    }

    void FixedUpdate()
    {
        if(isLever)
            MoveByLever();
    }

    public void DestroyPlatform()
    {
        destroyCd -= Time.deltaTime;
        if(destroyCd <= 0)
        {
            StartCoroutine(ReactivePlatform());
            destroyCd = timeToDestroy;
            startCd = false;
        }
    }

    IEnumerator ReactivePlatform()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        BoxCollider2D[] colliders = gameObject.GetComponents<BoxCollider2D>();

        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }

        yield return new WaitForSeconds(2);

        foreach (var collider in colliders)
        {
            collider.enabled = true;
        }

        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    private void MoveByLever()
    {
        if(leverOn.leverOn)
            transform.position = Vector2.MoveTowards(transform.position, pointA.position, speed * Time.deltaTime);
        else
            transform.position = Vector2.MoveTowards(transform.position, pointB.position, speed * Time.deltaTime);
    }

    private void MoveObject()
    {
        float distanceToA = Vector2.Distance(transform.position, pointA.position);
        float distanceToB = Vector2.Distance(transform.position, pointB.position);

        if(distanceToA > 0.1 && moveToA)
        {
            transform.position = Vector2.MoveTowards(transform.position, pointA.position, speed * Time.deltaTime);
            if(distanceToA < 0.3f && continueToMove)
            {
                moveToA = false;
                moveToB = true;

                if(canWait)
                {
                    StartCoroutine(Waiter());   
                }
        
            }
        }

        if(distanceToB > 0.1 && moveToB)
        {
            transform.position = Vector2.MoveTowards(transform.position, pointB.position, speed * Time.deltaTime);
            if(distanceToB < 0.3f && continueToMove)
            {
                moveToA = true;
                moveToB = false;

                if(canWait)
                {
                    StartCoroutine(Waiter());    
                }        
            }
        }
    }

    IEnumerator Waiter()
    {
        canMove = false;
        continueToMove = false;
        yield return new WaitForSeconds(waitTime);
        canMove = true;
        continueToMove = true;
    }


}
