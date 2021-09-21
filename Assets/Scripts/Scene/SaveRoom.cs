using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveRoom : MonoBehaviour
{
    bool interactArea;
    Animator anim;

    void Update()
    {
        if(interactArea)
        {
            if(Input.GetKeyDown(KeyCode.Z))
            {
                Experience.instance.DataToSave();
                print("jogo salvo");
                if(anim != null)
                    anim.SetTrigger("savingStance");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.CompareTag("Player"))
        {
            interactArea = true;
            anim = coll.GetComponent<Animator>();
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
