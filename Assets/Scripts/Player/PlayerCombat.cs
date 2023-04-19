using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Basic References")]
    [SerializeField] private PlayerMov playerMov;
    [SerializeField] private Animator anim;

    private bool canBeDamaged = true;

    public bool CanBeDamaged { get => canBeDamaged; set => canBeDamaged = value; }

    public void damagePlayer()
    {
        if (canBeDamaged)
        {
            anim.SetTrigger("Hit");
        }
    }

    public void disableMovement()
    {
        playerMov.CanMove = false;
        playerMov.CanDodge = false;
    }

    public void enableMovement()
    {
        playerMov.CanDodge = true;
        playerMov.CanMove = true;
    }

    public void StartRoll()
    {
        canBeDamaged = false;
    }

    public void finishRoll()
    {
        canBeDamaged = true;
    }
}
