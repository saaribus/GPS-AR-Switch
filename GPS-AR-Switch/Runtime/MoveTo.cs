using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveTo : MonoBehaviour
{
    [Header("Configuration of Automatic Walking")]
    [Space(2)]
    [Header("Set Reference to the Object the NavMesh Agent has to follow")]
    [Space(7)]
    [Tooltip("Select the Position Target GameObject as the Target")]
    [SerializeField] Transform target;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (target == null)
        {
            Debug.LogError("The Reference to the target has not been set");
        }
    }

    void Update()
    {
        
        agent.destination = target.position;
    }
}