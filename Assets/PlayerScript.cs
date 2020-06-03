using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class PlayerScript : MonoBehaviour
{
 
    public GameObject Planet;
    public GameObject Camera;
    public GameObject PlayerPlaceholder;
    public Joystick playerJoystick;
 
 
    public float speed = 0.1f;
    public float JumpHeight = 1.2f;
 
    float gravity = 500;
    bool OnGround = false;
 
 
    float distanceToGround;
    Vector3 Groundnormal;
 
 
 
    private Rigidbody rb;
 
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }
 
    void FixedUpdate() {

    }
    // Update is called once per frame
    void Update() {
		// Gravity
      	getInput();
        RaycastHit hit = new RaycastHit();
        bool hasHit = false;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 10)) {
            hasHit = true;
            distanceToGround = hit.distance;
            if (distanceToGround <= 0.2 + transform.localScale.y / 2f) 
                OnGround = true;
            else 
                OnGround = false;
          if (hit.transform == Planet.transform)
            Groundnormal = hit.normal;
          else {
            Groundnormal = (transform.position - Planet.transform.position).normalized;
            OnGround = false;
          }
        Debug.DrawRay(transform.position, (hit.transform.position - transform.position), Color.red, 5f);
        } else {
          OnGround = false;
          bool isSideHit = false;
          if (Physics.Raycast(transform.position, -transform.forward, out hit, 10))
            isSideHit = true;
          else if (isSideHit = Physics.Raycast(transform.position, transform.forward, out hit, 10))
            isSideHit = true;
          else if (Physics.Raycast(transform.position, transform.right, out hit, 10))
            isSideHit = true;
          else if (Physics.Raycast(transform.position, -transform.right, out hit, 10))
            isSideHit = true;
          if (isSideHit) {
              distanceToGround = hit.distance;
            if (hit.transform == Planet.transform)
              Groundnormal = hit.normal;
            else {
              Groundnormal = (transform.position - Planet.transform.position).normalized;
              OnGround = false;
            }
          Debug.DrawRay(transform.position, (hit.transform.position - transform.position), Color.red, 5f);
          }
        }
        Vector3 gravDirection;
        if (hasHit)
          gravDirection = (transform.position - hit.point).normalized;
        else
          gravDirection = (transform.position - Planet.transform.position).normalized;
 
        if (!OnGround) {
            rb.AddForce(gravDirection * -gravity);
        }
 
        float angle = Quaternion.Angle(Quaternion.identity, Quaternion.FromToRotation(transform.up, Groundnormal));
        if (true) {
          Quaternion toRotation = Quaternion.FromToRotation(transform.up, Groundnormal) * transform.rotation;
          transform.rotation = toRotation;
        }
 
 
    }

    private void getInput() {
        float x = 0;
        float z = 0;
        if (playerJoystick.Horizontal >= 0.2f)
          x = speed;
        if (playerJoystick.Vertical >= 0.2f)
          z = speed;
        if (playerJoystick.Horizontal <= -0.2f)
          x = -speed;
        if (playerJoystick.Vertical <= -0.2f)
          z = -speed;
        Vector3 move = rb.transform.TransformDirection(new Vector3(x, 0, z));
        rb.velocity = move;
    }
}
