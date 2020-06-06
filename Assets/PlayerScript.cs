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
    public float JumpHeight = 1.2f;
 
    private Shape shape;
	private float rotationSpeed;
	private bool collideingWall;
    private Rigidbody rb;
 
    // Start is called before the first frame update
    void Start()
    {
		//shape = Shape.Box;
        shape = Planet.GetComponent<Maze>().shape;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
		rotationSpeed = speed * 360 / (Mathf.PI * Planet.transform.lossyScale.x / transform.localScale.x);
    }

    // Update is called once per frame
    void Update() {
		movePlayer(getInput());
 
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

	void sphereMove(Vector3 direction) {
		rb.velocity = Vector3.zero;
        transform.RotateAround(Planet.transform.position, transform.forward, direction.x * rotationSpeed * Time.deltaTime);
        transform.RotateAround(Planet.transform.position, Planet.transform.right, direction.z * rotationSpeed * Time.deltaTime);
    }
	
	void boxMove(Vector3 direction) {
		rb.velocity = direction;
		// TODO: check edges
	}
	
	// use this instead of standard collisions because they don't work propertly
	// when objects are not at 90 degreees angle to each other
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
		return result;
		
    }

    Vector3 getInput() {
        float x = 0;
        float z = 0;
        if (playerJoystick.Horizontal >= 0.2f)
          x = speed;
        if (playerJoystick.Vertical >= 0.2f)
          z = -speed;
        if (playerJoystick.Horizontal <= -0.2f)
          x = -speed;
        if (playerJoystick.Vertical <= -0.2f)
          z = speed;
        return new Vector3(x, 0, z);
    }
}
