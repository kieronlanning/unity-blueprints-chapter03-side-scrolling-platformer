using UnityEngine;
using System.Collections;

public class GoalBehaviour : MonoBehaviour
{
    ParticleSystem _ps;

    // Use this for initialization
    void Start()
    {
        _ps = GetComponent<ParticleSystem>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (_ps.isPlaying)
        {
            print("You win!!!1!");
        }
    }
}