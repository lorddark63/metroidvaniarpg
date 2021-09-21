using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public bool generated = false;
    public bool open;
    public ItemObject[] shopItems;
    public int[] itemsAmount;
    public List<Item> generatedItems = new List<Item>();
    public Inventory shopData;
    public DynamicInterface shopUi;
    public GameObject canvas;

    bool itemsAdded;



    // Start is called before the first frame update
    void Start()
    {
        shopUi.UpdateSlots();
    }

    // Update is called once per frame
    void Update()
    {
         UpdateShopSlots();
         shopUi.UpdateSlots();
    }

    IEnumerator WaitOneFrame()
    {
        yield return new WaitForSeconds(0.2f);

        open = true;
        if(!itemsAdded)
        {
            GenerateShopItems();
            itemsAdded = true;
        }

        canvas.SetActive(true);
        shopUi.UpdateSlots();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerController.instance.isOnShopMode = true;
            shopUi.GetComponent<UserInterface>().inventory = shopData;
            var newArray = new InventorySlot[shopItems.Length];
        // chestData.GetSlots = newArray;
            StartCoroutine(WaitOneFrame());
        }
    }

     private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerController.instance.isOnShopMode = false;
            shopUi.GetComponent<UserInterface>().inventory = null;
            open = false;
            shopUi.slotsOnInterface.Clear();
            shopData.Clear();
            foreach (Transform child in shopUi.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            canvas.SetActive(false);
            itemsAdded = false;
        }
    }

    void GenerateShopItems()
    {
        if (generated)
        {
            for (int i = 0; i < generatedItems.Count; i++)
            {
                shopData.AddItem(generatedItems[i], itemsAmount[i]);
            }
            return;
        }
        for (int i = 0; i < shopItems.Length; i++)
        {
            Item item = new Item(shopItems[i]);
            int amount = itemsAmount[i];
            shopData.AddItem(item, amount);
            
            generatedItems.Add(item);
            
        }
        generated = true;  
    }

    void UpdateShopSlots()
    {
        shopUi.UpdateSlots();
        if (!shopUi.isActiveAndEnabled || !open)
            return;
         for (int i = 0; i < generatedItems.Count; i++)
        {
            bool found = false;
            for (int j = 0; j < shopData.GetSlots.Length; j++)
            {
                if(generatedItems[i] == shopData.GetSlots[j].item)
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
        shopData.Clear();
    }
}
