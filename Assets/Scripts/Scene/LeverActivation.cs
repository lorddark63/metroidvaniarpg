using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverActivation : MonoBehaviour
{
    public bool leverOn;
    bool interactArea;
    public GameObject lever;
    public GameObject objectToActivate;

    void Start()
    {

    }

    void Update()
    {
        if(interactArea)
        {
            if(Input.GetKeyDown(KeyCode.Z))
            {   
                leverOn = !leverOn; 
            }
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.CompareTag("Player"))
        {
            interactArea = true;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if(coll.CompareTag("Player"))
        {
            interactArea = false;
        }
    }

}
