using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningBomb : MonoBehaviour
{
    private float velMove = 2;
    [SerializeField] private int babysAmount = 1;
    private Rigidbody2D rb;
    float dir;

    [SerializeField] private float distanceEnemy;
    [SerializeField] private float distanceExplode;
    [SerializeField] private LayerMask layer;

    [SerializeField] private GameObject bombChildrens;

    Animator anim;
    bool stopMove = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        CheckDir();
        Flip();
    }

    // Update is called once per frame
    void Update()
    {
        if(stopMove)
            return;

        Move();
        EnemyDetector();
    }

    void CheckDir()
    {
        if(PlayerController.instance.isLookingRight)
            dir = velMove;
        else
            dir = -velMove;
    }

    void EnemyDetector()
    {
        float distanceEnemyDir;
        float detectExplosionDir;

        if(PlayerController.instance.isLookingRight)
        {
            distanceEnemyDir = distanceEnemy;
            detectExplosionDir = distanceExplode;
        }
        else
        {
            distanceEnemyDir = -distanceEnemy;
            detectExplosionDir = -distanceExplode;
        }
        RaycastHit2D detectEnemy = Physics2D.Raycast(transform.position, Vector2.right, distanceEnemyDir, layer);
        Debug.DrawRay(transform.position, Vector2.right * distanceEnemyDir, Color.green);

        RaycastHit2D detectExplosion = Physics2D.Raycast(transform.position, Vector3.forward, detectExplosionDir, layer);
        Debug.DrawRay(transform.position, Vector3.right * detectExplosionDir, Color.red);

       // explodeDetection = Physics2D.OverlapCircle(transform.position, explosionRange, layer);

        if(detectEnemy)
        {
            print(detectEnemy.transform.name);
            anim.SetBool("doneToExplode", true);
        }

        if(detectExplosion)
        {
            anim.SetBool("explode", true);
            rb.velocity = Vector2.zero;
            stopMove = true;
        }

       
    }

    void Move()
    {
        rb.velocity = new Vector2(dir, rb.velocity.y);
    }

    void Flip()
    {
        Vector3 temp = transform.localScale;

        if(PlayerController.instance.isLookingRight)
        {
            temp.x = -Mathf.Abs(temp.x);
        }
        else
        {
            temp.x = Mathf.Abs(temp.x);
        }

        transform.localScale = temp;
    }

    void GoodjobMyMan()
    {
        Destroy(gameObject);
    }

    void SpwanChildresn()
    {
        for (int i = 0; i < babysAmount; i++)
        {
            GameObject tempChildren = Instantiate(bombChildrens, transform.position, Quaternion.identity);
            int ramdomX = Random.Range(-25, 25);
            int ramdomY = Random.Range(150, 200);
            tempChildren.GetComponent<Rigidbody2D>().AddForce(new Vector2(ramdomX, ramdomY));

            if(ramdomX > 0)
                tempChildren.GetComponent<RunningBombBabys>().isFaceRight= true;
            else
                tempChildren.GetComponent<RunningBombBabys>().isFaceRight= false;
        }
    }
}
