using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
    public enum Type
    {
        Blank,
        Loot,
        Trap,
        Monster,
        Jester,
        Boss
    }

    [SerializeField]
    bool m_Active = false;

    [Header("Exits")]
    [SerializeField]
    bool m_North = true;
    [SerializeField]
    bool m_South = true;
    [SerializeField]
    bool m_East = true;
    [SerializeField]
    bool m_West = true;

    Transform m_Mesh = null;
    Type m_TileType = Type.Blank;

    void Awake ()
    {
        m_Mesh = transform.FindChild("Mesh");

        if (m_Active == false)
        {
            m_Mesh.gameObject.SetActive(false);
        }
    }
	
	void OnTriggerEnter (Collider other)
    {
        if (m_Active) return;

        if (other.tag == "Player")
        {
            m_Mesh.gameObject.SetActive(true);
            StartCoroutine(move_cr());
            m_Active = true;
        }
	}

    public void SetTileType(Type aType, Transform aObject = null)
    {
        m_TileType = aType;

        if (aObject != null)
        {
            aObject.parent = m_Mesh;
        }
    }

    IEnumerator move_cr()
    {
        Vector3 start = transform.position;
        Vector3 end = start;
        end.y = 0;

        float duration = 1;
        float t = 0;
        while(t < duration)
        {
            float val = t / duration;
            transform.position = Easing.EaseOutExpo(start, end, val);

            t += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
    }
}
