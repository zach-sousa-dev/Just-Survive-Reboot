using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [field: SerializeField] private string name { get; set; }

    [field: Header("Rates and Timings")]
    [field: SerializeField] private float fireRateDelay { get; set; }
    [field: SerializeField] private float reloadTime { get; set; }

    [field: Header("Ammo")]
    [field: SerializeField] private int maxAmmo { get; set; }
    [field: SerializeField] private int dryAmmoThreshold { get; set; }
    [field: SerializeField] private int currentAmmo { get; set; }
    [field: SerializeField] private int reserveAmmo { get; set; }

    [field: Header("Handling")]
    [field: SerializeField] private float accuracy { get; set; }
    [field: SerializeField] private float recoil { get; set; }

    [field: Header("Capability")]
    [field: SerializeField] private float maxRange { get; set; }
    [field: SerializeField] private float damage { get; set; }

    [field: Header("Object References")]
    [field: SerializeField] private Camera cam { get; set; }
    [field: SerializeField] private LayerMask layerMask { get; set; }
    [field: SerializeField] private GameObject weaponModel { get; set; }
    [field: SerializeField] private Animator animator { get; set; }
    [field: SerializeField] private AudioSource audioSrc { get; set; }

    [field: Header("Animation States")]
    [field: SerializeField] private string fireAnimation { get; set; }
    [field: SerializeField] private string emptyAnimation { get; set; }
    [field: SerializeField] private string idleAnimation { get; set; }
    [field: SerializeField] private string reloadAnimation { get; set; }
    [field: SerializeField] private string equipAnimation { get; set; }

    [field: Header("Sounds")]
    [field: SerializeField] private AudioClip fireSound { get; set; }
    [field: SerializeField] private AudioClip noAmmoSound { get; set; }
    [field: SerializeField] private AudioClip dryFireSound { get; set; }
    [field: SerializeField] private AudioClip reloadSound { get; set; }
    [field: SerializeField] private AudioClip equipSound { get; set; }

    //  States
    [SerializeField] private bool canFire = true;
    [SerializeField] private bool canReload = true;
    [SerializeField] private bool isReloading = false;
    [SerializeField] private bool bufferedShot = false;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) || bufferedShot)
        {
            RequestShoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
        Debug.DrawRay(cam.transform.position, cam.transform.forward, Color.green);
    }

    private void RequestShoot()
    {
        if(currentAmmo > 0 && canFire && !bufferedShot)
        {
            Shoot();
        } 
        else if(currentAmmo > 0 && !canFire && !isReloading)
        {
            bufferedShot = true;
        }
        else if(bufferedShot && canFire && !isReloading && currentAmmo > 0)
        {
            Shoot();
            bufferedShot = false;
        } else if(currentAmmo < 1)
        {
            audioSrc.PlayOneShot(dryFireSound);
        }
    }

    private void Shoot()
    {
        StartCoroutine(FireRateRoutine());

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, maxRange, layerMask))
        {
            Debug.Log(hit.collider.gameObject.name);
        }

        currentAmmo--;
        audioSrc.PlayOneShot(fireSound);
        animator.Play(fireAnimation, -1, 0f);
    }

    private void Reload()
    {
        if(currentAmmo < maxAmmo && canReload)
        {
            StartCoroutine(ReloadRoutine());
            audioSrc.PlayOneShot(reloadSound);
            animator.Play(reloadAnimation, -1, 0f);
        }
    }

    private IEnumerator ReloadRoutine()
    {
        canFire = false;
        canReload = false;
        isReloading = true;
        //animator.Play(reloadAnimation);
        yield return new WaitForSeconds(reloadTime);
        canFire = true;
        canReload = true;
        isReloading = false;
        if (reserveAmmo < maxAmmo)
        {
            currentAmmo = reserveAmmo;
            reserveAmmo = 0;
        } else
        {
            currentAmmo = maxAmmo;
            reserveAmmo -= maxAmmo;
        }
    }

    private IEnumerator FireRateRoutine()
    {
        canFire = false;
        canReload = false;
        yield return new WaitForSeconds(fireRateDelay);
        canFire = true;
        canReload = true;
    }
}
