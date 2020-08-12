using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : Enemy
{
    public float shootingInterval = 5f;
    public float shootingDistance = 10f;

    private Player player;
    private float shootingTimer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        shootingTimer = Random.Range(0, shootingInterval);    
    }

    // Update is called once per frame
    void Update()
    {
        shootingTimer -= Time.deltaTime;

        if (shootingTimer <= 0 && Vector3.Distance(transform.position, player.transform.position) <= shootingDistance)
        {
            shootingTimer = shootingInterval;

            GameObject bulletPrefab = ObjectPoolingManager.Instance.GetBullet(false);
            bulletPrefab.transform.position = transform.position;
            bulletPrefab.transform.forward = (player.transform.position - transform.position).normalized;
        }
    }
}
