using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeDuration = 2f;
    public int damage = 5;

    private float lifeTime;
    private bool shotByPlayer;
    public bool ShotByPlayer { get { return shotByPlayer; } set { shotByPlayer = value; } }

    // Start is called before the first frame update
    void OnEnable()
    {
        lifeTime = lifeDuration;
    }

    // Update is called once per frame
    void Update()
    {
        // Make the bullet move
        transform.position += transform.forward * speed * Time.deltaTime;

        // Check if bullet should be destroyed
        lifeTime -= Time.deltaTime;
        if(lifeTime <= 0f)
        {
            gameObject.SetActive(false); 
        }
    }
}
