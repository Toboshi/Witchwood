using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{
    public enum Slot
    {
        Momento,
        Accessory,
        Head,
        Body,
        Feet,
        Weapon,
        Backpack,
        None
    }

    [SerializeField]
    string m_ItemName = "Name";

    [SerializeField]
    Slot m_Slot = Slot.None;

    [SerializeField]
    Sprite m_Icon = null;

    [SerializeField]
    [TextArea(3, 8)]
    string m_Description = "Description";
    
    UseEffect[] m_UseEffects = null;
    Transform m_Mesh = null;

    void Start ()
    {
        m_UseEffects = GetComponents<UseEffect>();
        m_Mesh = transform.Find("Mesh");
	}

    void OnTriggerEnter(Collider other)
    {
        if (m_Mesh.gameObject.activeSelf == false) return;

        if (other.tag == "Player")
        {
            //attach the item to the player
            if (other.GetComponent<Player>().PickupItem(this))
            {
                m_Mesh.gameObject.SetActive(false);
                transform.parent = other.transform;
            }
        }
    }

    public void Drop()
    {
        m_Mesh.gameObject.SetActive(true);
        transform.parent = null;
        transform.position = Board.i.GetActiveCharacter().transform.position + Board.i.GetActiveCharacter().transform.forward * 2;
    }

    public bool Use()
    {
        if (m_UseEffects.Length <= 0) return false;

        foreach (UseEffect effect in m_UseEffects)
        {
            effect.Use();
        }

        if (Board.i.GetActiveCharacter().GetInventory().GetItem(m_Slot) == this)
        {
            Board.i.GetActiveCharacter().GetInventory().SetItem(null, m_Slot);
        }
        else
        {
            Board.i.GetActiveCharacter().GetInventory().Backpack.Remove(this);
        }

        Destroy(gameObject);

        return true;
    }

    public string GetItemDescription ()
    {
        string description = m_ItemName;
        description += " [" + m_Slot.ToString() + "]\n";
        description += m_Description;

        return description;
	}

    public Sprite GetSprite()
    {
        return m_Icon;
    }

    public Slot GetSlot()
    {
        return m_Slot;
    }
}
