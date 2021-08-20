using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyPooler : MonoBehaviourPun
{

// basic enemy pooler
    public static EnemyPooler _Instance;
    public GameObject enemySpawner; // used to spawn the enemies on this gameobject

    public GameObject enemyObj;

    public int basicS; // the number of how many to spawn of each, 10,10,5

    private List<GameObject> list;
    // Start is called before the first frame update
    void Start()
    {
        _Instance = this;
        list = new List<GameObject>();

        for(int i  = 0; i < basicS; i++){
            GameObject obj;
            if(PhotonNetwork.OfflineMode == true){
                obj = Instantiate(enemyObj,enemySpawner.transform.position,Quaternion.identity);
                obj.SetActive(false);
            }
            else{
            obj = PhotonNetwork.Instantiate(enemyObj.name,enemySpawner.transform.position,Quaternion.identity);
            this.GetComponent<PhotonView>().RPC("DisableGameObject", RpcTarget.AllBuffered,obj.GetComponent<PhotonView>().ViewID);
            }
            
            list.Add(obj);
            
        }
        
    }
    public GameObject SpawnEnemy(){
        for(int i = 0; i < list.Count; i++){    
            if(!list[i].activeInHierarchy){
                return list[i];
            }
        }
        return null;
    }

    [PunRPC]
    private void DisableGameObject(int targetViewID){
        PhotonView targetPhotonView = PhotonView.Find(targetViewID);
            if(targetPhotonView != null)
            targetPhotonView.gameObject.SetActive(false);
    }

    
}
