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
    [SerializeField] private TextMeshProUGUI lblTime;
    [SerializeField] private bool hasTime;

    [Header("Game design")]
    [SerializeField] private Animator doorAnim;
    private float actualTime = 0f;

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

    private void Update()
    {
        if (hasTime)
        {
            actualTime += Time.deltaTime;
            int min = Mathf.FloorToInt(actualTime / 60f);
            int seg = Mathf.FloorToInt(actualTime % 60f);
            lblTime.text = min.ToString("00") + ":" + seg.ToString("00");
        }
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
