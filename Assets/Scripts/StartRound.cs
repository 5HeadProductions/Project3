using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRound : MonoBehaviour
{

  public void StartR(){
    EnemySpawner.Instance.BeginRound();
  }
}
