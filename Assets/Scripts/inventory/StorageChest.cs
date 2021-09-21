using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageChest : MonoBehaviour
{
    public string chestName;
    public bool generated = false;
    bool itemsAdded;
    public int chestLootType;
    public bool open;

    public int weightTotal;
    public int randomLootTryTimes;
    public DefinedLoot[] definedLoots;
    public RandomLoot[] randomLoots;

    public List<Item> generatedItems = new List<Item>();
    public List<int> generatedAmount = new List<int>();
    public Inventory chestData;
    public DynamicInterface chestUi;
    public GameObject canvas;


    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            UiManager.instance.ChestName(chestName);
            chestData.Load();
            chestUi.GetComponent<UserInterface>().inventory = chestData;
         //   var newArray = new InventorySlot[itemsInChest.Length];
        // chestData.GetSlots = newArray;
            StartCoroutine(WaitOneFrame());
        }
    }

    IEnumerator WaitOneFrame()
    {
        yield return new WaitForSeconds(0.2f);

        open = true;
        if(!itemsAdded)
        {
            UpdateChestData();
            itemsAdded = true;
        }
        
        canvas.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            chestUi.GetComponent<UserInterface>().inventory = null;
            open = false;
            chestUi.slotsOnInterface.Clear();
            chestData.Clear();
            foreach (Transform child in chestUi.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            canvas.SetActive(false);
            itemsAdded = false;
        }
    }
    
    private void UpdateChestData()
    {
        // Debug.Log(chestData.Container.Items.Length);
        if (generated)
        {
            for (int i = 0; i < generatedItems.Count; i++)
            {
                chestData.AddItem(generatedItems[i], generatedAmount[i]); 
            }
            return;
        }

        switch(chestLootType)
        {
            case 0:
                DefinedLoot();
                break;
            
            case 1:
                for (int i = 0; i < randomLootTryTimes; i++)
                    RandomLoot();
                break;

            case 2:
                DefinedLoot();

                for (int i = 0; i < randomLootTryTimes; i++)
                    RandomLoot();
                break;
        }

        generated = true;
    }

    void DefinedLoot()
    {
        for (int i = 0; i < definedLoots.Length; i++)
        {
            Item item = new Item(definedLoots[i].itemsInChest);
            int amount = definedLoots[i].itemsAmount;
            chestData.AddItem(item, amount);
            
            generatedItems.Add(item);
            generatedAmount.Add(amount);
        }
    }

    void RandomLoot()
    {
        float rand = Random.Range(0, weightTotal);

        for (int i = 0; i < randomLoots.Length; i++)
        {
            weightTotal += randomLoots[i].weightTable;
        }
       
        

        for (int i = 0; i < randomLoots.Length; i++)
        {
            if(rand <= randomLoots[i].weightTable)
            {
                Item item = new Item(randomLoots[i].itemsInChest);
                int amount = definedLoots[i].itemsAmount;
                chestData.AddItem(item, amount);
                generatedItems.Add(item);
                generatedAmount.Add(amount);
                return;
            }
            else
            {
                rand -= randomLoots[i].weightTable;
            }
        }
    }


    private void Update()
    {
        if (!chestUi.isActiveAndEnabled || !open)
            return;
        for (int i = 0; i < generatedItems.Count; i++)
        {
            bool found = false;
            for (int j = 0; j < chestData.GetSlots.Length; j++)
            {
                if(generatedItems[i] == chestData.GetSlots[j].item)
                {
                    found = true;
                   
                }
            }
            if(found == false)
            {
                generatedItems.Remove(generatedItems[i]);
            }
        }
       
    }
    private void OnApplicationQuit()
    {
        chestData.Clear();
    }
}

[System.Serializable]
public class DefinedLoot
{
    public ItemObject itemsInChest;
    public int itemsAmount;
}

[System.Serializable]
public class RandomLoot
{
    public ItemObject itemsInChest;
    public int itemsAmount;
    public int weightTable;
}
