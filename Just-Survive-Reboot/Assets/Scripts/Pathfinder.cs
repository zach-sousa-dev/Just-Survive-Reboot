using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/**
 * Enemy
 * Detecs the fastest path to a target and draws a line to it
 * @version 0.1.0
 * @author Eli Wood
 */
public class Pathfinder : MonoBehaviour
{
    public GameObject target;

    public NavMeshAgent agent;

    //private NavMeshPath path;
    //private float elapsed = 0.0f;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //path = new NavMeshPath();
        //elapsed = 0.0f;
    }

    // Update is called once per frame
    protected virtual void Update()
    {

        agent.SetDestination(target.transform.position);

        /*
        elapsed += Time.deltaTime;
        if (elapsed > 0.5f && target != null)
        {
            elapsed -= 0.5f;
            NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
        }
        
        for (int i = 0; i < path.corners.Length - 1; i++) //display the path
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
        }
        */
        
    }
}
