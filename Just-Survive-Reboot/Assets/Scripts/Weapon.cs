using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Weapon : MonoBehaviour
{
    [field: SerializeField] private string Name { get; set; }

    [field: Header("Rates and Timings")]
    [field: SerializeField] private float FireRateDelay { get; set; }
    [field: SerializeField] private float ReloadTime { get; set; }

    [field: Header("Ammo")]
    [field: SerializeField] private int MaxAmmo { get; set; }
    [field: SerializeField] private int DryAmmoThreshold { get; set; }
    [field: SerializeField] private int CurrentAmmo { get; set; }
    [field: SerializeField] private int ReserveAmmo { get; set; }

    [field: Header("Handling")]
    [field: SerializeField] private float Accuracy { get; set; }
    [field: SerializeField] private float ADSFOVMultiplier { get; set; }

    [SerializeField] private AnimationCurve RecoilFunction;
    [field: SerializeField] private float RecoilMultiplier { get; set; }
    private float currentRecoilTimeStamp = 0;
    private float maxRecoilTimeStamp;   //  set in Start
    private float minRecoilTimeStamp;   //  set in Start
    private Coroutine recoilRecoveryRoutineInstance;
    [field: SerializeField] private float RecoilRecoveryTime { get; set; }

    [field: Header("Capability")]
    [field: SerializeField] private float MaxRange { get; set; }
    [field: SerializeField] private float Damage { get; set; }
    [field: SerializeField] private bool isFullAuto {  get; set; }

    [field: Header("Object References")]
    [field: SerializeField] private Camera Cam { get; set; }
    [field: SerializeField] private LayerMask LayerMask { get; set; }
    [field: SerializeField] private GameObject WeaponModel { get; set; }
    [field: SerializeField] private Animator Animator { get; set; }
    [field: SerializeField] private AudioSource AudioSrc { get; set; }
    [field: SerializeField] private ParticleSystem MuzzleFlash { get; set; }
    [field: SerializeField] private PlayerController PlayerController { get; set; }
    [field: SerializeField] private Transform TracerStartPosition { get; set; }
    [field: SerializeField] private LineRenderer Tracer { get; set; }
    private FOVController fovController;

    [field: Header("Animation States")]
    [field: SerializeField] private string FireTrigger { get; set; }
    [field: SerializeField] private string ReloadState { get; set; }
    [field: SerializeField] private string ADSState { get; set; }

    [field: Header("Sounds")]
    [field: SerializeField] private AudioClip FireSound { get; set; }
    [field: SerializeField] private AudioClip NoAmmoSound { get; set; }
    [field: SerializeField] private AudioClip DryFireSound { get; set; }
    [field: SerializeField] private AudioClip ReloadSound { get; set; }
    [field: SerializeField] private AudioClip EquipSound { get; set; }
    
    //  States
    private bool canFire = true;
    private bool canReload = true;
    private bool isReloading = false;
    private bool bufferedShot = false;
    private bool isADS = false;

    private void Start()
    {
        minRecoilTimeStamp = RecoilFunction.keys[0].time;
        maxRecoilTimeStamp = RecoilFunction.keys[RecoilFunction.keys.Length-1].time;
        fovController = Cam.GetComponent<FOVController>();
    }

    private void Update()
    {
        isADS = (Input.GetMouseButton(1) && CurrentAmmo > 0);
        Animator.SetBool(ADSState, isADS);
        if(isADS)
        {
            fovController.FOVMultiplier = ADSFOVMultiplier;
        } else
        {
            fovController.FOVMultiplier = 1;
        }

        if(isFullAuto && CurrentAmmo > 0)
        {
            if (Input.GetMouseButton(0))    //  don't check for buffered shot because it will be near impossible to tap fire
            {
                RequestShoot();
            }
        } else
        {
            if (Input.GetMouseButtonDown(0) || bufferedShot)
            {
                RequestShoot();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
        Debug.DrawRay(Cam.transform.position, Cam.transform.forward, Color.green);
    }

    private void RequestShoot()
    {
        if(canFire && !bufferedShot)
        {
            Shoot();
        } 
        else if(CurrentAmmo > 0 && !canFire && !isReloading)
        {
            bufferedShot = true;
        }
        else if(bufferedShot && canFire && !isReloading)
        {
            Shoot();
            bufferedShot = false;
        }
    }

    private void Shoot()
    {
        StartCoroutine(FireRateRoutine());

        if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out RaycastHit hit, MaxRange, LayerMask))
        {
            Debug.Log(hit.collider.gameObject.name);
        }

        if (CurrentAmmo != 0)
        {
            CurrentAmmo--;
            DoRecoil();
            Animator.SetTrigger(FireTrigger);
            MuzzleFlash.Play();
            LineRenderer tracerInstance = Instantiate(Tracer);
            tracerInstance.SetPosition(0, TracerStartPosition.position);
            tracerInstance.SetPosition(1, hit.point);

            if (CurrentAmmo > DryAmmoThreshold)
            {
                AudioSrc.PlayOneShot(FireSound);
            }
            else
            {
                AudioSrc.PlayOneShot(DryFireSound);
            }
        } else
        {
            AudioSrc.PlayOneShot(NoAmmoSound);
        }
        
    }

    private void DoRecoil()
    {
        float recoilIncrement = RecoilFunction.Evaluate(currentRecoilTimeStamp) * RecoilMultiplier;

        if(recoilRecoveryRoutineInstance != null)
        {
            StopCoroutine(recoilRecoveryRoutineInstance);
        }

        PlayerController.RecoilX -= recoilIncrement;

        //Debug.Log("RECOIL TIME STAMP: " + currentRecoilTimeStamp);
        //Debug.Log("RECOIL EVAL: " + recoilIncrement);

        currentRecoilTimeStamp += maxRecoilTimeStamp / MaxAmmo;

        recoilRecoveryRoutineInstance = StartCoroutine(RecoilRecovery(maxRecoilTimeStamp / MaxAmmo));
    }

    private IEnumerator RecoilRecovery(float decrement)
    {
        if (!(currentRecoilTimeStamp <= 0))
        {
            //Debug.Log("entered");
            yield return new WaitForSeconds(RecoilRecoveryTime + FireRateDelay);
            currentRecoilTimeStamp -= decrement;
            //Debug.Log("RECOIL DECREMENTED");
            recoilRecoveryRoutineInstance = StartCoroutine(RecoilRecovery(maxRecoilTimeStamp / MaxAmmo));
        }
    }

    private void Reload()
    {
        if(CurrentAmmo < MaxAmmo && canReload)
        {
            StartCoroutine(ReloadRoutine());
            currentRecoilTimeStamp = minRecoilTimeStamp;
            AudioSrc.PlayOneShot(ReloadSound);
        }
    }


    private IEnumerator ReloadRoutine()
    {
        canFire = false;
        canReload = false;
        isReloading = true;
        Animator.SetBool(ReloadState, true);
        //animator.Play(reloadAnimation);
        yield return new WaitForSeconds(ReloadTime);
        Animator.SetBool(ReloadState, false);
        canFire = true;
        canReload = true;
        isReloading = false;
        if (ReserveAmmo < MaxAmmo)
        {
            CurrentAmmo = ReserveAmmo;
            ReserveAmmo = 0;
        } else
        {
            CurrentAmmo = MaxAmmo;
            ReserveAmmo -= MaxAmmo;
        }
    }

    private IEnumerator FireRateRoutine()
    {
        canFire = false;
        canReload = false;
        yield return new WaitForSeconds(FireRateDelay);
        canFire = true;
        canReload = true;
    }
}
