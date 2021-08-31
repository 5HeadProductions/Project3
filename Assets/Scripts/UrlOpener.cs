using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrlOpener : MonoBehaviour
{
 public string URl;
 public void OpenUrl(){
     Application.OpenURL(URl);
 }
}
