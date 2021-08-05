using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
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
    public bool canSpawn = true; // determines if enemies can keep on spawning
    [Header("Text Fields")]
    public TextMeshProUGUI enemiesInWave;
    public TextMeshProUGUI waveRound;
    private bool pause = false; // button that start the round 

    [Header("WAVES")]
    public Wave[] waves; //total waves
    private Wave currentWave; //instead of using waves[i].... to access the information inside the class we use this variable
    private int currentWaveNumber; // used to traverse our array of waves and keep track of when the end is reached
    private int numberOfEnemiesInWaveCounter = 0, enemiesToSpawn; // used to update the text field and used to tell the enemy pooler how many to set active
    private float timeBetweeSpawns;

    public Button button;
    /*

        TODO:health for ship, boss spawning, separate code chunks into their own functions, add a conuter to keep track
            of the total bosses spawned, every boss rounnd numOfEnemies should be odd, subtract by 1 and evenly spawn b & s then the extra
            enemies are gonna be the bosses

    */
    private int basicEnemySpawnTracker = 0, suicideEnemySpawnTracker = 0, bossCount = 0; // keeping track of how many enemies spawned in, in order to stop them from continously spawning

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
            if(numberOfEnemiesInWaveCounter < currentWave.numOfEnemies){ // doesn't decrement count of the text until the ships start getting destroyed
                enemiesToSpawn = currentWave.numOfEnemies; // used to spawn enemies
                numberOfEnemiesInWaveCounter = currentWave.numOfEnemies; 
                enemiesInWave.text = "Enemies remaining " + numberOfEnemiesInWaveCounter.ToString();
            } 
            SpawnWave();
            GameObject[] activeEnemies = GameObject.FindGameObjectsWithTag("Enemy"); // finding how many enemies are ON the scene
            if(activeEnemies.Length == 0 && !canSpawn)
            { 
                if(currentWaveNumber + 1 != waves.Length){ // making sure we dont try to access 1 past the end
                currentWaveNumber++;
                canSpawn = true;
                pause = false;
                button.gameObject.SetActive(true);
                }
            }
        }
    }

    public void BeginRound(){// this is called when the player hits the start button on the top of their screen
        pause = true;
    }

    void RotateSpawner(){ // keeps the spawner moving around Earth
        transform.eulerAngles += new Vector3(0,0,(100f * Time.deltaTime));
    }

    void SpawnWave(){
        int totalBasicEnemiesToSpawn; // used to determine how many basic enemies to spawn
        int totalSuicideEnemiesToSpawn; // used to determine how many suicide enemies to spawn
        if(canSpawn && timeBetweeSpawns < Time.time){ // spawning enemies at different time not all at once

            if(currentWave.waveNum >= 2){ // waves with more than justt he basic enemies 
                if(currentWave.waveNum % 5 == 0){ // boss spawns in when the round is equally divisible by 5
                    //spawn a boss
                    switch (currentWave.waveNum)
                    {
                        case 5:
                            Boss();
                            break;
                        case 10:
                            Boss();
                            break;
                        case 15:
                            Boss();
                            break;
                        case 20:
                            Boss();
                            break;
                        case 25:
                            Boss();
                            break;
                        case 30:
                            Boss();
                            break;
                        case 35:
                            Boss();
                            break;
                        case 40:
                            Boss();
                            break;
                        case 45:
                            Boss();
                            break;
                        case 50:
                            Boss();
                            break;

                        default: break;
                    }
                }
                if(enemiesToSpawn % 2 == 0){
                    //evenly spawn basic and suicide ships
                     totalBasicEnemiesToSpawn = enemiesToSpawn / 2;
                     totalSuicideEnemiesToSpawn = enemiesToSpawn / 2; 

                     //stoping the spawning of more enemies than specified
                     if(basicEnemySpawnTracker <= totalBasicEnemiesToSpawn) BasicEnemies();
                     
                    if(suicideEnemySpawnTracker <= totalSuicideEnemiesToSpawn)SuicideEnemies();
                    
                 }
                    currentWave.numOfEnemies = currentWave.numOfEnemies - 2;
                    if(currentWave.numOfEnemies == 0)canSpawn = false;
            }else{

                BasicEnemies();
            currentWave.numOfEnemies--;   //only subtract when they spawn in
            if(currentWave.numOfEnemies == 0) canSpawn = false; // keeping track of how many enemies are supposed to spawn in each wave ////canSpawn = false;
            }
            timeBetweeSpawns = Time.time + currentWave.enemySpawnRate; 
        }
    }

    public void UpdateEnemyTracker(){   /// update the text field which will show how many enemies are in each round and update the counter as they get killed
        numberOfEnemiesInWaveCounter--;
        enemiesInWave.text = "Enemies remaining " + numberOfEnemiesInWaveCounter.ToString();
    }


    public void SuicideEnemies(){  // enables the suicide ships across from the spawn position of the basic enemy
            GameObject obj = SuicidePooler._Instance.SpawnSuiEnemy();
            if(obj == null) return;   
            obj.SetActive(true);
            obj.transform.position = new Vector3 (-enemySpawner.transform.position.x, -enemySpawner.transform.position.y, enemySpawner.transform.position.z);
           // obj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None; 
            suicideEnemySpawnTracker ++;
            }
    public void BasicEnemies(){ // enables the basic ship        
            GameObject obj = EnemyPooler._Instance.SpawnEnemy();
            if(obj == null)return;   
            obj.SetActive(true);
            obj.transform.position = enemySpawner.transform.position;
            obj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            basicEnemySpawnTracker ++;
           
    }

    public void Boss(){ // enables the boss ship
           GameObject obj = BossPooler._Instance.SpawnBoss();
            if(obj == null)return;   
            obj.SetActive(true);
            obj.transform.position = new Vector3 (enemySpawner.transform.position.x, -enemySpawner.transform.position.y, enemySpawner.transform.position.z);
            obj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            bossCount++;
    }

}
