using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AsteroidScript : MonoBehaviour
{
    float rotationSpeed = 45;
    Vector3 currentEulerAngles;
    float x;
    float y;
    float z;

    private AudioManager _audio;
    public void OnEnable()
    {
        Invoke("Disable", 15);
    }
    public void Disable()
    {
        gameObject.SetActive(false);
    }
    public void OnDisable()
    {
        CancelInvoke();
    }
    
    void Start (){
        _audio = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    void Update()
    {
        z = 1 - z;
        this.gameObject.transform.position += new Vector3(.1f,.1f,0f);

        //modifying the Vector3, based on input multiplied by speed and time
        currentEulerAngles += new Vector3(x, y, z) * Time.deltaTime * rotationSpeed;

        //apply the change to the gameObject
        transform.eulerAngles = currentEulerAngles;
    }

    public void Disapear(){ // destroying the asteroids in the MM
        _audio.Play("Asteroid");
        this.gameObject.SetActive(false);
    }
}
