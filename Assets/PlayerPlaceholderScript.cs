using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class PlayerPlaceholderScript : MonoBehaviour
{
 
    public GameObject Player;
    public GameObject Planet;
 
    // Update is called once per frame
    void Update()
    {
        //SMOOTH
 
        //POSITION
        transform.position = Vector3.Lerp(transform.position, Player.transform.position, 0.1f);
 
        Vector3 gravDirection = (transform.position - Planet.transform.position).normalized;
 
        //ROTATION
        Quaternion toRotation = Quaternion.FromToRotation(transform.up, gravDirection) * transform.rotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 0.1f);
 
    }
 
 
    public void NewPlanet(GameObject newPlanet) {
 
        Planet = newPlanet;
    }
 
}
