using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public float initialVelocity = 3000;

    public new Rigidbody rigidbody;
    public ParticleSystem explosion;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Awake()
    {
        rigidbody.AddForce(transform.forward * initialVelocity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer != 6)
        {
            var exp = Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        // New damage application method
        collision.gameObject.SendMessage("ApplyDamage", 25, SendMessageOptions.DontRequireReceiver);
    }

}
