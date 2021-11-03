using UnityEngine;

public class RocketBehaviour : MonoBehaviour
{
    private Transform target;
    private Vector3 direction;
    private float speed = 15f, timer = 5f;


    private void Update()
    {
        if(target != null)
        {
            direction = (target.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    public void Fire(Transform targetPosition)
    {
        target = targetPosition;
        Destroy(gameObject, timer);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (target != null && collision.gameObject.CompareTag(target.tag))
        {
            Rigidbody targetRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromRocketDirection = collision.transform.position - transform.position;
            targetRb.AddForce(awayFromRocketDirection * speed, ForceMode.Impulse);

            Destroy(gameObject);
        }
    }
}
