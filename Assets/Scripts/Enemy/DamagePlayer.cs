using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    [SerializeField] private Transform SpawnPoint;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerCombat>().CanBeDamaged)
            {
                other.transform.GetComponentInParent<PlayerMov>().transform.position = SpawnPoint.transform.position;
                other.GetComponent<PlayerCombat>().damagePlayer();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerCombat>().CanBeDamaged)
            {
                other.transform.GetComponentInParent<PlayerMov>().transform.position = SpawnPoint.transform.position;
                other.GetComponent<PlayerCombat>().damagePlayer();
            }
        }
    }
}
