using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyBehaviour : MonoBehaviour
{
    [SerializeField] private float life;
    private Animator anim;
    private Collider hitbox;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        hitbox = GetComponent<Collider>();
    }

    //Methos that manage the dummy's health
    public void activateHitAnimation()
    {
        life -= 1;
        anim.SetTrigger("hit");
        if(life <= 0)
        {
            anim.SetTrigger("dead");
            hitbox.enabled = false;
        }
    }
}
