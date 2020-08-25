using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootingEnemy : Enemy
{
    public AudioSource deathSound;
    public float shootingInterval = 5f;
    public float shootingDistance = 10f;
    public float chasingInterval = 2f;
    public float chasingDistance = 12f;

    private Player player;
    private float shootingTimer;
    private float chasingTimer;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        agent = GetComponent<NavMeshAgent>();
        shootingTimer = Random.Range(0, shootingInterval);    
    }

    // Update is called once per frame
    void Update()
    {
        if(player.Killed == true)
        {
            agent.enabled = false;
            this.enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;

        }

        // Shooting Logic
        shootingTimer -= Time.deltaTime;

        if (shootingTimer <= 0 && Vector3.Distance(transform.position, player.transform.position) <= shootingDistance)
        {
            shootingTimer = shootingInterval;

            GameObject bulletPrefab = ObjectPoolingManager.Instance.GetBullet(false);
            bulletPrefab.transform.position = transform.position;
            bulletPrefab.transform.forward = (player.transform.position - transform.position).normalized;

            
        }

        chasingTimer -= Time.deltaTime;
        if(chasingTimer <= 0 && Vector3.Distance(transform.position, player.transform.position) <= chasingDistance)
        {
            chasingTimer = chasingInterval;
            agent.SetDestination(player.transform.position);
        }
    }

    protected override void OnKill()
    {
        base.OnKill();

        deathSound.Play();

        agent.enabled = false;
        this.enabled = false;

        transform.localEulerAngles = new Vector3(10, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

}

