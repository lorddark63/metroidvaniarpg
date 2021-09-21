using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurretArrow : MonoBehaviour
{
    public int towerType; //0 = arow, 1 = bomb
    public float health;
    [SerializeField] private float force;
    [SerializeField] private float fireRate;
    [SerializeField] private int projectilesAmount;
    int currentArrowsAmount;

    [SerializeField] private float rechargeTime;
    private float nextFire;
    [SerializeField] private Transform ammoPlace;
    [SerializeField] private float distanceRay;
    [SerializeField] private LayerMask layer;

    [SerializeField] private GameObject bombObj;
    [SerializeField] private GameObject arrowObj;
    [SerializeField] private int multipleArrowsToShot = 1;

    [SerializeField] private Transform wayPoint;

    [SerializeField] private GameObject explosion;
    [SerializeField] private GameObject damageEffect;

    [SerializeField] private TextMeshProUGUI arrowTxt;
    [SerializeField] private GameObject loadingImg;

    bool isFaceRight;
    bool isDamaged;
    bool isRecharging;

    public bool isGrounded;
    public Transform groundCheck;
    public float detectRadius;
	public LayerMask groundLayer;

    public EnemyMove[] allEnemies;
 


    // Start is called before the first frame update
    void Start()
    {
        currentArrowsAmount = projectilesAmount;
        isFaceRight = PlayerController.instance.isLookingRight;
        loadingImg.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        MultipleTargets();
        GroundDetectin();
        EnemyDetector();
    }

    void GroundDetectin()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, detectRadius, groundLayer);
    }

    void EnemyDetector()
    {
        float distanceDir;

        if(isFaceRight)
            distanceDir = distanceRay;
        else
            distanceDir = -distanceRay;

        RaycastHit2D detectEnemy = Physics2D.Raycast(wayPoint.position, Vector2.right, distanceDir, layer);
        Debug.DrawRay(wayPoint.position, Vector2.right * distanceDir, Color.green);


        if(detectEnemy && !isRecharging && isGrounded)
        {
            if(currentArrowsAmount > 0)
            {
                if(Time.time > nextFire)
                {
                    nextFire = Time.time + fireRate;
                    if(towerType == 0)
                        ShootArrow();
                    else if(towerType == 1)
                        ShootBomb();
                }
            }
            else 
            {
                StartCoroutine(RechargeArrows());
            }
        }
    }

    void MultipleTargets()
    {
        allEnemies = GameObject.FindObjectsOfType<EnemyMove>();
    }

    void ShootArrow()
    {
        currentArrowsAmount--;
        StartCoroutine(ShootArrowSet());

        arrowTxt.text = currentArrowsAmount.ToString();
    }

    IEnumerator ShootArrowSet()
    {
        
        for (int i = 0; i < multipleArrowsToShot; i++)
        {
            for (int j = 0; j < allEnemies.Length; j++)
            {
                GameObject temp = Instantiate(arrowObj, ammoPlace.position, ammoPlace.rotation) as GameObject;
                temp.GetComponent<TowerBombProjectile>().target = allEnemies[j].gameObject;
                yield return new WaitForSeconds(0.3f);   
            }
        }    
        
    }

    void ShootBomb()
    {
        currentArrowsAmount--;
        arrowTxt.text = currentArrowsAmount.ToString();
        GameObject temp = Instantiate(bombObj, ammoPlace.position, ammoPlace.rotation);
        temp.GetComponent<TowerBombProjectile>().target = allEnemies[allEnemies.Length - 1].gameObject;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.CompareTag("enemy") && !isDamaged)
        {
            float damage = coll.GetComponent<Enemy>().forceDamage;
			health -= damage;
            StartCoroutine(Imunity());

            if(health <= 0)
            {
                Instantiate(explosion, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

    IEnumerator Imunity()
    {
    	isDamaged = true;
        Instantiate(damageEffect, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
        isDamaged = false;
    }

    IEnumerator RechargeArrows()
    {
        isRecharging = true;

        arrowTxt.text = "";
        loadingImg.SetActive(true);

        yield return new WaitForSeconds(rechargeTime);
        currentArrowsAmount  = projectilesAmount;
        
        yield return new WaitForSeconds(0.5f);
        loadingImg.SetActive(false);
        arrowTxt.text = currentArrowsAmount.ToString();
        isRecharging = false;
    }
}
