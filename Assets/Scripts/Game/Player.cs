using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Declarations
    [Header("Visuals")]
    public Camera playerCamera;

    [Header("Gameplay")]
    public int initialHealth = 100;
    public int initialAmmo = 12;
    public float knockBackForce = 10f;
    public float hurtDuration = 0.5f;

    private int health;                 
    public int Health { get { return health; } }            // Property Declaration

    private int ammo;                                       
    public int Ammo { get { return ammo; } }                // Property Declaration

    private bool killed;
    public bool Killed { get { return killed; } }
    private bool isHurt;

    void Start()
    {
        // initialization of variables
        health = initialHealth;                                 
        ammo = initialAmmo;    
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0))         // Check for Left Mouse Click
        {
            // Fire Bullets
            if (ammo > 0 && Killed == false)
            {
                ammo--;

                GameObject bulletObject = ObjectPoolingManager.Instance.GetBullet(true);        // Instantiate bullet from object pool
                bulletObject.transform.position = playerCamera.transform.position + playerCamera.transform.forward;     // bullets position setting
                bulletObject.transform.forward = playerCamera.transform.forward;            // Move bullets forward
            }
        }
    }

    // Check for collision

    private void OnTriggerEnter(Collider otherCollider)         // This collision inbuilt function only works for FPS type main players
    {
        if(otherCollider.gameObject.GetComponent<AmmoCrate>() != null)       // Check if hit with ammoCrate
        {
            //Debug.Log(hit.collider.name);       // Get name of gameObject with which the player is hit.
            
            // Collect ammo crates
            AmmoCrate ammoCrate = otherCollider.gameObject.GetComponent<AmmoCrate>();    // Reference to AmmoCrate Script
            ammo += ammoCrate.ammo;         // Add ammoCrate bullets to existing ammo amount

            Destroy(ammoCrate.gameObject);  // Destroy the ammoCrate 
        }
        else if (otherCollider.gameObject.GetComponent<HealthCrate>() != null)       // Check if hit with ammoCrate
        {
            //Debug.Log(hit.collider.name);       // Get name of gameObject with which the player is hit.

            // Collect health crates
            HealthCrate healthCrate = otherCollider.gameObject.GetComponent<HealthCrate>();    // Reference to AmmoCrate Script
            health += healthCrate.health;         // Add health to existing ammo amount

            Destroy(healthCrate.gameObject);  // Destroy the HealthCrate 
        }

        if (isHurt == false)     // Toggle to check if hurt by enemy or not                                           
        {
            GameObject hazard = null;
            if (otherCollider.gameObject.GetComponent<Enemy>() != null)          // Check if hit with enemy
            {
                Enemy enemy = otherCollider.gameObject.GetComponent<Enemy>();        // Reference to Enemy Script
                if (enemy.Killed == false)
                {
                    hazard = enemy.gameObject;
                    health -= enemy.damage;                                             // Health decrease | Take damage from enemy
                }
                isHurt = true;                                                      // Toggle bool to true if hurt by enemy
                                      // Start sequence of can't be hurt more than one time in one hit
            }

            else if(otherCollider.GetComponent<Bullet>() != null)
            {
                Bullet bullet = otherCollider.GetComponent<Bullet>();
                if(bullet.ShotByPlayer == false)
                {
                    hazard = bullet.gameObject;
                    health -= bullet.damage;
                    bullet.gameObject.SetActive(false);
                }
            }

            if(hazard != null)      // Optimization tip
            {
                isHurt = true;

                // Perform the Knockback effect
                Vector3 hurtDirection = (transform.position - hazard.transform.position).normalized;
                Vector3 knockBackDirection = (hurtDirection + Vector3.up).normalized;

                GetComponent<ForceReceiver>().AddForce(knockBackDirection, knockBackForce);

                StartCoroutine("HurtRoutine");
            }

            if(health <= 0)
            {
                if(killed == false)
                {
                    killed = true;

                    OnKill();
                }
            }
        }
    }

    IEnumerator HurtRoutine()               // Coroutine setup
    {
        yield return new WaitForSeconds(hurtDuration);

        isHurt = false;
    }

    private void OnKill()
    {
        GetComponent<CharacterController>().enabled = false;
        GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
    }
}
