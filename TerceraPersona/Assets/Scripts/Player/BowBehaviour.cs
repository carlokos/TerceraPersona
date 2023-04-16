using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowBehaviour : MonoBehaviour
{
    [Header("Shooting references")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float shotCD;
    private bool canShoot = true;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private PlayerMov player;

    [Header ("Keybinds")]
    [SerializeField] private KeyCode shotKey = KeyCode.Mouse0;

    [Header("Animations")]
    private GameObject decorativeArrow;

    public bool CanShoot { get => canShoot; set => canShoot = value; }

    private void Start()
    {
        decorativeArrow = GameObject.Find("decorativeArrow");
        decorativeArrow.SetActive(true);
    }

    // Calculates the center of the screen and make a direcction between it and the spawnPoint
    void Update()
    {        
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if(Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            mouseWorldPosition = raycastHit.point; 
        }

        if (Input.GetKey(shotKey) && canShoot && !player.IsDodging)
        {
            StartCoroutine(getArrowAnimation());
            canShoot = false;
            Vector3 aimDir = (mouseWorldPosition - spawnPoint.position).normalized;
            Instantiate(arrowPrefab, spawnPoint.position, Quaternion.LookRotation(aimDir, Vector3.up));
            Invoke(nameof(resetShot), shotCD);
        }
    }

    private void resetShot()
    {
        canShoot = true;
    }

    //Methos use only for animation
    private IEnumerator getArrowAnimation()
    {
        decorativeArrow.SetActive(false);
        yield return new WaitForSeconds(1.26f);
        decorativeArrow.SetActive(true);
    }
}
