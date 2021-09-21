using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Spells : MonoBehaviour
{
    public float maxMana;
    public float currentMana;

    public bool[] isColldown;

    Transform enemyPos;
    public float firePunchDuration = 0.3f;
    public GameObject iceAttack;
    PlayerDash dash;

    bool attackCharge;
    bool attackForwardPunch;

    bool firstIceSpike;
    bool hasEnemyOnScene;

    int extraIceAttack;
    Vector3 detectionPos;

    public Image[] colldownImg;

    
    public int extraIceAttackValue;

    public bool slot1IsColldown;
    public bool slot2IsColldown;

    public GameObject spellSlot1;
    public GameObject spellSlot2;


    // Start is called before the first frame update
    void Start()
    {
        currentMana = maxMana;
        extraIceAttack = extraIceAttackValue;

        for (int i = 0; i < colldownImg.Length; i++)
        {
            colldownImg[i].fillAmount = 0;
        }

        for (int i = 0; i < isColldown.Length; i++)
        {
            isColldown[i] = false;
        }

        FindClosestEnemy();
        dash = GetComponent<PlayerDash>();
        firstIceSpike = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        UiManager.instance.ManaFillup(currentMana, maxMana);
        if(currentMana < 0)
        {
            currentMana = 0;
        }
        FirePunchAnimation();
        FindClosestEnemy();
        DeactiveSpellSlots();
    }

    void DeactiveSpellSlots()
    {
        
             spellSlot1.GetComponent<EventTrigger>().enabled = !slot1IsColldown;
             spellSlot1.GetComponent<Button>().interactable = !slot1IsColldown;
      
             spellSlot2.GetComponent<EventTrigger>().enabled = !slot2IsColldown;
             spellSlot2.GetComponent<Button>().interactable = !slot2IsColldown;
        
    
    }

    public void CollDown(int collDownSlot, float colldownTime, int isCooldoidId)
    {
        //iscooldown id 0 = firePunch, 1 = icespikes
        if(isColldown[isCooldoidId])
        {
            colldownImg[collDownSlot].fillAmount -= 1 / colldownTime * Time.deltaTime;
            print("slot hud:" + collDownSlot + " time:" + colldownTime + " id:" + isCooldoidId);

            if(colldownImg[collDownSlot].fillAmount <= 0)
            {
                colldownImg[collDownSlot].fillAmount = 0;
                isColldown[isCooldoidId] = false;
            }
        }

        
            if(collDownSlot == 0)
            {
                if(isColldown[isCooldoidId] == true)
                    slot1IsColldown = true;
                else
                    slot1IsColldown = false;
            }

            if(collDownSlot == 1)
            {
                if(isColldown[isCooldoidId] == true)
                    slot2IsColldown = true;
                else
                    slot2IsColldown = false;
            }
            
        
        
    }

    public IEnumerator Dash()
    {
        this.gameObject.layer = 13;
        attackCharge = true;
        yield return new WaitForSeconds(0.4f);
        dash.StartDash();
        attackCharge = false;
        attackForwardPunch = true;
        yield return new WaitForSeconds(firePunchDuration);
        dash.StopDash();
        attackForwardPunch = false;
        PlayerHealth.instance.isDamaged = false;
        this.gameObject.layer = 8;
    }

    void FirePunchAnimation()
    {
        Animator player = PlayerController.instance.anim;
        player.SetBool("attackCharge", attackCharge);
        player.SetBool("attackPunch", attackForwardPunch);
    }

    public void FindClosestEnemy()
    {
        float distanceClosestEnemy = Mathf.Infinity;
        EnemyMove closestEnemy = null;
        EnemyMove[] allEnemies = GameObject.FindObjectsOfType<EnemyMove>();
        
        foreach(EnemyMove currentEnemy in allEnemies)
        {
            if(currentEnemy != null)
            {
                float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
                if(distanceToEnemy < distanceClosestEnemy && distanceToEnemy <= 35)
                {
                    
                    hasEnemyOnScene = true;
                    distanceClosestEnemy = distanceToEnemy;
                    closestEnemy = currentEnemy;
                    enemyPos = closestEnemy.transform;
                    Debug.DrawLine (this.transform.position, closestEnemy.transform.position);
                }
                else
                {
                    enemyPos = null;
                    hasEnemyOnScene = false;
                }
            }
            else
            {
                hasEnemyOnScene = false;
            }
        }

    }

    public void IceAttack()
    {
        StartCoroutine(IceAttackMultipleTimes());
    }

    public IEnumerator IceAttackMultipleTimes()
    {

        if(hasEnemyOnScene)
        {
            if(enemyPos != null)
                detectionPos = new Vector3(enemyPos.position.x, enemyPos.GetComponent<Collider2D>().bounds.min.y, enemyPos.position.z);
            else
            {
                enemyPos = null;
                hasEnemyOnScene = false;
                detectionPos = PlayerController.instance.DetectGroundByRaycast().transform.position;
            }
        }
        else
        {
            if(PlayerController.instance.DetectGroundByRaycast())
                detectionPos = PlayerController.instance.DetectGroundByRaycast().transform.position;
            
        }

        while(extraIceAttack > 0)
        {
           
            float spikeDistance = 1f; 

            if(firstIceSpike)
            {
                firstIceSpike = false;
            }

            GameObject spikes = Instantiate(iceAttack, detectionPos, Quaternion.identity);
            Vector3 temp = spikes.GetComponent<Transform>().position;
            detectionPos = new Vector3(temp.x + spikeDistance, temp.y, temp.z);

            extraIceAttack--;
            spikeDistance += spikeDistance;

            yield return new WaitForSeconds(0.8f);
        }
        extraIceAttack = extraIceAttackValue;
        firstIceSpike = true;
    }
}
