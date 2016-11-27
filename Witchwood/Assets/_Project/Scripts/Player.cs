using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField]
    string m_CharacterName = "Character Name";

    Rigidbody m_Body = null;
    Inventory m_Inventory = null;
    bool m_IsActive = false;

    int m_MaxHP = 6;
    int m_HP = 1;
    int m_Aspect = 0;

	void Start()
    {
        m_Body = GetComponent<Rigidbody>();
        m_Inventory = new Inventory();

        m_HP = m_MaxHP;
	}
	
	void FixedUpdate()
    {
        m_Body.velocity = Vector3.zero;
        if (!m_IsActive) return;

        if (Board.i.m_AllowRotation) transform.Rotate(transform.up, Input.GetAxis("mouse x"));

        Vector3 movement = Vector3.zero;
        movement += transform.forward * Input.GetAxis("Vertical");
        movement += transform.right * Input.GetAxis("Horizontal");
        if (movement.magnitude > 1) movement.Normalize();
        if (Board.i.m_AllowMovement) m_Body.velocity += movement * 5;
    }

    public bool PickupItem(Item aItem)
    {
        if (m_Inventory.Backpack.Count > 3) return false;

        m_Inventory.Backpack.Add(aItem);
        return true;
    }

    public void ChangeHealth(int aHealthChange)
    {
        m_HP += aHealthChange;
        if (m_HP > m_MaxHP) m_HP = m_MaxHP;
    }

    public bool GetIsActive() { return m_IsActive; }

    public void SetIsActive(bool aActive) { m_IsActive = aActive; }

    public Inventory GetInventory() { return m_Inventory; }

    public string GetName() { return m_CharacterName; }

    public int GetHP() { return m_HP; }

    public int GetMaxHP() { return m_MaxHP; }

    public int GetAspect() { return m_Aspect; }
}

public class Inventory
{
    public Item MomentoSlot = null;
    public Item HeadSlot = null;
    public Item BodySlot = null;
    public Item FeetSlot = null;
    public Item AccessorySlot = null;
    public List<Item> Weapons = new List<Item>();
    public List<Item> Backpack = new List<Item>();

    public Item GetItem(Item.Slot aSlot, int aIndex = 0)
    {
        switch (aSlot)
        {
            case Item.Slot.Momento:
                return MomentoSlot;
            case Item.Slot.Accessory:
                return AccessorySlot;
            case Item.Slot.Head:
                return HeadSlot;
            case Item.Slot.Body:
                return BodySlot;
            case Item.Slot.Feet:
                return FeetSlot;
            case Item.Slot.Weapon:
                return Weapons[aIndex];
            case Item.Slot.Backpack:
                return Backpack[aIndex];
        }
        return null;
    }

    public void SetItem (Item aItem, Item.Slot aSlot)
    {
        switch (aSlot)
        {
            case Item.Slot.Momento:
                MomentoSlot = aItem;
                break;
            case Item.Slot.Accessory:
                AccessorySlot = aItem;
                break;
            case Item.Slot.Head:
                HeadSlot = aItem;
                break;
            case Item.Slot.Body:
                BodySlot = aItem;
                break;
            case Item.Slot.Feet:
                FeetSlot = aItem;
                break;
            case Item.Slot.Weapon:
                if (aItem != null) Weapons.Add(aItem);
                break;
            case Item.Slot.Backpack:
                if (aItem != null) Backpack.Add(aItem);
                break;
        }
    }
}
