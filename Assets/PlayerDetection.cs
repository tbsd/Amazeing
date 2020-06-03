using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
  public GameObject Player;
  public bool isColliding = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnCollisionEnter(Collision collision) {
        if (collision.collider.gameObject == Player) {
            isColliding = true;
        }
    }

    void OnCollisionStay(Collision collision) {
        if (collision.collider.gameObject == Player) {
        }
    }

    void OnCollisionExit(Collision collision) {
        if (collision.collider.gameObject == Player) {
            isColliding = false;
        }
    }

}
