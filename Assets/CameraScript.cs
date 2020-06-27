using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
 
public class CameraScript : MonoBehaviour
{
		public Joystick camJoystick;
		public Joystick PlayerJoystick;
    public GameObject planet;
    public GameObject player;
    public float camSpeed = 1f;
    float sensitivity = 17f;
    Vector3 onPlayer;
    Quaternion onPlayerRot;
    bool isOnPlayer = true;
 
    float minFov = 35;
    float maxFov = 100;
 
    void Start() {
      savePosition();
    }
 
    // Update is called once per frame
    void Update()
    {
        float right = -camJoystick.Vertical * camSpeed;
        float up = camJoystick.Horizontal * camSpeed;
        if (isOnPlayer && (Mathf.Abs(right) > 0.01 || Mathf.Abs(up) > 0.01))
          savePosition();
        transform.RotateAround(planet.transform.position, transform.up, up);
        transform.RotateAround(planet.transform.position, transform.right, right);
        /*float d = 0.1f;
        float fov = 90;
        fov += Input.GetAxis("not realy needed") * -sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        */
 
    }

    void savePosition() {
            onPlayer = transform.localPosition;
            onPlayerRot = transform.localRotation;
            isOnPlayer = false;
    }

    public void restorePositon() {
      transform.localPosition = onPlayer;
      transform.localRotation = onPlayerRot;
      isOnPlayer = true;
      return;
      Vector3 planetNormal = new Vector3(0, 0, 0);
      RaycastHit hit = new RaycastHit();
      if (Physics.Raycast(player.transform.position, -transform.up, out hit, 10)) {
          planetNormal = hit.normal;
      }
      Vector3 cameraNormal = (player.transform.position - planet.transform.position).normalized;
      Vector3 cameraTargetPosition = player.transform.position + cameraNormal * 14;
      transform.position = cameraTargetPosition;
      transform.LookAt(planet.transform);
      isOnPlayer = true;
      Debug.Log(transform.position);
    }
}
