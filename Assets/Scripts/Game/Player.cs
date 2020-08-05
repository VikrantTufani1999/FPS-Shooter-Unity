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
            if (ammo > 0)
            {
                ammo--;

                GameObject bulletObject = ObjectPoolingManager.Instance.GetBullet();        // Instantiate bullet from object pool
                bulletObject.transform.position = playerCamera.transform.position + playerCamera.transform.forward;     // bullets position setting
                bulletObject.transform.forward = playerCamera.transform.forward;            // Move bullets forward
            }
        }
    }

    // Check for collision

    private void OnControllerColliderHit(ControllerColliderHit hit)         // This collision inbuilt function only works for FPS type main players
    {
        if(hit.collider.gameObject.GetComponent<AmmoCrate>() != null)       // Check if hit with ammoCrate
        {
            //Debug.Log(hit.collider.name);       // Get name of gameObject with which the player is hit.
            
            // Collect ammo
            AmmoCrate ammoCrate = hit.collider.gameObject.GetComponent<AmmoCrate>();    // Reference to AmmoCrate Script
            ammo += ammoCrate.ammo;         // Add ammoCrate bullets to existing ammo amount
            Destroy(ammoCrate.gameObject);  // Destroy the ammoCrate 
        }

        else if(hit.collider.gameObject.GetComponent<Enemy>() != null)          // Check if hit with enemy
        {
            if (isHurt == false)     // Toggle to check if hurt by enemy or not                                           
            {
                Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();        // Reference to Enemy Script
                health -= enemy.damage;                                             // Health decrease | Take damage from enemy

                isHurt = true;                                                      // Toggle bool to true if hurt by enemy

                // Perform the Knockback effect
                Vector3 hurtDirection = (transform.position - enemy.transform.position).normalized;
                Vector3 knockBackDirection = (hurtDirection + Vector3.up).normalized;

                GetComponent<Rigidbody>(    )

                StartCoroutine("HurtRoutine");                                      // Start sequence of can't be hurt more than one time in one hit
            }
        }
    }

    IEnumerator HurtRoutine()               // Coroutine setup
    {
        yield return new WaitForSeconds(hurtDuration);

        isHurt = false;
    }
}
