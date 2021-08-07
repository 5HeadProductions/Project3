using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

public class TempHealthBar : MonoBehaviour
{
    public MMProgressBar healthBar;
    // Start is called before the first frame update
    void Start()
    {
        healthBar.Initialization();
    }

}
