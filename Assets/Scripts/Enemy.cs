using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveForce;
    private GameObject player;
    private Rigidbody enemyRb;
    private Vector3 direction;

    void Start()
    {
        player = GameObject.Find("Player");
        enemyRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Get a vector that points in the direction of the player's position
        direction = (player.transform.position - transform.position).normalized;

        if(transform.position.y < -10f) Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        //Move the enemy towards the player
        enemyRb.AddForce(direction * moveForce);
    }
}
