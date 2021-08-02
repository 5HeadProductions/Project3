using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[System.Serializable]
public class Wave { // class = a container that will holds all the variables each wave must have
    [Header("Wave Stats")]
    public int waveNum; // number of waves 5 to be displayed on screen
    public int numOfEnemies; //used to stop the objects spawning
    public GameObject[] typesOfEnemies;
    public float enemySpawnRate; // spawn rate
}
public class EnemySpawner : MonoBehaviour
{
    
    public static EnemySpawner Instance; // allows StartRound to start the round
   
    public GameObject enemySpawner; // used to spawn the enemies on this gameobject
 
    public bool canSpawn = true;
    [Header("Text Fields")]
    public TextMeshProUGUI enemiesInWave;
    public TextMeshProUGUI waveRound;
    private bool pause = false; // start the round

    [Header("WAVES")]
    public Wave[] waves; //total waves
    private Wave currentWave; //instead of using waves[i].... to access the information inside the class we use this variable
    private int currentWaveNumber;
    private int numberOfEnemiesInWaveCounter = 0, enemyNum; // used to update the text field and used to tell the enemy pooler how many to set active
    private float timeBetweeSpawns;


    /*

        TODO: when 6 enemies need to be spawned 12 do bc it is spawning 2 at a time// needs fix
            health for ship, stop and shoot, boss spawning




    */
    private int enem = 12;
    private int temp = 12;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        RotateSpawner();
        if(pause == true){
             currentWave = waves[currentWaveNumber];
            waveRound.text = currentWave.waveNum.ToString();
            if(numberOfEnemiesInWaveCounter < currentWave.numOfEnemies){ // doesn't decrement count of the text until the ship is destroyed
                enemyNum = currentWave.numOfEnemies; // used to spawn enemies
                numberOfEnemiesInWaveCounter = currentWave.numOfEnemies; 
                if(currentWave.waveNum >= 2){
                enemiesInWave.text = "Enemies remaining " + temp.ToString();
                }else
                enemiesInWave.text = "Enemies remaining " + numberOfEnemiesInWaveCounter.ToString();
            } 
            SpawnWave();
            GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            if(totalEnemies.Length == 0 && !canSpawn)
            { 
                if(currentWaveNumber + 1 != waves.Length){ // making sure we dont try to access 1 past the end
                currentWaveNumber++;
                canSpawn = true;
                pause = false;
                }
            }
        }
    }

    public void BeginRound(){// this is called when the player hits the start button on the top of their screen
        pause = true;
  
    }

    void RotateSpawner(){ // keeps the spawner moving around Earth
        transform.eulerAngles += new Vector3(0,0,(40f * Time.deltaTime));
    }

    void SpawnWave(){
        int basicCount; // used to determine how many basic enemies to spawn
        int suiCount; // used to determine how many suicide enemies to spawn
        //call the get spawn enemy script here
        if(canSpawn && timeBetweeSpawns < Time.time){ // spawning enemies at different time not all at once
            if(currentWave.waveNum >= 2){ // suicides spawn on wave 5 and on
                if(currentWave.waveNum % 5 == 0){ // boss spawns in when the round is equally divisible by 5
                    //spawn a boss
                    switch (currentWave.waveNum)
                    {
                        case 5:
                            Boss(1);
                            break;
                        case 10:
                            Boss(1);
                            break;
                        case 15:
                            Boss(2);
                            break;
                        case 20:
                            Boss(2);
                            break;
                        case 25:
                            Boss(3);
                            break;
                        case 30:
                            Boss(3);
                            break;
                        case 35:
                            Boss(4);
                            break;
                        case 40:
                            Boss(4);
                            break;
                        case 45:
                            Boss(5);
                            break;
                        case 50:
                            Boss(5);
                            break;

                        default: break;
                    }
                }
                if(enemyNum % 2 == 0){
                    //evenly spawn basic and suicide ships
                     basicCount = enemyNum / 2;
                     suiCount = enemyNum / 2;       
                     BasicEnemies(basicCount);
                     SuicideEnemies(suiCount);
                  
                 }

            }else{

                BasicEnemies(enemyNum);
            }
            currentWave.numOfEnemies--;   //only subtract when they spawn in
            if(currentWave.numOfEnemies == 0){  // keeping track of how many enemies are supposed to spawn in each wave ////
                canSpawn = false;
                
            }
            

            timeBetweeSpawns = Time.time + currentWave.enemySpawnRate; 
        }
    }

    public void UpdateEnemyTracker(){   /// update the text field which will show how many enemies are in each round and update the counter as they get killed
        numberOfEnemiesInWaveCounter--;
        if(currentWave.waveNum >= 2){    
            temp --; 
        enemiesInWave.text = "Enemies remaining " + temp.ToString();
        }else
        enemiesInWave.text = "Enemies remaining " + numberOfEnemiesInWaveCounter.ToString();
    }


    public void SuicideEnemies(int max){  // enables the suicide ships across from the spawn position of the basic enemy
            GameObject obj = SuicidePooler._Instance.SpawnSuiEnemy(max);
            if(obj == null) return;   
            obj.SetActive(true);
            obj.transform.position = new Vector3 (-enemySpawner.transform.position.x, -enemySpawner.transform.position.y, enemySpawner.transform.position.z);
            obj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None; 
            }
    public void BasicEnemies(int max){ // enables the basic ship        
            GameObject obj = EnemyPooler._Instance.SpawnEnemy(max);
            if(obj == null)return;   
            obj.SetActive(true);
            obj.transform.position = enemySpawner.transform.position;
            obj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
           
    }

    public void Boss(int max){ // enables the boss ship
         //   GameObject obj = EnemyPooler._Instance.SpawnBossEnemy(max + 20);
            // if(obj == null)return;   
            // obj.SetActive(true);
            // obj.transform.position = new Vector3 (enemySpawner.transform.position.x, -enemySpawner.transform.position.y, enemySpawner.transform.position.z);
            // obj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
    }

}
