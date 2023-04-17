using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator erika_anim;
    // Start is called before the first frame update
    void Start()
    {
        erika_anim.SetTrigger("Sitting");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
