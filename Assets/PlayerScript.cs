using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MazeNs;
using MazeBuilderNs;
 
public class PlayerScript : MonoBehaviour
{
 
  public GameObject Planet;
  public GameObject Camera;
  public GameObject PlayerPlaceholder;
  public Joystick playerJoystick;
  public float speed = 0.1f;

  private Shape shape;
  private float rotationSpeed;
  private bool collideingWall;
  private Rigidbody rb;

  // Start is called before the first frame update
  void Start() {
    //shape = Shape.Box;
    shape = Planet.GetComponent<Maze>().shape;
    rb = GetComponent<Rigidbody>();
    rb.freezeRotation = true;
    rotationSpeed = speed * 360 / (Mathf.PI * Planet.transform.lossyScale.x / transform.localScale.x);
    if (shape == Shape.Sphere) {
      prevDistance = Vector3.Distance(Planet.transform.position, transform.position);
    }
  }

  // Update is called once per frame
  void Update() {
    movePlayer(getVelocity());
  }

  private void movePlayer(Vector3 direction) {
    direction = wallsEvasion(direction);
    switch(shape) {
      case Shape.Sphere:
        sphereMove(direction);
      break;
      case Shape.Box:
        boxMove(direction);
      break;
      case Shape.Cyllinder:
        cylinderMove(direction);
      break;
      }
    }

  void cylinderMove(Vector3 direction) {
    rb.velocity = new Vector3(direction.x, 0, 0);
    transform.RotateAround(Planet.transform.localPosition, Planet.transform.up, direction.z * rotationSpeed * Time.deltaTime);
  }

  private float prevDistance;
  void sphereMove(Vector3 direction) {
    float rSpeed = 0.2f;
    float up = -direction.z * rSpeed;
    float right = -direction.x * rSpeed;
    transform.RotateAround(Planet.transform.position, transform.forward, right);
    transform.RotateAround(Planet.transform.position, transform.right, up);
  }
  /*
  void sphereMove(Vector3 direction) {
    float rSpeed = 0.2f;
    float up = -direction.z * rSpeed;
    float right = -direction.x * rSpeed;
    Vector3 prevPosition = PlayerPlaceholder.transform.localPosition;
    Quaternion prevRotation = PlayerPlaceholder.transform.localRotation;
    // prevDistance = Vector3.Distance(Planet.transform.position, transform.position);
    PlayerPlaceholder.transform.RotateAround(Planet.transform.position, transform.forward, right);
    PlayerPlaceholder.transform.RotateAround(Planet.transform.position, transform.right, up);
    float newDistance = Vector3.Distance(Planet.transform.position, PlayerPlaceholder.transform.position);
    // transform.RotateAround(Planet.transform.position, transform.forward, right);
    // transform.RotateAround(Planet.transform.position, transform.right, up);
    // float newDistance = Vector3.Distance(Planet.transform.position, transform.position);
    // Debug.Log(prevDistance - newDistance);
    Debug.Log(Mathf.Abs(Mathf.Abs(prevDistance) - Mathf.Abs(newDistance)));
    if (Mathf.Abs(Mathf.Abs(prevDistance) - Mathf.Abs(newDistance)) < 0.4f) {
      // rb.velocity = rb.transform.TransformDirection(prev);
      transform.RotateAround(Planet.transform.position, transform.forward, right);
      transform.RotateAround(Planet.transform.position, transform.right, up);
      // transform.localPosition = prevPosition;
      // transform.localRotation = prevRotation;
    } else {
      PlayerPlaceholder.transform.localPosition = prevPosition;
      PlayerPlaceholder.transform.localRotation = prevRotation;
    }
  }
  */

  float gravity = 80;
  bool OnGround = false;
  float distanceToGround;
  Vector3 Groundnormal;
  void boxMove(Vector3 direction) {
    Vector3 move = getVelocity();
    rb.velocity = rb.transform.TransformDirection(move.x, 0, -move.z);
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
  
  private float rotationCounter = -1;
  private RaycastHit lastHit = new RaycastHit();
  void boxMoveNoGravity(Vector3 direction) {
    Vector3 localVelocity = transform.TransformDirection(new Vector3(direction.x, 0, -direction.z));
    rb.velocity = new Vector3(localVelocity.x, localVelocity.y, localVelocity.z);
    // when player reached an edge
    if (rotationCounter <= 0) {
      RaycastHit hit = new RaycastHit();
      if (!Physics.Raycast(transform.position, -transform.up, out hit, 1) || hit.transform.gameObject != Planet) {
        // get input to find player movement direction
        Vector3 input = getInput();
        Vector3 rotateDirection; 
        Debug.Log(input);
        if (Mathf.Abs(input.x) < Mathf.Abs(input.z))
          rotateDirection = new Vector3(Mathf.Sign(input.x), 0, 0);
        else
          rotateDirection = new Vector3(0, 0, Mathf.Sign(input.z));
        transform.RotateAround(lastHit.point, rotateDirection, 90);
      } else if (hit.transform.gameObject == Planet) {
        lastHit = hit;
      }
      Debug.DrawLine(transform.position, hit.point, Color.red, 10f);
    } else {
      rotationCounter -= Time.deltaTime;
    }
  }
  
  // use this instead of standard collision system because it doesn't work
  // propertly on curved surfaces with small objects
  Vector3 wallsEvasion(Vector3 direction) {
    float distance = 0.3f;
    Vector3 result = direction;
    RaycastHit hit = new RaycastHit();
    if (Physics.Raycast(transform.position, transform.forward, out hit, distance))
      if (direction.z < 0 && hit.collider.tag == "wall")
        result.z = 0;
    if (Physics.Raycast(transform.position, -transform.forward, out hit, distance))
      if (direction.z > 0 && hit.collider.tag == "wall")
        result.z = 0;
    if (Physics.Raycast(transform.position, transform.right, out hit, distance))
      if (direction.x > 0 && hit.collider.tag == "wall")
        result.x = 0;
    if (Physics.Raycast(transform.position, -transform.right, out hit, distance))
      if (direction.x < 0 && hit.collider.tag == "wall")
        result.x = 0;
    if (Physics.Raycast(transform.position, transform.forward + transform.right, out hit, distance))
      if ((direction.z < 0 && direction.x > 0) && hit.collider.tag == "wall") {
        result.z = 0;
        result.x = 0;
      }
    if (Physics.Raycast(transform.position, -transform.forward + transform.right, out hit, distance))
      if ((direction.z > 0 && direction.x > 0) && hit.collider.tag == "wall") {
        result.z = 0;
        result.x = 0;
      }
    if (Physics.Raycast(transform.position, transform.forward - transform.right, out hit, distance))
      if ((direction.z < 0 && direction.x < 0) && hit.collider.tag == "wall") {
        result.z = 0;
        result.x = 0;
      }
    if (Physics.Raycast(transform.position, -transform.forward - transform.right, out hit, distance))
      if ((direction.z > 0 && direction.x < 0) && hit.collider.tag == "wall") {
        result.z = 0;
        result.x = 0;
      }
    return result;
  }

  Vector3 getVelocity() {
    Vector3 input = getInput();
    float x = 0;
    float z = 0;
    if (input.x >= 0.2f)
      x = speed;
    if (input.z >= 0.2f)
      z = speed;
    if (input.x <= -0.2f)
      x = -speed;
    if (input.z <= -0.2f)
      z = -speed;
    return new Vector3(x, 0, z);
  }

  Vector3 getInput() {
    return new Vector3(playerJoystick.Horizontal, 0, -playerJoystick.Vertical);
  }

  void OnCollisionEnter(Collision col) {
    // if (col.collider.tag == "wall")
      // rb.velocity = Vector3.zero;
  }
}
