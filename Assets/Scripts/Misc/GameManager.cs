using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("HUD user")]
    [SerializeField] private int dummys;
    [SerializeField] private TextMeshProUGUI lblDummys;
    [SerializeField] private GameObject imgDummy;

    [Header("Game design")]
    [SerializeField] private Animator doorAnim;

    public static GameManager instance
    {
        get; private set;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        lblDummys.text = "x " + dummys;
    }

    public void restDummys()
    {
        dummys -= 1;
        lblDummys.text = "x " + dummys;
        if (dummys <= 0)
        {
            doorAnim.SetTrigger("Completed");
            lblDummys.gameObject.SetActive(false);
            imgDummy.SetActive(false);
        }
    }
}
