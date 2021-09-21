using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomControl : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.CompareTag("Player"))
        {
            transform.GetChild(0).gameObject.SetActive(true);
            CameraController.instance.activeRoom = transform.GetChild(0);
        }
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if(coll.CompareTag("Player"))
        {
            CameraController.instance.activeRoom = transform.GetChild(0);
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if(coll.CompareTag("Player"))
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
