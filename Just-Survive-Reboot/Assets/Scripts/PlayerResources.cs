using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Rendering.PostProcessing;

//handles resources like health and ammo
public class PlayerResources : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;


    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Hurt(float damage)
    {
        health -= damage;

        Debug.Log(health);

        if (health <= 0)
        {
            //die
            Debug.Log("Dead");
        }
        else if (health <= maxHealth)
        {
            //modify UI and maybe vignette
        }


    }
}
