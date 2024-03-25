using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Activateur : MonoBehaviour
{
    UnityEvent _evenementActiverBonus = new UnityEvent();
    public UnityEvent evenementActiverBonus => _evenementActiverBonus;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
