using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;
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
        
    }

    public void ChangePosition(){
        Debug.Log(basicPerRound.Length.ToString());
        Instantiate(basic.shipType, gameObject.transform.position, gameObject.transform.rotation); 
    }
}
