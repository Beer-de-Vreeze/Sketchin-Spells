using System;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class TestingEvents : MonoBehaviour
{
    public delegate void GoedeNaamEvent(int random);

    public event GoedeNaamEvent DoEvent;

    void Start()
    {
        DoEvent += TestEvent;
        int random = UnityEngine.Random.Range(0, 1000000);
        DoEvent -= TestEvent;
        DoEvent += TestEvent2;
        random = UnityEngine.Random.Range(0, 1000000);
        DoEvent -= TestEvent2;
        DoEvent?.Invoke(random);
    }

    // Update is called once per frame
    void Update() 
    {

     }

    public void TestEvent(int random)
    {
        Debug.Log("Event is called with random number: " + random);
    }

    public void TestEvent2(int random)
    {
        Debug.Log("Event is called with random number: " + random);
    } 
}
