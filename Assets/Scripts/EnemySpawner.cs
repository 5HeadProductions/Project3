using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    public GameObject enemySpawner;

    public GameObject pain;

    private bool pause = false;
    public int[] basicPerRound;
    public int[] suicidePerRound;
    public int[] bossPerRound;

    public EnemyStats basic, suicide, boss;
    // Start is called before the first frame update
    void Start()
    {
       
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(pause == true){
        
            RotateSpawner();
        }
    }

    public void SpawnEnemy(){
        Debug.Log(basicPerRound.Length.ToString());
        pain = Instantiate(basic.shipType, enemySpawner.transform.position,Quaternion.identity); 
        pause = true;
        
    }
    void RotateSpawner(){
        transform.eulerAngles += new Vector3(0,0,(10f * Time.deltaTime));
    }
}
