using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankAccount : MonoBehaviour
{
    public static BankAccount instance;
    public int bank;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        bank = PlayerPrefs.GetInt("gold", bank);
        UiManager.instance.GoldUiUpdate(bank);
    }

    public void Money(int cashAmount)
    {
        bank += cashAmount;
        DataManager.instance.GoldData(bank);
        UiManager.instance.GoldUiUpdate(bank);

        bank = PlayerPrefs.GetInt("gold");
    }
}
