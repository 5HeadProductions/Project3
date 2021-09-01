using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
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
    //public TextMeshProUGUI enemiesInWave;
    public TextMeshProUGUI waveRound;
    private bool pause = false; // button that start the round 
    [Header("StartButtonAnimation")][SerializeField]Animator animator;

    [Header("WAVES")]
    public Wave[] waves; //total waves
    private Wave currentWave; //instead of using waves[i].... to access the information inside the class we use this variable
    private int currentWaveNumber; // used to traverse our array of waves and keep track of when the end is reached
    private int numberOfEnemiesInWaveCounter = 0, enemiesToSpawn; // used to update the text field and used to tell the enemy pooler how many to set active
    private float timeBetweeSpawns;
    private int basicEnemySpawnTracker = 0, suicideEnemySpawnTracker = 0, bossCount = 0, temp; // keeping track of how many enemies spawned in, in order to stop them from continously spawning
    [Header("NumberOfBosses")][SerializeField] private int[] bossPerRound; // keeps track of how many bosses to spawn when there is a boss fight
    private int bossToSpawnIndex = 0; // keep track of which index to use the bossPerRound array

    [SerializeField]GameObject winningCanvas;
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
            if(PhotonNetwork.OfflineMode)
            waveRound.text = currentWave.waveNum.ToString();
            else
            this.GetComponent<PhotonView>().RPC("UpdateWaveNumber", RpcTarget.All, currentWave.waveNum);
           // if(numberOfEnemiesInWaveCounter < currentWave.numOfEnemies){ // doesn't decrement count of the text until the ships start getting destroyed
                enemiesToSpawn = currentWave.numOfEnemies; // used to spawn enemies
            //     numberOfEnemiesInWaveCounter = currentWave.numOfEnemies; 
            //     if(currentWave.waveNum % 5 == 0){ // need to include the number of regular enemies plus the number of bosses, 
            //     if(numberOfEnemiesInWaveCounter <= 0){
            //         temp--;
            //         enemiesInWave.text = "Enemies remaining " + temp.ToString();
            //     }
            //     else enemiesInWave.text = "Enemies remaining " + (numberOfEnemiesInWaveCounter + bossPerRound[bossToSpawnIndex]).ToString();
            //     }else enemiesInWave.text = "Enemies remaining " + numberOfEnemiesInWaveCounter.ToString();
            // } 
            SpawnWave();
            // condition to end the spawning of the round
            GameObject[] activeEnemies = GameObject.FindGameObjectsWithTag("Enemy"); // finding how many enemies are ON the scene
            GameObject[] activeBosses = GameObject.FindGameObjectsWithTag("Boss");
            GameObject[] activeSuicides = GameObject.FindGameObjectsWithTag("Suicide");
            if(activeEnemies.Length == 0 && !canSpawn && activeBosses.Length == 0 && activeSuicides.Length == 0)
            { 
                if(currentWave.waveNum % 5 == 0 && bossToSpawnIndex < bossPerRound.Length){
                    bossToSpawnIndex ++;
                }
                if(currentWaveNumber + 1 != waves.Length){ // making sure we dont try to access 1 past the end
                currentWaveNumber++;
                canSpawn = true;
                pause = false;


                if(PhotonNetwork.OfflineMode)
                    animator.SetTrigger("FadeIn");
                else
                    this.GetComponent<PhotonView>().RPC("FadeInStartButton", RpcTarget.All);
                
                }
            }
                if(currentWave.waveNum == 20){
                    if(activeEnemies.Length == 0 && activeSuicides.Length == 0 && activeBosses.Length == 0){
                       StartCoroutine(Delay());
                    }
                }
        }
    }

    IEnumerator Delay(){
        yield return new WaitForSeconds(3);
         winningCanvas.SetActive(true);
    }

    public void BeginRound(){// this is called when the player hits the start button on the top of their screen
        pause = true;
        basicEnemySpawnTracker = 0;
        suicideEnemySpawnTracker = 0;
        bossCount = 0;
    }

    void RotateSpawner(){ // keeps the spawner moving around Earth
        transform.eulerAngles += new Vector3(0,0,(100f * Time.deltaTime));
    }

    void SpawnWave(){
        int totalBasicEnemiesToSpawn; // used to determine how many basic enemies to spawn
        int totalSuicideEnemiesToSpawn; // used to determine how many suicide enemies to spawn

        if(canSpawn && timeBetweeSpawns < Time.time){ // spawning enemies at different time not all at once
            if(currentWave.waveNum >= 6){ // waves with more than justt he basic enemies 
                if(currentWave.waveNum % 5 == 0){ // boss spawns in when the round is equally divisible by 5 
                // boss rounds must have odd number of enemies
                BossRound();
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
                if(currentWave.waveNum % 5 == 0){ // boss spawns in when the round is equally divisible by 5 should only happen at round 5
                BossRound();
                }
                BasicEnemies();
            currentWave.numOfEnemies--;   //only subtract when they spawn in
            if(currentWave.numOfEnemies == 0) canSpawn = false; // keeping track of how many enemies are supposed to spawn in each wave ////canSpawn = false;
            }
            timeBetweeSpawns = Time.time + currentWave.enemySpawnRate; 
        }
    }

    public void UpdateEnemyTracker(){   /// update the text field which will show how many enemies are in each round and update the counter as they get killed
        numberOfEnemiesInWaveCounter--;
        // if(currentWave.waveNum % 5 == 0){ // need to include the number of regular enemies plus the number of bosses
        //     enemiesInWave.text = "Enemies remaining " + (numberOfEnemiesInWaveCounter + bossPerRound[bossToSpawnIndex]).ToString();   
        // }else enemiesInWave.text = "Enemies remaining " + numberOfEnemiesInWaveCounter.ToString();
       
    }


    public void BossRound(){
        switch (currentWave.waveNum)
            {
                case 5:
                if(bossCount < bossPerRound[bossToSpawnIndex])Boss();
                    break;
                case 10:
                if(bossCount < bossPerRound[bossToSpawnIndex])Boss();;
                    break;
                case 15:
                if(bossCount < bossPerRound[bossToSpawnIndex])Boss();
                    break;
                case 20:
                if(bossCount < bossPerRound[bossToSpawnIndex])Boss();
                    break;
                // case 25:
                // if(bossCount < bossPerRound[bossToSpawnIndex])Boss();
                //     break;
                // case 30:
                // if(bossCount < bossPerRound[bossToSpawnIndex])Boss();
                //     break;
                // case 35:
                // if(bossCount < bossPerRound[bossToSpawnIndex])Boss();
                //     break;
                // case 40:
                // if(bossCount < bossPerRound[bossToSpawnIndex])Boss();
                //     break;
                // case 45:
                // if(bossCount < bossPerRound[bossToSpawnIndex])Boss();
                //     break;
                // case 50:
                // if(bossCount < bossPerRound[bossToSpawnIndex])Boss();
                //     break;
                default: break;
            }
    }


    public void SuicideEnemies(){  // enables the suicide ships across from the spawn position of the basic enemy
            GameObject obj = SuicidePooler._Instance.SpawnSuiEnemy();
            if(obj == null) return;   
            obj.transform.position = new Vector3 (-enemySpawner.transform.position.x, -enemySpawner.transform.position.y, enemySpawner.transform.position.z);
            if(PhotonNetwork.OfflineMode)
            obj.SetActive(true);
            else{
            this.GetComponent<PhotonView>().RPC("EnableSuicides", RpcTarget.AllBuffered,obj.GetComponent<PhotonView>().ViewID);
            }
            obj.GetComponent<BasicEnemy>().onSpawnFeedback?.Initialization(); // needed to play next feedback
            obj.GetComponent<BasicEnemy>().onSpawnFeedback?.PlayFeedbacks(); // resetting their color, both feedbacks needed "Allow additive plays"
            suicideEnemySpawnTracker ++;
            }
    public void BasicEnemies(){ // enables the basic ship        
            GameObject obj = EnemyPooler._Instance.SpawnEnemy();
            if(obj == null)return;   
            obj.transform.position = enemySpawner.transform.position;
            if(PhotonNetwork.OfflineMode)
            obj.SetActive(true);
            else
            this.GetComponent<PhotonView>().RPC("EnableGameObject", RpcTarget.AllBuffered,obj.GetComponent<PhotonView>().ViewID);
            if(PhotonNetwork.OfflineMode){
            obj.GetComponent<BasicEnemy>().onSpawnFeedback?.Initialization();
            obj.GetComponent<BasicEnemy>().onSpawnFeedback?.PlayFeedbacks();
            }
            else{
                this.gameObject.GetComponent<PhotonView>().RPC("PlayFeedbacks", RpcTarget.All, obj.GetComponent<PhotonView>().ViewID);
            }
            obj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            basicEnemySpawnTracker ++;
           
    }

    public void Boss(){ // enables the boss ship
           GameObject obj = BossPooler._Instance.SpawnBoss();
            if(obj == null)return;   
            obj.transform.position = new Vector3 (enemySpawner.transform.position.x, -enemySpawner.transform.position.y, enemySpawner.transform.position.z);
            if(PhotonNetwork.OfflineMode)
            obj.SetActive(true);
            else
            this.GetComponent<PhotonView>().RPC("EnableGameObject", RpcTarget.AllBuffered,obj.GetComponent<PhotonView>().ViewID);
            obj.GetComponent<BasicEnemy>().onSpawnFeedback?.Initialization();
            obj.GetComponent<BasicEnemy>().onSpawnFeedback?.PlayFeedbacks();
            obj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            bossCount++;
            temp = bossCount;
      

    }

    [PunRPC]
    private void EnableGameObject(int targetViewID){
        PhotonView targetPhotonView = PhotonView.Find(targetViewID);
            if(targetPhotonView != null){
                targetPhotonView.gameObject.SetActive(true);
            }

        
    }
    [PunRPC]
    private void EnableSuicides(int targetViewID){
        PhotonView targetPhotonView = PhotonView.Find(targetViewID);
            if(targetPhotonView != null){
                targetPhotonView.gameObject.SetActive(true);
            }  
    }

    [PunRPC]
    private void UpdateWaveNumber(int currentWaveNumber){
        waveRound.text = currentWaveNumber.ToString();
    }

    [PunRPC]
    private void FadeInStartButton(){
        animator.SetTrigger("FadeIn");
    }

    [PunRPC]
    private void PlayFeedbacks(int viewID){
        PhotonView temp = PhotonView.Find(viewID);
            if(temp != null){
                temp.GetComponent<BasicEnemy>().onSpawnFeedback?.Initialization();
            temp.GetComponent<BasicEnemy>().onSpawnFeedback?.PlayFeedbacks();
            }
            
    }

}
