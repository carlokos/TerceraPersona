using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerObj;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject crosshair;

    [SerializeField] private float rotationSpeed;

    [SerializeField] private GameObject[] cameras;

    [SerializeField] Transform combatLookAt;

    public CameraStyle currentStyle;

    public enum CameraStyle
    {
        Basic,
        Combat
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        //rotate player Object
        if (currentStyle == CameraStyle.Basic) {
            crosshair.SetActive(false);
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
            {
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
            }
        }

        //Activate the crosshair and change the direcction the camera looks while aiming
        else if(currentStyle == CameraStyle.Combat)
        {
            crosshair.SetActive(true);
            Vector3 dirCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = dirCombatLookAt.normalized;

            playerObj.forward = dirCombatLookAt.normalized;
        }
    }

    //Methos use for switching cameras
    public void SwitchCamera(CameraStyle newStyle)
    {
        for(int i = 0; i < cameras.Length; i++)
        {
            cameras[i].SetActive(false);
        }

        if (newStyle == CameraStyle.Basic) cameras[0].SetActive(true);
        if (newStyle == CameraStyle.Combat) cameras[1].SetActive(true);

        currentStyle =  newStyle;
        
    }
}

