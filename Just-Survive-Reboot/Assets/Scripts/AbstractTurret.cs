using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;


/**
 * AbstractTurret
 * Reusable turret class for any type of turret.
 * @version     1.0.0
 * @author      Zachary Sousa
 */
public abstract class AbstractTurret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected GameObject bottom;
    [SerializeField] protected GameObject head;
    [SerializeField] protected GameObject gun;
    [SerializeField] protected AudioClip movingSound;
    [SerializeField] protected AudioClip detectionSound;
    [SerializeField] protected AudioClip deactivateSound;
    [SerializeField] protected AudioClip shotSound;
    [SerializeField] protected ParticleSystem muzzleFlash;
    [SerializeField] protected AudioSource bodySrc;
    [SerializeField] protected AudioSource gunSrc;
    [SerializeField] protected AudioSource headSrc;

    [Header("Variables")]
    [SerializeField] protected float idleRotationAmount;
    [SerializeField] protected float volumeRamping;
    [SerializeField] protected float newTaskTime;
    [SerializeField] protected float rangeRadius;
    [SerializeField] protected float lockOnSpeed;
    [SerializeField] protected float detectionVolume;
    [SerializeField] protected string enemyLayer;
    [SerializeField] protected float buildAnimationSpeed;

    [Header("Weapon Stats")]
    [SerializeField] protected float dmg;
    [SerializeField] protected float fireRate;


    protected Quaternion idleRotationVector;
    protected float timePassed;
    protected int idleTaskId = 0;
    protected GameObject[] enemies;
    protected ArrayList targets = new ArrayList();
    protected bool hasTarget = false;
    protected Vector3 lastNewDir;

    protected NavMeshObstacle obstacle;

    protected Vector3 initialSize;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake() {
        //bodySrc = GetComponent<AudioSource>();
        bodySrc.loop = true;
        bodySrc.clip = movingSound;
        bodySrc.volume = 0;
        bodySrc.spatialBlend = 1;

        initialSize = transform.localScale;
        transform.localScale = Vector3.zero;

        obstacle = GetComponent<NavMeshObstacle>();
        obstacle.carving = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, initialSize, Time.deltaTime * buildAnimationSpeed);

        if(hasTarget) {
            aimAtTarget((GameObject)targets[0]);
            fireAtRate(fireRate);//should wait to fire until the target is in front of the turret
        } else {
            newTask(newTaskTime, 3);
            doIdleTask(idleTaskId);
        }

        enemies = GameObject.FindGameObjectsWithTag("Enemy");   //this whole section is probably stupid resource instensive but its fine for now :D
        targets = new ArrayList();
        for (int i = 0; i < enemies.Length; i++) {
            if(Vector3.Distance(transform.position, enemies[i].transform.position) <= rangeRadius && enemies[i].layer == 7) {
                targets.Add(enemies[i]);
            }
        }

        if(targets.Count > 0 && hasTarget == false) {
            hasTarget = true;
            //bodySrc.Stop();
            headSrc.volume = detectionVolume;
            headSrc.PlayOneShot(detectionSound);
        }
        if (targets.Count == 0 && hasTarget == true) {
            headSrc.volume = detectionVolume;
            headSrc.PlayOneShot(deactivateSound);
            hasTarget = false;
            //bodySrc.Play();
        }
    }

    protected virtual void newTask(float rate, int range) {
        if(timePassed >= rate) {
            idleTaskId = (UnityEngine.Random.Range(0, range));
            timePassed = 0;
        }
        timePassed += Time.deltaTime;
    }

    protected virtual void fireAtRate(float rate) {
        if (timePassed >= rate) {
            timePassed = 0;
            fire();
        }
        timePassed += Time.deltaTime;
    }

    protected virtual void fire() {
        muzzleFlash.Play();
        gunSrc.PlayOneShot(shotSound, 1);
    }

    protected virtual void doIdleTask(int task) {
        try {
            switch (task) {
                case 0: //do nothing
                    idleRotationVector = Quaternion.Euler(0, 0, 0);
                    bodySrc.clip = movingSound;
                    bodySrc.volume = Mathf.Lerp(bodySrc.volume, 0, Time.deltaTime * volumeRamping);
                    break;
                case 1: //rotate cw
                    idleRotationVector = Quaternion.Euler(0, idleRotationAmount, 0);
                    head.transform.rotation = head.transform.rotation * idleRotationVector;
                    bodySrc.clip = movingSound;
                    bodySrc.volume = Mathf.Lerp(bodySrc.volume, 1, Time.deltaTime * volumeRamping);
                    break;
                case 2: //rotate ccw
                    idleRotationVector = Quaternion.Euler(0, -idleRotationAmount, 0);
                    head.transform.rotation = head.transform.rotation * idleRotationVector;
                    bodySrc.clip = movingSound;
                    bodySrc.volume = Mathf.Lerp(bodySrc.volume, 1, Time.deltaTime * volumeRamping);
                    break;
            }
        } catch(Exception e) {
            Debug.LogError("Task number not in range." + e);
        }
    }

    protected virtual void aimAtTarget(GameObject obj) {
        Vector3 realTargetDir = obj.transform.position - head.transform.position;
        Vector3 levelTargetDir = new Vector3(obj.transform.position.x, head.transform.position.y, obj.transform.position.z) - head.transform.position;
        Vector3 newDir = (Vector3.RotateTowards(head.transform.forward, levelTargetDir, lockOnSpeed, 0f));

        if (newDir == lastNewDir) {
            bodySrc.volume = Mathf.Lerp(bodySrc.volume, 0, Time.deltaTime * volumeRamping);
        } else {
            bodySrc.volume = Mathf.Lerp(bodySrc.volume, 1, Time.deltaTime * volumeRamping);
        }

        head.transform.rotation = Quaternion.LookRotation(newDir);
        lastNewDir = newDir;
    }

    void OnDrawGizmosSelected() {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = new Color(0.3f, 0.4f, 0.6f, 0.5f);
        Gizmos.DrawSphere(transform.position, rangeRadius);
    }

}
