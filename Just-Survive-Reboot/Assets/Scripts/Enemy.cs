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
public class Enemy : MonoBehaviour
{
    public Transform target;

    public NavMeshAgent agent;

    private NavMeshPath path;
    private float elapsed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        path = new NavMeshPath();
        elapsed = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {

        


        elapsed += Time.deltaTime;
        if (elapsed > 1.0f && target != null)
        {
            elapsed -= 1.0f;
            NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
        }
        
        for (int i = 0; i < path.corners.Length - 1; i++) //display the path
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
        }
        
        
    }
}
