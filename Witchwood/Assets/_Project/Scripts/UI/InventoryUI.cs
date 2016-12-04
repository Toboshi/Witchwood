using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    RectTransform m_Panel = null;
    float m_Width = 0;

    [SerializeField]
    Image[] m_PlayerIcons = null;

    [SerializeField]
    Image[] m_MomentoIcons = null;

    [SerializeField]
    Image m_HeadIcon = null;

    [SerializeField]
    Image m_BodyIcon = null;

    [SerializeField]
    Image m_FeetIcon = null;

    [SerializeField]
    Image m_AccessoryIcon = null;

    [SerializeField]
    Image[] m_WeaponIcons = null;

    [SerializeField]
    Image[] m_BackpackIcons = null;

    [SerializeField]
    Text m_DescriptionText = null;

    Inventory m_ActiveInventory = null;
    Item m_SelectedItem = null;
    Item.Slot m_SelectedSlot = Item.Slot.None;
    bool m_IsActive = false;

    void Start ()
    {
        m_Width = m_Panel.sizeDelta.x;

        if (Board.i.GetCharacterCount() == 2)
        {
            m_PlayerIcons[1].gameObject.SetActive(true);
            m_MomentoIcons[1].gameObject.SetActive(true);

            m_PlayerIcons[0].GetComponent<RectTransform>().anchoredPosition += Vector2.left * 32;
            m_PlayerIcons[1].GetComponent<RectTransform>().anchoredPosition += Vector2.right * 32;
            m_MomentoIcons[0].GetComponent<RectTransform>().anchoredPosition += Vector2.left * 32;
            m_MomentoIcons[1].GetComponent<RectTransform>().anchoredPosition += Vector2.right * 32;
        }
        else if (Board.i.GetCharacterCount() == 3)
        {
            m_PlayerIcons[1].gameObject.SetActive(true);
            m_PlayerIcons[2].gameObject.SetActive(true);
            m_MomentoIcons[1].gameObject.SetActive(true);
            m_MomentoIcons[2].gameObject.SetActive(true);

            m_PlayerIcons[1].GetComponent<RectTransform>().anchoredPosition += Vector2.left * 70;
            m_PlayerIcons[2].GetComponent<RectTransform>().anchoredPosition += Vector2.right * 70;
            m_MomentoIcons[1].GetComponent<RectTransform>().anchoredPosition += Vector2.left * 70;
            m_MomentoIcons[2].GetComponent<RectTransform>().anchoredPosition += Vector2.right * 70;
        }
        else if (Board.i.GetCharacterCount() == 4)
        {
            m_PlayerIcons[1].gameObject.SetActive(true);
            m_PlayerIcons[2].gameObject.SetActive(true);
            m_PlayerIcons[3].gameObject.SetActive(true);
            m_MomentoIcons[1].gameObject.SetActive(true);
            m_MomentoIcons[2].gameObject.SetActive(true);
            m_MomentoIcons[3].gameObject.SetActive(true);

            m_PlayerIcons[0].GetComponent<RectTransform>().anchoredPosition += Vector2.left * 100;
            m_PlayerIcons[1].GetComponent<RectTransform>().anchoredPosition += Vector2.left * 32;
            m_PlayerIcons[2].GetComponent<RectTransform>().anchoredPosition += Vector2.right * 32;
            m_PlayerIcons[3].GetComponent<RectTransform>().anchoredPosition += Vector2.right * 100;
            m_MomentoIcons[0].GetComponent<RectTransform>().anchoredPosition += Vector2.left * 100;
            m_MomentoIcons[1].GetComponent<RectTransform>().anchoredPosition += Vector2.left * 32;
            m_MomentoIcons[2].GetComponent<RectTransform>().anchoredPosition += Vector2.right * 32;
            m_MomentoIcons[3].GetComponent<RectTransform>().anchoredPosition += Vector2.right * 100;
        }
        
        m_Panel.gameObject.SetActive(false);
	}
	
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.Tab))
        {
            SetActive(!m_IsActive);
            if (Board.i.GetGameState() != Board.GameState.Battle)
                Board.i.SetGameState(m_IsActive ? Board.GameState.Menu : Board.GameState.Movement);
        }
        if (Input.GetKeyDown(KeyCode.Space) && m_SelectedItem != null)
        {
            MoveItem(Item.Slot.None);
        }
        if (Input.GetKeyDown(KeyCode.E) && m_SelectedItem != null)
        {
            if (m_SelectedItem.Use()) UpdateInventory();
        }
    }

    void SetActive (bool aActive)
    {
        if (m_IsActive == aActive) return;

        m_IsActive = aActive;
        m_Panel.gameObject.SetActive(m_IsActive);

        if (m_IsActive) UpdateInventory();
    }

    void UpdateInventory()
    {
        m_SelectedItem = null;
        m_SelectedSlot = Item.Slot.None;
        m_DescriptionText.text = "";

        Player p = Board.i.GetActiveCharacter();
        m_ActiveInventory = p.GetInventory();

        m_HeadIcon.sprite = (m_ActiveInventory.HeadSlot == null ? null : m_ActiveInventory.HeadSlot.GetSprite());
        m_BodyIcon.sprite = (m_ActiveInventory.BodySlot == null ? null : m_ActiveInventory.BodySlot.GetSprite());
        m_FeetIcon.sprite = (m_ActiveInventory.FeetSlot == null ? null : m_ActiveInventory.FeetSlot.GetSprite());
        m_AccessoryIcon.sprite = (m_ActiveInventory.AccessorySlot == null ? null : m_ActiveInventory.AccessorySlot.GetSprite());

        for (int i = 0; i < m_WeaponIcons.Length; i++)
        {
            m_WeaponIcons[i].sprite = (m_ActiveInventory.Weapons.Count > i ? m_ActiveInventory.Weapons[i].GetSprite() : null);
        }

        for (int i = 0; i < m_BackpackIcons.Length; i++)
        {
            m_BackpackIcons[i].sprite = (m_ActiveInventory.Backpack.Count > i ? m_ActiveInventory.Backpack[i].GetSprite() : null);
        }
    }

    public void SetActiveCharacter(int aIndex)
    {
        if (Board.i.GetGameState() == Board.GameState.Battle) return;

        Board.i.SetActiveCharacter(aIndex);
        UpdateInventory();
    }
    
    void Select(Item aItem, Item.Slot aSlot)
    {
        if (m_SelectedItem != null)
        {
            m_DescriptionText.text = "";
            m_SelectedItem = null;
            m_SelectedSlot = Item.Slot.None;
            return;
        }

        m_SelectedItem = aItem;
        m_SelectedSlot = aSlot;
        m_DescriptionText.text = aItem.GetItemDescription();
    }

    void MoveItem(Item.Slot aSlot)
    {
        if (m_SelectedItem.GetSlot() != aSlot && aSlot != Item.Slot.Backpack && aSlot != Item.Slot.None) return;
        if (m_SelectedSlot == aSlot) return;

        //Put the item in its new slot
        if (aSlot == Item.Slot.None)
            m_SelectedItem.Drop();
        else
            m_ActiveInventory.SetItem(m_SelectedItem, aSlot);

        //Remove the item from its old slot
        if (m_SelectedSlot == Item.Slot.Weapon)
            m_ActiveInventory.Weapons.Remove(m_SelectedItem);
        else if (m_SelectedSlot == Item.Slot.Backpack)
            m_ActiveInventory.Backpack.Remove(m_SelectedItem);
        else
            m_ActiveInventory.SetItem(null, m_SelectedSlot);

        UpdateInventory();
    }

    public void SelectItem(int aIndex)
    {
        switch (aIndex)
        {
            case (-1): //Character 1 momento
                break;

            case (-2): //Character 2 momento
                break;

            case (-3): //Character 3 momento
                break;

            case (-4): //Character 4 momento
                break;

            case (0): //Head
                if (m_ActiveInventory.HeadSlot == null)
                {
                    if (m_SelectedItem != null) MoveItem(Item.Slot.Head);
                    break;
                }
                Select(m_ActiveInventory.HeadSlot, Item.Slot.Head);
                break;

            case (1): //Body
                if (m_ActiveInventory.BodySlot == null)
                {
                    if (m_SelectedItem != null) MoveItem(Item.Slot.Body);
                    break;
                }
                Select(m_ActiveInventory.BodySlot, Item.Slot.Body);
                break;

            case (2): //Feet
                if (m_ActiveInventory.FeetSlot == null)
                {
                    if (m_SelectedItem != null) MoveItem(Item.Slot.Feet);
                    break;
                }
                Select(m_ActiveInventory.FeetSlot, Item.Slot.Feet);
                break;

            case (3): //Accessory
                if (m_ActiveInventory.AccessorySlot == null)
                {
                    if (m_SelectedItem != null) MoveItem(Item.Slot.Accessory);
                    break;
                }
                Select(m_ActiveInventory.AccessorySlot, Item.Slot.Accessory);
                break;

            case (4):
            case (5):
            case (6): //Weapon
                if (m_ActiveInventory.Weapons.Count < aIndex - 3)
                {
                    if (m_SelectedItem != null) MoveItem(Item.Slot.Weapon);
                    break;
                }
                Select(m_ActiveInventory.Weapons[aIndex - 4], Item.Slot.Weapon);
                break;

            case (7):
            case (8):
            case (9): //Backpack
                if (m_ActiveInventory.Backpack.Count < aIndex - 6)
                {
                    if (m_SelectedItem != null) MoveItem(Item.Slot.Backpack);
                    break;
                }
                Select(m_ActiveInventory.Backpack[aIndex - 7], Item.Slot.Backpack);
                break;

            default:
                Debug.Log("Invalid Selection");
                break;
        }
    }
}
