using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [field: SerializeField] protected string name { get; set; }

    [field: Header("Rates and Timings")]
    [field: SerializeField] protected float fireRateDelay { get; set; }
    [field: SerializeField] protected float reloadTime { get; set; }

    [field: Header("Ammo")]
    [field: SerializeField] protected int maxAmmo { get; set; }
    [field: SerializeField] protected int dryAmmoThreshold { get; set; }
    [field: SerializeField] protected int currentAmmo { get; set; }
    [field: SerializeField] protected int reserveAmmo { get; set; }

    [field: Header("Handling")]
    [field: SerializeField] protected float accuracy { get; set; }
    [field: SerializeField] protected float recoil { get; set; }

    [field: Header("Capability")]
    [field: SerializeField] protected float maxRange { get; set; }
    [field: SerializeField] protected float damage { get; set; }

    [field: Header("Object References")]
    [field: SerializeField] protected Camera cam { get; set; }
    [field: SerializeField] protected LayerMask layerMask { get; set; }
    [field: SerializeField] protected GameObject weaponModel { get; set; }
    [field: SerializeField] protected Animator animator { get; set; }
    [field: SerializeField] protected AudioSource audioSrc { get; set; }
    [field: SerializeField] protected GameObject wallHitEffect { get; set; }

    [field: Header("Animation States")]
    [field: SerializeField] protected string fireAnimation { get; set; }
    [field: SerializeField] protected string emptyAnimation { get; set; }
    [field: SerializeField] protected string idleAnimation { get; set; }
    [field: SerializeField] protected string reloadAnimation { get; set; }
    [field: SerializeField] protected string equipAnimation { get; set; }

    [field: Header("Sounds")]
    [field: SerializeField] protected AudioClip fireSound { get; set; }
    [field: SerializeField] protected AudioClip noAmmoSound { get; set; }
    [field: SerializeField] protected AudioClip dryFireSound { get; set; }
    [field: SerializeField] protected AudioClip reloadSound { get; set; }
    [field: SerializeField] protected AudioClip equipSound { get; set; }

    //  States
    protected bool canFire = true;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
        Debug.DrawRay(cam.transform.position, cam.transform.forward, Color.green);
    }

    protected virtual void Shoot()
    {
        if(currentAmmo > 0 && canFire)
        {
            StartCoroutine(FireRateRoutine());

            ShootBullet(cam.transform.position, cam.transform.forward);

            //if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, maxRange, layerMask))
            //{
            //    //Debug.Log(hit.collider.gameObject.name);

            //    EnemyLimb limb = hit.collider.gameObject.GetComponent<EnemyLimb>();
            //    if (limb != null) {
            //        limb.Hurt(damage, hit);
            //    }
            //}

            currentAmmo--;
            audioSrc.PlayOneShot(fireSound);
            animator.Play(fireAnimation, -1, 0f);
        }
    }

    protected void ShootBullet(Vector3 position, Vector3 direction) {
        if (Physics.Raycast(position, direction, out RaycastHit hit, maxRange, layerMask)) {
            //Debug.Log(hit.collider.gameObject.name);

            GameObject obj = hit.collider.gameObject;

            if (obj.layer == 7) {
                EnemyLimb limb = obj.GetComponent<EnemyLimb>();
                if (limb != null) {
                    limb.Hurt(damage, hit);
                }
            }

            if (obj.layer == 14) {
                Instantiate(wallHitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }

    private void Reload()
    {
        StartCoroutine(ReloadRoutine());
    }

    private IEnumerator ReloadRoutine()
    {
        //animator.Play(reloadAnimation);
        yield return new WaitForSeconds(reloadTime);
        if(reserveAmmo < maxAmmo)
        {
            currentAmmo = reserveAmmo;
            reserveAmmo = 0;
        } else
        {
            currentAmmo = maxAmmo;
            reserveAmmo -= maxAmmo;
        }
    }

    protected IEnumerator FireRateRoutine()
    {
        canFire = false;
        yield return new WaitForSeconds(fireRateDelay);
        canFire = true;
    }
}
