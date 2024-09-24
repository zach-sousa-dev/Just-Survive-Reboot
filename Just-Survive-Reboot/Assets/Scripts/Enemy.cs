using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Pathfinder
{
    [SerializeField] private float speed;

    [SerializeField] private float damage;
    [SerializeField] private float range;
    [SerializeField] private float attackSpeed;//effectively attack cooldown
    [SerializeField] private float hitStun;//freeze movement

    [SerializeField] private PlayerResources player;


    protected int layerMask;
    protected float timePassed;

    // Start is called before the first frame update
    override protected void Start()
    {
        timePassed = attackSpeed; //allows immediate attack

        if (hitStun > attackSpeed)
        {
            hitStun = attackSpeed;
        }

        layerMask = 1 << 8;
        //layerMask = ~layerMask; don't do this for some reason
    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();

        timePassed += Time.deltaTime;

        if (timePassed > hitStun) {
            agent.speed = speed;
        }

        if (Vector3.Distance(transform.position, target.transform.position) <= range)
        {
            RaycastHit hit;

            if (timePassed > attackSpeed && Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, range, layerMask))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow, 1);
                //Debug.Log("Hit");
                Attack();
            } 
            else
            {
                //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * range, Color.white);//debug

                //rotate
                Vector3 targetDir = target.transform.position - transform.position;
                Vector3 levelTargetDir = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) - transform.position;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, levelTargetDir, agent.angularSpeed * Mathf.Deg2Rad * Time.deltaTime, 0f);

                transform.rotation = Quaternion.LookRotation(newDir);
            }
        }
    }

    protected void Attack()
    {
        
        timePassed = 0f;

        agent.speed = 0f;//potentially change to make the enemy lunge

            

        //Debug.Log("attack for " + damage + " damage");

        player.Hurt(damage);
    }

    /**
     * Applies modifiers to the enemy. All values are multipliers unless otherwise stated and default to do nothing. This means you can access only the field you want to change.
     * All modifiers are multiplicative, making them additive would require storing multiplier values.
     */
    public void ApplyModifiers(bool death = false, float speed = 1, float damage = 1, float range = 1, float attackSpeed = 1, float hitStun = 1) {
        if (death) {
            Destroy(gameObject);
            return;
        }

        this.speed *= speed;
        this.damage *= damage;
        this.range *= range;
        this.attackSpeed *= attackSpeed;
        this.hitStun *= hitStun;
    }
}
