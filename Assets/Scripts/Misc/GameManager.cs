using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header ("HUD user")]
    [SerializeField] private int mmoArrows;
    [SerializeField] private GameObject arrowsCanvas;

    public static GameManager instance
    {
        get; private set;
    }

    private void Awake()
    {
        instance = this;
    }

    public void LoseArrow()
    {
        mmoArrows--;
    }

    public void GainArrows(int num)
    {
        mmoArrows += num;
    }
}
