using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autels : MonoBehaviour
{
    [SerializeField] string element;
    [SerializeField] SOPerso _donneesPerso;


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Element : " + element);
        }
    }
}
