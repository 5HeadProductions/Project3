using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public void OnCollisionEnter2D(Collision2D col){ // inner collider
        if(col.transform.tag == "Suicide"){
          //  col.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
            Debug.Log("Crashed on Earth");
            EnemySpawner.Instance.UpdateEnemyTracker();
            col.gameObject.SetActive(false);
        }
    }

    public void OnTriggerEnter2D(Collider2D col){ // outer collider
        if(col.transform.tag == "Enemy"){
            col.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        }
    }

    
}
