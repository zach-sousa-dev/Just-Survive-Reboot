using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor.SceneManagement;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class EnemyLimb : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private bool vital;
    private Enemy parent;

    [SerializeField] private GameObject hitEffect;
    [SerializeField] private GameObject destructionEffect;

    void Start() {
        health = maxHealth;
        parent = GetComponentInParent<Enemy>();
    }

    public void Hurt(float damage, RaycastHit hit) {
        health -= damage;

        if (health <= 0f) {
            //die
            DestructionEffect(parent);

            if (vital) {
                
                parent.ApplyModifiers(death: true);
            }
            Destroy(gameObject);
            return;
        }

        SpawnSplatter(hitEffect, hit.point, Quaternion.Euler(hit.normal));

        DamageEffect(parent);
    }

    protected virtual void DestructionEffect(Enemy parent) {
        
    }

    protected virtual void DamageEffect(Enemy parent) {
        
    }

    private void SpawnSplatter(GameObject splatterEffect, Vector3 position, Quaternion direction, Transform parent) {
        GameObject splatter = Instantiate(splatterEffect, position + (direction.eulerAngles.normalized * 0.2f), direction);
        splatter.transform.SetParent(parent);
    }

    private void SpawnSplatter(GameObject splatterEffect, Vector3 position, Quaternion direction) {
        SpawnSplatter(splatterEffect, position, direction, transform);
    }

    private void OnDestroy() {
        SpawnSplatter(destructionEffect, transform.position, Quaternion.Euler(transform.forward), null);
        List<GameObject> particles = Utilities.FindChildrenWithTag(gameObject, "Particle");
        foreach (GameObject particle in particles) {
            particle.transform.SetParent(null, true);
            particle.GetComponent<Annihilate>().enabled = true;
        }
    }
}
