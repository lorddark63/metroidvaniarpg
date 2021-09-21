using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubItems : MonoBehaviour
{
    public static SubItems instance;
    public int subItemsAmount;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        subItemsAmount = PlayerPrefs.GetInt("ammoAmount", 0);
        UiManager.instance.AmmoUiUpdate(subItemsAmount);
    }

    public void SubItemCost(int amount)
    {
        subItemsAmount += amount;
        UiManager.instance.AmmoUiUpdate(subItemsAmount);

        DataManager.instance.AmmoAmountData(subItemsAmount);
        subItemsAmount = PlayerPrefs.GetInt("ammoAmount");
    }
}
