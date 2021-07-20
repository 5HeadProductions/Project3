using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleScript : MonoBehaviour
{
    float rotationSpeed = 45;
    Vector3 currentEulerAngles;
    float x;
    float y;
    float z;

    void Update()
    {
        if(this.gameObject.transform.position.x > 1100f)Destroy(this.gameObject);
        if(this.gameObject.transform.position.y > 645f)Destroy(this.gameObject);
        
        z = 1 - z;
        this.gameObject.transform.position += new Vector3(.1f,.1f,0f);

        //modifying the Vector3, based on input multiplied by speed and time
        currentEulerAngles += new Vector3(x, y, z) * Time.deltaTime * rotationSpeed;

        //apply the change to the gameObject
        transform.eulerAngles = currentEulerAngles;
    }
}
