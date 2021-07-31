using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[System.Serializable]
public class Wave { // class = a container that will holds all the variables each wave must have
    public int waveNum; // number of waves 5 to be displayed on screen
    public int numOfEnemies; //used to stop the objects spawning
    public GameObject[] typesOfEnemies;
    public float enemySpawnRate; // spawn rate
}
public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance; // allows StartRound to start the round
    public GameObject enemySpawner; // used to spawn the enemies on this gameobject
 
    private bool pause = false; // start the round

    public TextMeshProUGUI enemiesInWave;

    public Wave[] waves; //total waves
    private Wave currentWave; //instead of using waves[i].... to access the information inside the class we use this variable
    private int currentWaveNumber;
    private int numberOfEnemiesInWaveCounter = 0, enemyNum; // used to update the text field and used to tell the enemy pooler how many to set active
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
            RotateSpawner();
        if(pause == true){
             currentWave = waves[currentWaveNumber];
           
            if(numberOfEnemiesInWaveCounter < currentWave.numOfEnemies){
                enemyNum = currentWave.numOfEnemies;
                numberOfEnemiesInWaveCounter = currentWave.numOfEnemies;
                enemiesInWave.text = "Enemies remaining " + numberOfEnemiesInWaveCounter.ToString();
            } 
            SpawnWave();
            GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
   /// upadate the text field which will show how many enemies are in each round and update the counter as they get killed
            if(totalEnemies.Length == 0 && !canSpawn)
            { // this makes the waves go back to back we only want to start the wave when the player presses the play button
                if(currentWaveNumber + 1 != waves.Length){
                currentWaveNumber++;
                canSpawn = true;
                pause = false;
                }
            }else{
              //  Debug.Log("GameOver");
            }
        }
    }

    public void BeginRound(){// this is called when the player hits the start button on the top of their screen

       // pain = Instantiate(basic.shipType, enemySpawner.transform.position,Quaternion.identity); 
        pause = true;
        
    }
    void RotateSpawner(){
        transform.eulerAngles += new Vector3(0,0,(40f * Time.deltaTime));
    }
    void SpawnWave(){
        //call the get spawn enemy script here
        if(canSpawn && timeBetweeSpawns < Time.time){
            //GameObject randomeEnemy = currentWave.typesOfEnemies[Random.Range(0, currentWave.typesOfEnemies.Length)]; // will be used in later rounds
            //Instantiate(randomeEnemy, enemySpawner.transform.position,Quaternion.identity); 
            GameObject obj = EnemyPooler._Instance.SpawnEnemy(enemyNum); // spawning in only the number a enemies specified in the inspector
            if(obj == null)return;
       
            obj.transform.position = enemySpawner.transform.position;
            obj.SetActive(true);
            obj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            currentWave.numOfEnemies--;
            timeBetweeSpawns = Time.time + currentWave.enemySpawnRate;
            if(currentWave.numOfEnemies == 0){
                canSpawn = false;
            }

        }


    }

    public void UpdateEnemyTracker(){
        numberOfEnemiesInWaveCounter--;
        enemiesInWave.text = "Enemies remaining " + numberOfEnemiesInWaveCounter.ToString();
    }

}
