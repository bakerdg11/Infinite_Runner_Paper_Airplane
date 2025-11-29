using UnityEngine;


public class Missile : MonoBehaviour
{
    [Header("Motion")]
    public float speed = 50f;           // m/s
    public float lifeTime = 5f;         // seconds before auto-destroy
    public float acceleration = 0f;     // optional

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;                    // missiles fly straight
        rb.isKinematic = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

    }

    void OnEnable()
    {
        // initial velocity in the missile's forward direction (Z+ on the prefab)
        if (rb) rb.linearVelocity = transform.forward * speed;

        // auto-cleanup
        if (lifeTime > 0f) Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        // keep pushing forward (optional; keeps speed exact even after minor contacts)
        if (rb)
        {
            if (acceleration != 0f) speed += acceleration * Time.fixedDeltaTime;
            rb.linearVelocity = transform.forward * speed;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            Destroy(other.gameObject); // destroy obstacle
            Destroy(gameObject);       // destroy missile
            SoundManager.Instance.PlayMissileExplosion();
        }
    }

}