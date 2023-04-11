using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
    [SerializeField] private float Speed = 10f;
    [SerializeField] private float lifeSpan = 0.7f;
    private Rigidbody arrowRigidbody;

    private void Awake()
    {
        arrowRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        arrowRigidbody.velocity = transform.forward * Speed;
        Quaternion.Euler(0, 0, 90);
        Destroy(gameObject, lifeSpan);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")){
            Debug.Log("Enemigo derrotado");
            Destroy(gameObject);
        }
    }
}
