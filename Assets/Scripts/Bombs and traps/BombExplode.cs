using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplode : MonoBehaviour
{
    [SerializeField] private bool colide = false;
    [SerializeField] private float range;
    [SerializeField] private LayerMask layer;
    [SerializeField] private GameObject animBomb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(colide)
        {
            Instantiate(animBomb, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        colide = Physics2D.OverlapCircle(transform.position, range, layer);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
