using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
    public static Board i = null;

    public enum GameState
    {
        Movement,
        Menu,
        Battle,
        Other
    }

    [Header("Settings")]
    [SerializeField]
    int m_BoardSize = 10;
    
    [SerializeField]
    [Range(0, 1)]
    float m_TileDensity = 0.5f;

    [SerializeField]
    int m_LootWeight = 10;

    [SerializeField]
    int m_TrapWeight = 10;

    [SerializeField]
    int m_EnemyWeight = 10;

    [SerializeField]
    [Range(1, 4)]
    int m_CharacterCount = 1;
    List<Player> m_Characters = null;

    [Header("Prefabs")]
    [SerializeField]
    GameObject m_PlayerPrefab = null;

    [SerializeField]
    GameObject[] m_TilePrefabs = null;

    [SerializeField]
    GameObject[] m_JesterPrefabs = null;

    [SerializeField]
    GameObject m_LootPrefab = null;

    [SerializeField]
    GameObject m_TrapPrefab = null;

    [SerializeField]
    GameObject[] m_EnemyPrefabs = null;

    [SerializeField]
    GameObject[] m_BossPrefabs = null;

    Tile[,] m_Tiles = null;
    GameState m_State = GameState.Movement;

    void Awake ()
    {
        if (i == null)
            i = this;
        else if (i != this)
            Destroy(gameObject);
    }

	void Start ()
    {
        if (m_BoardSize < 3) m_BoardSize = 3;

        m_Tiles = new Tile[m_BoardSize, m_BoardSize];
        m_Characters = new List<Player>();

        //Spawn players
        for (int i = 0; i < m_CharacterCount; i++)
        {
            GameObject player = Instantiate(m_PlayerPrefab, new Vector3((i - m_CharacterCount * 0.5f) * 2, 0, -53), Quaternion.identity) as GameObject;
            m_Characters.Add(player.GetComponent<Player>());

            Color c = Color.white;
            if (i == 1) c = Color.grey;
            if (i == 2) c = Color.green;
            if (i == 3) c = Color.yellow;
            player.transform.GetChild(0).GetComponent<Renderer>().material.color = c;
        }
        SetActiveCharacter(0);

        //Spawn tiles
        for (int y = 0; y < m_BoardSize; y++)
        {
            for (int x = 0; x < m_BoardSize; x++)
            {
                GameObject obj = Instantiate(m_TilePrefabs[Random.Range(0, m_TilePrefabs.Length)], new Vector3((x - m_BoardSize * 0.5f) * 40, -30, y * 40), Quaternion.identity) as GameObject;
                obj.transform.Rotate(obj.transform.up, Random.Range(0, 3) * 90);
                obj.transform.parent = transform;
                m_Tiles[x, y] = obj.GetComponent<Tile>();
            }
        }

        //Set tile types
        float weightAmount = m_LootWeight + m_TrapWeight + m_EnemyWeight;
        float lootWeight = m_LootWeight / weightAmount;
        float trapWeight = m_TrapWeight / weightAmount;

        List<int> indices = new List<int>();
        int tileCount = Mathf.FloorToInt(m_BoardSize * m_BoardSize * m_TileDensity);
        for (int i = 0; i < tileCount; i++)
        {
            int rng = Random.Range(0, m_BoardSize * m_BoardSize);
            do
            {
                rng = Random.Range(0, m_BoardSize * m_BoardSize);
            }
            while (indices.Contains(rng));

            indices.Add(rng);

            if (i == 0)
            {
                Tile t = GetTile(rng);
                t.SetTileType(Tile.Type.Boss, (Instantiate(m_BossPrefabs[0], t.transform.position, Quaternion.identity) as GameObject).transform);
            }
            else if (i <= 3)
            {
                Tile t = GetTile(rng);
                t.SetTileType(Tile.Type.Jester, (Instantiate(m_JesterPrefabs[0], t.transform.position, Quaternion.identity) as GameObject).transform);
            }
            else if (i <= tileCount * lootWeight)
            {
                Tile t = GetTile(rng);
                t.SetTileType(Tile.Type.Loot, (Instantiate(m_LootPrefab, t.transform.position, Quaternion.identity) as GameObject).transform);
            }
            else if (i <= tileCount * lootWeight + trapWeight)
            {
                Tile t = GetTile(rng);
                t.SetTileType(Tile.Type.Trap, (Instantiate(m_TrapPrefab, t.transform.position, Quaternion.identity) as GameObject).transform);
            }
            else
            {
                Tile t = GetTile(rng);
                t.SetTileType(Tile.Type.Monster, (Instantiate(m_EnemyPrefabs[0], t.transform.position, Quaternion.identity) as GameObject).transform);
            }
        }
	}

    public GameState GetGameState()
    {
        return m_State;
    }

    public void SetGameState(GameState aState)
    {
        m_State = aState;
    }

    public int GetCharacterCount()
    {
        return m_CharacterCount;
    }

    public Player GetActiveCharacter()
    {
        for (int i = 0; i < m_CharacterCount; i++)
        {
            if (m_Characters[i].GetIsActive())
            {
                return m_Characters[i];
            }
        }

        return null;
    }

    public void SetActiveCharacter(int aIndex)
    {
        if (aIndex > m_CharacterCount) return;

        for (int i = 0; i < m_Characters.Count; i++)
        {
            if (i == aIndex)
            {
                Camera.main.transform.parent = m_Characters[i].transform;
                Camera.main.transform.eulerAngles = new Vector3(10, m_Characters[i].transform.eulerAngles.y, 0);
                Camera.main.transform.localPosition = new Vector3(1, 3.5f, -6);
                m_Characters[i].SetIsActive(true);
            }
            else
            {
                m_Characters[i].SetIsActive(false);
            }
        }
    }

    public void SetActiveCharacter(Player aCharacter)
    {
        for (int i = 0; i < m_Characters.Count; i++)
        {
            if (m_Characters[i] == aCharacter)
            {
                Camera.main.transform.parent = m_Characters[i].transform;
                Camera.main.transform.eulerAngles = new Vector3(10, m_Characters[i].transform.eulerAngles.y, 0);
                Camera.main.transform.localPosition = new Vector3(1, 3.5f, -6);
                m_Characters[i].SetIsActive(true);
            }
            else
            {
                m_Characters[i].SetIsActive(false);
            }
        }
    }

    Tile GetTile(int aIndex)
    {
        Vector2 coords = IndexToCoords(aIndex);
        return m_Tiles[(int)coords.x, (int)coords.y];
    }

    Vector2 IndexToCoords(int aIndex)
    {
        Vector2 result = new Vector2(0, 0);
        if (aIndex > m_BoardSize * m_BoardSize) return result;

        result.x = aIndex % m_BoardSize;
        result.y = aIndex / m_BoardSize;

        return result;
    }
}
