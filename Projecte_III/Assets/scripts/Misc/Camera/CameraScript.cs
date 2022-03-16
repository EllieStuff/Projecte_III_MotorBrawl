using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    public int camIndex;
    public GameObject playerVehicle;
    [SerializeField] Vector3 posOffset;
    [SerializeField] Vector3 rotOffset;

    public QuadControls controls;

    float camPosSpeed = 5.0f;
    [SerializeField] float camRotSpeed = 1.1f;
    float savedRotSpeed;
    Quaternion rotOffsetQuat, lookBackRotOffset;
    private Camera cam;
    private Rigidbody vehicleRB;

    private void Start()
    {
        Debug.Log("Start1: " + transform.position);
        cam = transform.GetChild(0).GetComponent<Camera>();
        controls = new QuadControls();
        controls.Enable();

        rotOffsetQuat = Quaternion.Euler(rotOffset);
        lookBackRotOffset = Quaternion.Euler(0, 180, 0);
        savedRotSpeed = camRotSpeed;

        if(camIndex == 0)
            playerVehicle = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetChild(0).gameObject;
        else if(camIndex == 1)
            playerVehicle = GameObject.FindGameObjectWithTag("Player2").transform.GetChild(0).GetChild(0).gameObject;

        if (playerVehicle != null)
            vehicleRB = playerVehicle.transform.parent.parent.GetComponent<PlayerVehicleScript>().vehicleRB;

        this.transform.position = new Vector3(playerVehicle.transform.position.x, playerVehicle.transform.position.y + 2, playerVehicle.transform.position.z);
        this.transform.rotation = rotOffsetQuat;
        Debug.Log("Start2: " + transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerVehicleScript pScript = playerVehicle.transform.parent.parent.GetComponent<PlayerVehicleScript>();

        float savedFov = new Vector3(vehicleRB.velocity.x, 0, vehicleRB.velocity.z).magnitude * 75 / pScript.vehicleMaxSpeed;
        
        if (pScript.vehicleMaxSpeed > pScript.savedMaxSpeed && savedFov >= 60)
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, savedFov, Time.deltaTime * 0.8f);
        else
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 60, Time.deltaTime * 5);

        Vector3 targetPos = new Vector3(playerVehicle.transform.position.x, playerVehicle.transform.position.y + 2, playerVehicle.transform.position.z);
        transform.position = Vector3.Lerp(this.transform.position + posOffset, targetPos, Time.deltaTime * camPosSpeed);
        //transform.rotation = Quaternion.Euler(rotOffset);
        Debug.Log("Update: " + transform.position);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, rotOffsetQuat, Time.deltaTime * camRotSpeed * 100);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotOffsetQuat, Time.deltaTime * camRotSpeed);
        
        //transform.rotation = rotOffsetQuat;
      
    }


    public void ChangeRotation(Vector3 _newRot)
    {
        rotOffsetQuat = Quaternion.Euler(_newRot);
    }
    public void ChangeRotation(Vector3 _newRot, float _customRotSpeed)
    {
        rotOffsetQuat = Quaternion.Euler(_newRot);
        camRotSpeed = _customRotSpeed;
    }

    public void ResetRotation()
    {
        rotOffsetQuat = Quaternion.Euler(rotOffset);
        camRotSpeed = savedRotSpeed;
    }
    public void ResetRotation(float _customRotSpeed)
    {
        rotOffsetQuat = Quaternion.Euler(rotOffset);
        camRotSpeed = _customRotSpeed;
    }

}
