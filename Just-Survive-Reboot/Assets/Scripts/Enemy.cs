using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Pathfinder
{
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;

    [SerializeField] private float speed;

    [SerializeField] private float damage;
    [SerializeField] private float range;
    [SerializeField] private float attackSpeed;//effectively attack cooldown
    [SerializeField] private float selfStun;//freeze movement


    protected int layerMask;
    protected float timePassed;

    // Start is called before the first frame update
    override protected void Start()
    {
        health = maxHealth;
        timePassed = attackSpeed; //allows immediate attack

        if (selfStun > attackSpeed)
        {
            selfStun = attackSpeed;
        }

        layerMask = 1 << 8;
        layerMask = ~layerMask;
    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();

        timePassed += Time.deltaTime;

        if (timePassed > selfStun) {
            agent.speed = speed;
        }

        if (Vector3.Distance(transform.position, target.transform.position) <= range)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, range, layerMask))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Debug.Log("Hit");
                Attack();
            }
        }
    }

    protected void Attack()
    {
        if (timePassed > attackSpeed)
        {
            timePassed = 0f;

            agent.speed = 0f;//potentially change to make the enemy lunge

            

            Debug.Log("attack for " + damage + " damage");
        }
    }

    public void Hurt(float damage)
    {
        health -= damage;

        if (health <= 0f)
        {
            //die
        }
        else
        {
            //make healthbar
        }


    }
}
