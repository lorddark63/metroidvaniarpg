using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEffect : MonoBehaviour
{
    public float ghostDelay;
    public float ghostDelaySeconds;
    public GameObject effect;
    public bool makeEffect = false;


    // Start is called before the first frame update
    void Start()
    {
        ghostDelaySeconds = ghostDelay;
    }

    // Update is called once per frame
    void Update()
    {
        GenerateEfect();
    }

    void GenerateEfect()
    {
        if(makeEffect)
        {
            if(ghostDelaySeconds > 0)
            {
                ghostDelaySeconds -= Time.deltaTime;
            }
            else
            {
                GameObject currentEffect = Instantiate(effect, transform.position, transform.rotation);
                Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;
                currentEffect.GetComponent<SpriteRenderer>().sprite = currentSprite;
                ghostDelaySeconds = ghostDelay;
                Destroy(currentEffect, 0.5f);
            }
        }
    }
}
