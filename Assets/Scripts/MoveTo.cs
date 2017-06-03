using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{

    public Transform goal;
    float tt;

    void Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position;
    }

    private void Update()
    {
        tt += Time.deltaTime;

        if( 3.0f < tt )
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.ResetPath();

            agent.destination = goal.position;
            tt = 0.0f;
        }
    }
}
