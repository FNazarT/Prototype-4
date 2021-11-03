using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{    
    public GameObject powerupIndicator;
    public GameObject rocketPrefab;
    private float verticalInput, moveForce = 5f, pushBackForce = 15f, jumpForce = 12f, explosionForce = 40f, explosionRadius = 10f;
    private GameObject focalPoint;
    private GameObject tmpRocket;
    private Rigidbody enemyRb, playerRb;
    private Vector3 awayFromPlayerDirection;
    private PowerUpType currentPowerUpType;
    private Coroutine powerUpCountdown;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    void Update()
    {
        //Take Input for moving player
        verticalInput = Input.GetAxis("Vertical");
        //Set Powerup Indicator position at player's feet
        powerupIndicator.transform.position = transform.position + new Vector3(0f, -0.5f, 0f);

        //Shoot rockets if Space key was pressed and the rocket powerup was taken
        if(Input.GetKeyDown(KeyCode.Space) && currentPowerUpType == PowerUpType.Rockets)
        {
            LaunchRockets();
        }

        //Make player jump if B Key was pressed and the smash powerup was taken
        if (Input.GetKeyDown(KeyCode.B) && currentPowerUpType == PowerUpType.Smash)
        {
            playerRb.velocity = Vector3.up * jumpForce;
        }

        //Make player fall faster while in the air if the Smash Powerup is activated
        if (playerRb.velocity.y < 0 && currentPowerUpType == PowerUpType.Smash)
        {
            playerRb.velocity += Physics.gravity * jumpForce * 2 * Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        //Move player
        playerRb.AddForce(focalPoint.transform.forward * verticalInput * moveForce);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //If player collides with an enemy and has the Pushback Powerup, make the enemy fly away from the player
        if (collision.gameObject.CompareTag("Enemy") && currentPowerUpType == PowerUpType.Pushback)
        {
            enemyRb = collision.gameObject.GetComponent<Rigidbody>();

            awayFromPlayerDirection = collision.transform.position - transform.position;

            enemyRb.AddForce(awayFromPlayerDirection * pushBackForce, ForceMode.Impulse);
        }

        //When the player collides with the ground and has the smash Powerup, apply a explision force to enemies near the player
        if (collision.gameObject.CompareTag("Ground") && currentPowerUpType == PowerUpType.Smash)
        {
            foreach(var enemy in FindObjectsOfType<Enemy>())
            {
                enemy.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, 0f, ForceMode.Impulse);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            currentPowerUpType = other.gameObject.GetComponent<PowerUp>().powerUpType;
            powerupIndicator.SetActive(true);
            Destroy(other.gameObject);
            if(powerUpCountdown != null)
            {
                StopCoroutine(powerUpCountdown);
            }
            powerUpCountdown = StartCoroutine(nameof(PowerupCountdownRoutine));
        }
    }

    IEnumerator PowerupCountdownRoutine() 
    {
        yield return new WaitForSeconds(7f);
        currentPowerUpType = PowerUpType.None;
        powerupIndicator.SetActive(false);
    }

    private void LaunchRockets()
    {
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            tmpRocket = Instantiate(rocketPrefab, transform.position + Vector3.up, rocketPrefab.transform.rotation);
            tmpRocket.GetComponent<RocketBehaviour>().Fire(enemy.transform);
        }
    }
}
