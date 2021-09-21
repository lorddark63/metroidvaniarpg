using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombs : MonoBehaviour
{
    [SerializeField] private float[] bombThrowDistance; //2.5f
    [SerializeField] private GameObject[] bombObj;
    [SerializeField] private GameObject bombPlace;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && PlayerController.instance.equipment.container.slots[6].itemObject != null)
        {
            BombIntantiate();
        }
    }

    public void BombIntantiate()
    { 
        InventorySlot bombEquiped = PlayerController.instance.equipment.container.slots[6];
        int bombId = bombEquiped.item.bombStats[0].value;

        float distance;
        if(PlayerController.instance.isLookingRight)
            distance = bombThrowDistance[bombId];
        else
            distance = -bombThrowDistance[bombId];

        GameObject bombInst = Instantiate(bombObj[bombId], bombPlace.transform.position, Quaternion.identity) as GameObject;
        bombInst.GetComponent<Rigidbody2D>().AddForce(new Vector2(distance, 2.5f), ForceMode2D.Impulse);

        bombEquiped.amount--;

        if(bombEquiped.amount <= 0)
        {
            bombEquiped.RemoveItem();
        }
    }
}
