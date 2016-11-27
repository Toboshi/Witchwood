using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour
{
    void Start()
    {
        transform.Rotate(transform.up, 45);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player p = other.GetComponent<Player>();
            p.ChangeHealth(-1);
            FindObjectOfType<TrapUI>().Activate();
            Destroy(gameObject);
        }
    }
}
