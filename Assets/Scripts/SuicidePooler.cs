using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicidePooler : MonoBehaviour
{
    public static SuicidePooler _Instance;
    public GameObject enemySpawner; // used to spawn the enemies on this gameobject

    public GameObject enemyObj;

    public int suicideS; // the number of how many to spawn of each, 10,10,5

    private List<GameObject> list;
    // Start is called before the first frame update
    void Start()
    {
        _Instance = this;
        list = new List<GameObject>();
        for(int i  = 0; i < suicideS; i++){
            GameObject obj = Instantiate(enemyObj,enemySpawner.transform.position,Quaternion.identity);
            obj.SetActive(false);
            list.Add(obj);
        }     
        
    }
    public GameObject SpawnSuiEnemy(){
        for(int i = 0; i < list.Count; i++){
            if(!list[i].activeInHierarchy){

                return list[i];
            }
            
        }
        return null;
    }
}
