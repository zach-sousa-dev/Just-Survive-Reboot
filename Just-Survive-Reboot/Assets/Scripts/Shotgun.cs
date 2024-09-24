using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shotgun : Weapon { 
    [field: Header("Shotgun")]
    [field: SerializeField] protected float pelletCount { get; set; }

    private void Awake() {
        accuracy = Utilities.map(accuracy, 0, 360, 0, 1);
        accuracy /= 2;
    }

    protected override void Shoot()
    {
        if (currentAmmo > 0 && canFire) {
            StartCoroutine(FireRateRoutine());

            //1 pellet is always accurate
            ShootBullet(cam.transform.position, cam.transform.forward);

            for (int i = 0; i < pelletCount-1; i++) {
                Vector3 direction = cam.transform.forward;

                //Debug.Log(Quaternion.AngleAxis(Random.Range(-accuracy / 2, accuracy / 2), Vector3.up));

                //direction.x *= Quaternion.AngleAxis(Random.Range(-accuracy/2, accuracy/2), Vector3.up).eulerAngles.x;

                direction += new Vector3(Random.Range(-accuracy, accuracy), Random.Range(-accuracy, accuracy), Random.Range(-accuracy, accuracy));

                ShootBullet(cam.transform.position, direction);

                Debug.DrawRay(cam.transform.position, direction * 5, Color.blue, 1);
            }

            

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
}
