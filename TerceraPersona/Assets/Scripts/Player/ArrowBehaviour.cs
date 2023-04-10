using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
    [SerializeField] private float Speed = 10f;
    private Rigidbody arrowRigidbody;

    private void Awake()
    {
        arrowRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        arrowRigidbody.velocity = transform.forward * Speed;
        Quaternion.Euler(0, 0, 90);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
