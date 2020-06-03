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
        // if (Input.GetMouseButton(0)) {
          // restorePositon();
        // }
 
					// float playerRight = -PlayerJoystick.Vertical * camSpeed;
					// float playerUp = PlayerJoystick.Horizontal * camSpeed;
					float right = -camJoystick.Vertical * camSpeed;
					float up = camJoystick.Horizontal * camSpeed;
					transform.RotateAround(planet.transform.position, transform.up, up);
					transform.RotateAround(planet.transform.position, transform.right, right);
          float d = 0.1f;
          //if player joystick triggerd and camera joystick is not in use
          /*
          if ((playerRight > d || playerRight < -d || playerUp > d || playerUp < -d) 
              && (right < d || right > -d || up < d || up > -d)) {
            if (!isOnPlayer) {
              restorePositon();
              isOnPlayer = true;
              }
            }
          if (right > d || right < -d || up > d || up < -d) 
            isOnPlayer = false;
            */
        // if (Input.GetMouseButton(1)) {
          // float up = Input.GetAxis("Mouse X") * camSpeed;
          // float right = -Input.GetAxis("Mouse Y") * camSpeed;
            // if (isOnPlayer)
              // savePosition(right, up);
						// transform.RotateAround(planet.transform.position, transform.up, up);
						// transform.RotateAround(planet.transform.position, transform.right, right);
        // }
 
        //ZOOM
 
        // float fov = Camera.main.fieldOfView;
        float fov = 90;
        fov += Input.GetAxis("Mouse ScrollWheel") * -sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        // Camera.main.fieldOfView = fov;
 
    }

    void savePosition() {
            // onPlayer.x += right;
            // onPlayer.y += up;
            onPlayer = transform.position;
            onPlayerRot = transform.rotation;
            Debug.Log(onPlayer);
            isOnPlayer = false;
    }

    void restorePositon() {
        // RaycastHit hit = new RaycastHit();
        // Vector3 groundnormal = new Vector3();
        // if (Physics.Raycast(transform.position, -transform.up, out hit, 10)) {
            // groundnormal = hit.normal;
        // }
        // Vector3 gravDirection = (transform.position - Planet.transform.position).normalized;
        // Quaternion toRotation = Quaternion.FromToRotation(transform.up, groundnormal) * transform.rotation;
        // transform.rotation = toRotation;
      // transform.position = planet.transform.position + new Vector3(0, 1, 0) * target.transform.position.magnitude;
      // Vector3 planetNormal = (player.transform.position - planet.transform.position).normalized;
      Vector3 planetNormal = new Vector3(0, 0, 0);
      RaycastHit hit = new RaycastHit();
      if (Physics.Raycast(player.transform.position, -transform.up, out hit, 10)) {
          planetNormal = hit.normal;
      }
      Vector3 cameraNormal = (player.transform.position - planet.transform.position).normalized;
      Vector3 cameraTargetPosition = player.transform.position + cameraNormal * 14;
      // Vector3 increaseValues = (cameraTargetPosition - transform.position) / 2;
      // increaseValues.y = 8;
      // transform.position += increaseValues * Time.deltaTime;
      transform.position = cameraTargetPosition;
      transform.LookAt(planet.transform);
      // transform.RotateAround(planet.transform.position, Vector3.down, onPlayer.y);
      // transform.RotateAround(planet.transform.position, Vector3.left, onPlayer.x);
      // transform.RotateAround(planet.transform.position, Vector3.forward, onPlayer.z);
      isOnPlayer = true;
      // onPlayer = new Vector3(0, 0, 0);
            Debug.Log(transform.position);
    }
}
