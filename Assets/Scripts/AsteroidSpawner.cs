using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{

    public GameObject asteroid;
    private float spawnRate = 2f, timeToCross = 0; // how many to spawn in back to back
    // Update is called once per frame
    void Update()
    {
        if(Time.time > timeToCross){ 
            GameObject obj = ObjectPooler.Instance.GetPooledObject();
            if(obj == null) return;
            
            GameObject parent = GameObject.Find("NewMMPanel"); // spawn them in the correct canvas
            if(parent == null){
                parent = GameObject.Find("Play Canvas");
            }
            if(parent.activeInHierarchy){
            obj.transform.SetParent(parent.transform);
            }

            float spawnPositionX = Random.Range(-1, 800);
            gameObject.transform.position = new Vector2 (spawnPositionX, gameObject.transform.position.y); //moving the spawner to a different coordinate
            obj.transform.position = gameObject.transform.position; // moving the object to the new position
            obj.transform.localScale = new Vector3 (.25f, .25f, .25f);
            obj.SetActive(true); //spawning it at the new position

            timeToCross = Time.time + spawnRate;
        }
    }
}
