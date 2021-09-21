using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public abstract class UserInterface : MonoBehaviour
{
    public Inventory inventory;
    public ToolTipHandler toolTip;

    public Dictionary<GameObject, InventorySlot> slotsOnInterface = new Dictionary<GameObject, InventorySlot>();

    public bool invenotryIsActive;

    private Inventory previousInventory;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnEnable()
    {
        CreateSlots();
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            
            inventory.GetSlots[i].parent = this;
            //if(inventory.type != InterfaceType.chest && inventory.type != InterfaceType.shop)
            //    inventory.GetSlots[i].OnAfterUpdate += OnSlotUpdate;
        }

        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate {OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate {OnExitInterface(gameObject); });
    }
    

    public abstract void CreateSlots();

    private void OnSlotUpdate(InventorySlot _slot)
    {       
        
        if(_slot.item.ID >= 0)
        {
            _slot.slot.transform.GetChild(0).GetComponentInChildren<Image>().sprite = _slot.itemObject.slotImg;
            _slot.slot.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            _slot.slot.GetComponentInChildren<TextMeshProUGUI>().text = _slot.amount == 1 ? "" : _slot.amount.ToString("n0");
        
            if(PlayerController.instance.isOnShopMode)
            {
                int sellPrice = _slot.item.prices.SellPriceCalc();
                 _slot.slot.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = sellPrice.ToString();
            }
        }
        else
        {
            _slot.slot.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
            _slot.slot.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
            _slot.slot.GetComponentInChildren<TextMeshProUGUI>().text = "";
            _slot.slot.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = "";
        }           

    }


    // Update is called once per frame
    void Update()
    {
        UpdateSlots();
         if(Input.GetKeyDown(KeyCode.I))
        {
            invenotryIsActive = !invenotryIsActive;
            UiManager.instance.inventoryUi.SetActive(invenotryIsActive);    
        }
    }

    public void UpdateSlots()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in slotsOnInterface)
        {
            if(_slot.Value.item.ID >= 0)
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = _slot.Value.itemObject.slotImg;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text =  !_slot.Value.itemObject.stackable ? "" : _slot.Value.amount.ToString("n0");
            
                if(inventory.type == InterfaceType.shop)
                {
                   int total = _slot.Value.item.prices.TotalValue(_slot.Value.amount);
                    _slot.Key.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = total.ToString();
                }  

                if(_slot.Value.parent.inventory.type == InterfaceType.Inventory)
                {
                    if(PlayerController.instance.isOnShopMode)
                    {
                        int sellPrice = _slot.Value.item.prices.SellPriceCalc();
                        _slot.Key.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = sellPrice.ToString();
                    }
                    else
                    {
                        _slot.Key.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = "";
                    }
                }
                
            }
            else
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
                _slot.Key.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj)
    {
        MouseData.slotHoveredOver = obj;   
        if(MouseData.interfaceMouseOver == null)
            return;
        InventorySlot checkItem = MouseData.interfaceMouseOver.slotsOnInterface[MouseData.slotHoveredOver];
        if(checkItem.itemObject != null)
            toolTip.DisplayInfo(MouseData.interfaceMouseOver.slotsOnInterface[MouseData.slotHoveredOver].item);
    }

    public void OnEnterInterface(GameObject obj)
    {
        MouseData.interfaceMouseOver = obj.GetComponent<UserInterface>();
    }

    public void OnExitInterface(GameObject obj)
    {
        MouseData.interfaceMouseOver = null;
    }

    public void OnExit(GameObject obj)
    {
       MouseData.slotHoveredOver = null;
       toolTip.HideInfo();
    }

    public void OnDragStart(GameObject obj)
    { 
        MouseData.tempItemBeingDragged = CreatTempItem(obj);  
        MouseData.interfaceType =   slotsOnInterface[obj].parent.inventory.type;
    }

    public GameObject CreatTempItem(GameObject obj)
    {
        GameObject tempItem = null;
        if(slotsOnInterface[obj].item.ID >= 0)
        {
            tempItem = new GameObject();
            Transform canvas = GameObject.FindGameObjectWithTag("canvas").GetComponent<Transform>();
            //var mouseObject = new GameObject();
            var rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(50, 50);
            tempItem.transform.SetParent(canvas.parent);
            
            var img = tempItem.AddComponent<Image>();
            img.sprite = slotsOnInterface[obj].itemObject.slotImg;
            img.raycastTarget = false;
        }
        return tempItem;    
    }

    public void OnDragEnd(GameObject obj)
    {
        
        Destroy(MouseData.tempItemBeingDragged);
        if(MouseData.interfaceMouseOver == null)
        {
            slotsOnInterface[obj].RemoveItem();
            return;
        }

        //combine 2 equal stackable itens
        if(MouseData.slotHoveredOver)
        {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseOver.slotsOnInterface[MouseData.slotHoveredOver];
            
            if(slotsOnInterface[obj].item.ID == mouseHoverSlotData.item.ID)
            {
                mouseHoverSlotData.amount +=  slotsOnInterface[obj].amount;
                slotsOnInterface[obj].RemoveItem();
            }
            else
            {
                if(MouseData.interfaceType == InterfaceType.shop || mouseHoverSlotData.parent.inventory.type == InterfaceType.shop)
                {
                    if(mouseHoverSlotData.item.ID < 0)
                    {
                        inventory.SwapItem(slotsOnInterface[obj], mouseHoverSlotData);

                        if(mouseHoverSlotData.parent.inventory.type == InterfaceType.Inventory &&
                            MouseData.interfaceType == InterfaceType.shop)
                        {
                            print("item comprado");
                        }  

                        if(mouseHoverSlotData.parent.inventory.type == InterfaceType.shop &&
                            MouseData.interfaceType == InterfaceType.Inventory)
                        {
                            print("item vendido");
                        }
                    }
                }
                else
                {
                    inventory.SwapItem(slotsOnInterface[obj], mouseHoverSlotData);
                }
            }  
        }       
    }


    public void OnDrag(GameObject obj)
    {
        if(MouseData.tempItemBeingDragged != null)
        {
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

    public void OnClick(GameObject obj)
    {
        InventorySlot clickedSlot = MouseData.interfaceMouseOver.slotsOnInterface[MouseData.slotHoveredOver];
        PlayerController player = FindObjectOfType<PlayerController>();

        if(clickedSlot.slot.GetComponent<MouseClickHandler>().pointerUp != -2)
        {
            return;
        }
        
        if(player != null && clickedSlot.itemObject != null)
        {
            if(clickedSlot.itemObject.type == ItemType.potion)
            {
                player.UseItems(clickedSlot.item.name);
                clickedSlot.amount--;
                //clickedSlot.parent.UpdateSlots();

                if(clickedSlot.amount <= 0)
                {
                    slotsOnInterface[obj].RemoveItem();
                }
            }
        } 

    }

}

public static class MouseData
{
    public static InterfaceType interfaceType;
    public static UserInterface interfaceMouseOver;
    public static GameObject tempItemBeingDragged;
    public static GameObject slotHoveredOver;
}
