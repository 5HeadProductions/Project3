using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPooler : MonoBehaviour
{


    public static EnemyPooler _Instance;
    public GameObject enemySpawner; // used to spawn the enemies on this gameobject

    public GameObject[] enemyObj;

    public int basicS, suicideS, bossS; // the number of how many to spawn of each, 10,10,5


    private List<GameObject> list;
    // Start is called before the first frame update
    void Start()
    {
        _Instance = this;
        list = new List<GameObject>();

        for(int i  = 0; i < basicS; i++){
            GameObject obj = Instantiate(enemyObj[0],enemySpawner.transform.position,Quaternion.identity);
            obj.SetActive(false);
            list.Add(obj);
        }
        for(int i  = 0; i < suicideS; i++){
            GameObject obj = Instantiate(enemyObj[1],enemySpawner.transform.position,Quaternion.identity);
            obj.SetActive(false);
            list.Add(obj);
        }
        for(int i  = 0; i < bossS; i++){
            GameObject obj = Instantiate(enemyObj[2],enemySpawner.transform.position,Quaternion.identity);
            obj.SetActive(false);
            list.Add(obj);
        }
        
        
        
    }
    public GameObject SpawnEnemy(int numOfEnemies){
        for(int i = 0; i < numOfEnemies; i++){
            if(!list[i].activeInHierarchy){
                return list[i];
            }
        }
        return null;
    }
}
