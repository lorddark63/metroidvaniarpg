using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBombProjectile : MonoBehaviour
{
    [SerializeField] bool isArrow;
    [SerializeField] GameObject tower;
    public GameObject target;
    public int targetId;
    [SerializeField] float speed = 10f;

    float towerX;
    float targetX;

    float distance;
    float nextX;
    float baseY;
    float height;

    bool targetHasHited = false;


    // Start is called before the first frame update
    void Start()
    {
        tower = GameObject.FindGameObjectWithTag("tower");
//        target = GameObject.FindObjectOfType<TurretArrow>().allEnemies[targetId].gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
            Shoot();
    }

    public void Shoot()
    {
        if(targetHasHited)
         return;

        towerX = tower.transform.position.x;
        targetX = target.transform.position.x;

        distance = targetX - towerX;
        nextX = Mathf.MoveTowards(transform.position.x, targetX, speed * Time.deltaTime);
        baseY = Mathf.Lerp(tower.transform.position.y, target.transform.position.y, (nextX - towerX) / distance);
        height = 2 * (nextX - towerX) * (nextX - targetX) / (-1.5f * distance * distance);
        
        Vector3 movePos = new Vector3(nextX, baseY + height, transform.position.z);
        transform.rotation = LookAtTarget(movePos - transform.position);
        transform.position = movePos;

        if(transform.position == target.transform.position)
        {
            if(isArrow)
            {
                gameObject.transform.SetParent(target.transform);
                print("acertou miseravi");
                targetHasHited = true;
                Destroy(gameObject, 3);
            }
            else
            {
                Destroy(gameObject);
            }
            
        }
    }

    public static Quaternion LookAtTarget(Vector2 rotation)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg);
    }
}
