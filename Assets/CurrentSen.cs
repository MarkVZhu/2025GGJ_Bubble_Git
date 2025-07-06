using System.Collections;
using System.Collections.Generic;
using MarkFramework;
using UnityEngine;

public class CurrentSen : SingletonMono<CurrentSen>
{
    public float currentSen;
    
    // Start is called before the first frame update
    void Start()
    {
        currentSen = 35;
        DontDestroyOnLoad(this.gameObject);
    }
}
