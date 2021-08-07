using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPooler : MonoBehaviour
{
    public static BossPooler _Instance;
    public GameObject enemySpawner; // used to spawn the enemies on this gameobject

    public GameObject enemyObj;

    public int bossCount; 


    private List<GameObject> list;
    // Start is called before the first frame update
    void Start()
    {
        _Instance = this;
        list = new List<GameObject>();
        for(int i  = 0; i < bossCount; i++){
            GameObject obj = Instantiate(enemyObj,enemySpawner.transform.position,Quaternion.identity);
            obj.SetActive(false);
            list.Add(obj);
        }     
        
    }
    public GameObject SpawnBoss(){
        for(int i = 0; i < list.Count; i++){
            if(!list[i].activeInHierarchy){
                return list[i];
            }
            
        }
        return null;
    }
}
