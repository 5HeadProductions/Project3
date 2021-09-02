using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyProjectile : MonoBehaviour
{
    public EnemyStats enemyStats;
       // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnCollisionEnter2D(Collision2D col){
        
        if(col.gameObject.CompareTag("Turret") ){
            GameObject.Find("FeedbackManager").GetComponent<FeedbackManager>().ShipExplosion(new Vector3(col.transform.position.x,col.transform.position.y, 0));
                //turret got shot by enemy bullet, then destroy turret
                if(PhotonNetwork.OfflineMode){
                Destroy(col.gameObject); // single player
            Destroy(this.gameObject); // single player

                }
                else{
                    this.GetComponent<PhotonView>().RPC("DestroyGameObject", RpcTarget.AllBuffered, col.gameObject.GetComponent<PhotonView>().ViewID);
                    this.GetComponent<PhotonView>().RPC("DestroyGameObject", RpcTarget.AllBuffered, this.gameObject.GetComponent<PhotonView>().ViewID);
                }
        }
    }
    public int DoDamage(){
        return enemyStats.damage;
        
    }

    [PunRPC]
    public void DestroyGameObject(int gameObjectViewID){
            PhotonView temp = PhotonView.Find(gameObjectViewID);
            if(temp != null)
            Destroy(temp.gameObject);
    }


}
