using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D coll)
    {
       if(coll.CompareTag("Player"))
       {
           ObjMovement objMovement = gameObject.GetComponent<ObjMovement>();
           if(objMovement.willDestroy)
           {
               objMovement.startCd = true;
           }
           coll.transform.SetParent(this.transform);
       } 
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if(coll.CompareTag("Player"))
        {
            coll.transform.SetParent(null);
        } 
    }
}
