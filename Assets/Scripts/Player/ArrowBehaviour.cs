using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
    [SerializeField] private float Speed = 10f;
    [SerializeField] private float lifeSpan = 0.7f;
    private Rigidbody arrowRigidbody;
    private CinemachineImpulseSource source;

    private void Awake()
    {
        arrowRigidbody = GetComponent<Rigidbody>();
    }

    //As soon the arrow is spawner is moving and shake a little the camera
    private void Start()
    {
        arrowRigidbody.velocity = transform.forward * Speed;
        source = GetComponent<CinemachineImpulseSource>();
        source.GenerateImpulse(Camera.main.transform.forward);
        Quaternion.Euler(0, 0, 90);
        Destroy(gameObject, lifeSpan);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")){
            other.GetComponent<DummyBehaviour>().activateHitAnimation();
        }
        
        if(!other.CompareTag("UI"))Destroy(gameObject);
    }
}
