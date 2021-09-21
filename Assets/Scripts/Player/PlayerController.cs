using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public Inventory inventory;
    public Inventory equipment;
    public Inventory chest;

    [Header("Char atributes")]
    public Attribute[] attributes; 

    public float speed, jumpHeight;
    private float velX, velY;
    public Rigidbody2D rb;
    public Transform groundCheck;
    public bool isGrounded;
    public bool isLookingRight;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    public bool canMOve;
    public Animator anim;

    public Transform groundDetector;
    public bool isOnShopMode;

    [HideInInspector]public Spells spells;
    bool isCollDown = false;

    public GhostEffect ghostEffect;

    void Awake()
    {
        spells = GetComponent<Spells>();
        if(instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        canMOve = true;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ghostEffect = GetComponent<GhostEffect>();

        for (int i = 0; i < attributes.Length; i++)
        {
            attributes[i].SetParent(this);
        }

        for (int i = 0; i < equipment.GetSlots.Length; i++)
        {
            equipment.GetSlots[i].OnBeforeUpdate += OnBeforeUpdate;
            equipment.GetSlots[i].OnAfterUpdate += OnAfterUpdate;
        }
    }

    public void OnBeforeUpdate(InventorySlot _slot)
    {
        if(_slot.itemObject == null)
            return;

        switch(_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("removed ", _slot.itemObject, " on ", _slot.parent.inventory.type, ", allowed items: ", string.Join(", ", _slot.AllowedItem)));
                
                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if(attributes[j].type == _slot.item.buffs[i].attributes)
                            attributes[j].value.RemoveModifier(_slot.item.buffs[i]);
                    }
                }

                //update hud potion in game
                if(_slot.AllowedItem[0] == ItemType.potion)
                {
                    UiManager.instance.WeaponsAndSpellsHud(2, null);
                    UiManager.instance.WeaponsAndSpellsText(2, "");
                }
                    
                break;
        }
    }

    public void OnAfterUpdate(InventorySlot _slot)
    {
        if(_slot.itemObject == null)
            return;

        switch(_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("placed ", _slot.itemObject, " on ", _slot.parent.inventory.type, ", allowed items: ", string.Join(", ", _slot.AllowedItem)));
                
                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if(attributes[j].type == _slot.item.buffs[i].attributes)
                            attributes[j].value.AddModifier(_slot.item.buffs[i]);
                    }
                }
                
                //update hud potion in game
                if(_slot.AllowedItem[0] == ItemType.potion)
                {
                    UiManager.instance.WeaponsAndSpellsHud(2, _slot.itemObject.slotImg);
                    UiManager.instance.WeaponsAndSpellsText(2, _slot.amount.ToString());
                }

                break;   
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Flip();
        MoveInput();
        CheckDirection();
        JumAnim();
        Attack();

        Test();
        UseItemOnEquipment();
        UseSpellOnEquipment();

        DetectGroundByRaycast();
        if(equipment.container.slots[3].itemObject != null)
            spells.CollDown(0, equipment.container.slots[3].item.spellStats[1].value, (int)equipment.container.slots[3].item.spellStats[0].value);
        if(equipment.container.slots[4].itemObject != null)
            spells.CollDown(1, equipment.container.slots[4].item.spellStats[1].value, (int)equipment.container.slots[4].item.spellStats[0].value);
        
    }
    

    void Test()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            inventory.Save();
            equipment.Save();
            chest.Save();
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            inventory.Load();
            equipment.Load();
            chest.Load();
        }
    }

    public void AttributeModified(Attribute attribute)
    {
        Debug.Log(string.Concat(attribute.type, "was update, value now is: ", attribute.value.ModifiedValue));
    }

    //new code
    public void WeaponAttributeModified(SwordAttribute Wattribute)
    {
        Debug.Log(string.Concat(Wattribute.type, "was update, value now is: ", Wattribute.value));
    }

    void FixedUpdate()
    {
        CheckGround();

        if(!canMOve)
            return;
        if(!GetComponent<PlayerDash>().isDashing)
            ApplyMovement();
        Jump();
        
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    public RaycastHit2D DetectGroundByRaycast()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(groundDetector.position, Vector3.down, 10, groundLayer);
        Debug.DrawRay(groundDetector.position, Vector3.down);
        return hitInfo;
    }

    void MoveInput()
    {
        velX = Input.GetAxis("Horizontal");

        if(rb.velocity.x != 0)
        {
            ghostEffect.makeEffect = true;
            anim.SetBool("run", true);
        }
        else
        {
            ghostEffect.makeEffect = false;
            anim.SetBool("run", false);
        }
    }

    void ApplyMovement()
    {
        rb.velocity = new Vector2(speed * velX, rb.velocity.y);
    }

    void CheckDirection()
    {
        if(isLookingRight && velX < 0)
        {
            Flip();
        }
        else if(!isLookingRight && velX > 0)
        {
            Flip();
        }
    }

    public void Attack()
    {
        if(Input.GetKeyDown(KeyCode.Z))
            anim.SetBool("attack", true);
        else
            anim.SetBool("attack", false);
    }

    void Flip()
    {
        isLookingRight = !isLookingRight;
        transform.Rotate(0, 180, 0);
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }
    }

    void JumAnim()
    {
        if(isGrounded)
            anim.SetBool("jump", false);
        else
            anim.SetBool("jump", true);
    }

    public void CanNotMove()
    {
        canMOve = false;
    }

    public void CanMoveAgain()
    {
        canMOve = true;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        var item = coll.GetComponent<GroundItems>();
        if(item)
        {
            if(inventory.AddItem(new Item(item.item), item.amount))
            {
                Destroy(coll.gameObject);
            }
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Clear();
        equipment.Clear();
    }

    public void UseItemOnEquipment()
    {
        InventorySlot potion = equipment.container.slots[5];

        if(potion.itemObject != null)
        {
            if(Input.GetKeyDown(KeyCode.I))
            {
                print(potion.item.name);
                UseItems(potion.item.name);
                potion.amount--;
                UiManager.instance.WeaponsAndSpellsText(2, potion.amount.ToString());

                if( potion.amount <= 0)
                {
                    potion.RemoveItem();
                }

                potion.parent.UpdateSlots();
            }
        }
    }

    public void UseSpellOnEquipment()
    { //0 id, 1 colldown, 2 mana cost
        InventorySlot spell1 = equipment.container.slots[3];
        InventorySlot spell2 = equipment.container.slots[4];

        if(spell1.itemObject != null)
        {
            UiManager.instance.WeaponsAndSpellsHud(3, spell1.itemObject.slotImg);
            if(Input.GetKeyDown(KeyCode.Q) && !spells.isColldown[(int)spell1.item.spellStats[0].value])
            {
                UseSpells(spell1.item.name);
                spells.isColldown[(int)spell1.item.spellStats[0].value] = true;
                spells.colldownImg[0].fillAmount = 1;
                
                spells.currentMana -= spell1.item.spellStats[2].value;
                UiManager.instance.ManaUi(spell1.item.spellStats[2].value, spells.maxMana);
            }
        }
        else
        {
            UiManager.instance.WeaponsAndSpellsHud(3, null);
        }

        if(spell2.itemObject != null)
        {
            UiManager.instance.WeaponsAndSpellsHud(4, spell2.itemObject.slotImg);
            if(Input.GetKeyDown(KeyCode.E) && !spells.isColldown[(int)spell2.item.spellStats[0].value])
            {
                UseSpells(spell2.item.name);
                spells.isColldown[(int)spell2.item.spellStats[0].value] = true;
                spells.colldownImg[1].fillAmount = 1;

                spells.currentMana -= spell2.item.spellStats[2].value;
                UiManager.instance.ManaUi(spell2.item.spellStats[2].value, spells.maxMana);
            }
        }
        else
        {
            UiManager.instance.WeaponsAndSpellsHud(4, null);
        }
    }

    public void UseItems(string itemName)
    {
        switch(itemName)
        {
            case "potion":
                print("usou a poçao");
                break;
            case "potion midle":
                break;
        }
    }

    public void UseSpells(string itemName)
    {
        switch(itemName)
        {
            case "firepunch":
                StartCoroutine(spells.Dash());
                break;
            case "iceattack":
                StartCoroutine(spells.IceAttackMultipleTimes());
                break;
        }
    }
}


[System.Serializable]
public class Attribute
{
    [System.NonSerialized]
    public PlayerController parent;
    public Attributes type;
    public ModifiableInt value;

    public void SetParent(PlayerController _parent)
    {
        parent = _parent;
        value = new ModifiableInt(AttibuteModified);
    }

    public void AttibuteModified()
    {
        parent.AttributeModified(this);
    }
}

