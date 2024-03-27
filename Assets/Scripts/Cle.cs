using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cle : MonoBehaviour
{
    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Porte.aCle = true;
            Destroy(gameObject);    
        }
    }
}
