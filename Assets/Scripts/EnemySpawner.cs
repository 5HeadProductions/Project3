using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Wave { // class = a container that will holds all the variables each wave must have
    public int waveNum; // number of waves 5 to be displayed on screen
    public int numOfEnemies;
    public GameObject[] typesOfEnemies;
    public float enemySpawnRate; // spawn rate
}
public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance; // allows StartRound to start the round
    public GameObject enemySpawner; // used to spawn the enemies on this gameobject
 
    private bool pause = false; // start the round

    public Wave[] waves; //total waves

    private Wave currentWave; //instead of using waves[i].... to access the information inside the class we use this variable
    private int currentWaveNumber;
    private float timeBetweeSpawns;

    public bool canSpawn = true;
    // Start is called before the first frame update
    void Start()
    {
       
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(pause == true){
             currentWave = waves[currentWaveNumber];
            RotateSpawner();
            SpawnWave();
        }
    }

    public void BeginRound(){// this is called when the player hits the start button on the top of their screen

       // pain = Instantiate(basic.shipType, enemySpawner.transform.position,Quaternion.identity); 
        pause = true;
        
    }
    void RotateSpawner(){
        transform.eulerAngles += new Vector3(0,0,(20f * Time.deltaTime));
    }
    void SpawnWave(){
        if(canSpawn && timeBetweeSpawns < Time.time){
            GameObject randomeEnemy = currentWave.typesOfEnemies[Random.Range(0, currentWave.typesOfEnemies.Length)]; // will be used in later rounds
            Instantiate(randomeEnemy, enemySpawner.transform.position,Quaternion.identity); 
            currentWave.numOfEnemies--;
            timeBetweeSpawns = Time.time + currentWave.enemySpawnRate;
            if(currentWave.numOfEnemies == 0){
                canSpawn = false;
            }
        }
    }

}
