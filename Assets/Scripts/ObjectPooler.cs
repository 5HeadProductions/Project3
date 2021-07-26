using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{

    public static ObjectPooler Instance; // so other classes can reference this script and use its methods
    public GameObject pooledObj;  // the object to be spawned
    public int size;  // how many to spawn

    private List<GameObject> pooledObjList;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        pooledObjList = new List<GameObject>(); // set in start cannnot be changed dynamically
        for(int i = 0; i < size; i ++){  //instantiating as many objects as specified by size and storing them in the list
            GameObject obj = Instantiate(pooledObj);
            obj.SetActive(false);
            pooledObjList.Add(obj);
        }
    }
    public GameObject GetPooledObject(){
        if(pooledObjList.Count > 0){ //making sure the list hasn't been cleared yet
        //looping through all the object in the list and "respawning" all the "dead" object
        for(int i = 0; i < pooledObjList.Count; i++){
            if(!pooledObjList[i].activeInHierarchy){
                return pooledObjList[i];      
                }
            }
        }
        return null;
    }

    public void Stop(){
        pooledObjList.Clear(); // stoppping the object from being pooled
    }
}
