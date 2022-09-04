using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSphereDebugger : MonoBehaviour
{
    [SerializeField, Tooltip("Damage to apply upon entering THE SPHERE")]
    float damageOnEnter = 1000f;

    void OnTriggerEnter(Collider collider)
    {
        //Debug.Log("Entered");
        if (collider.gameObject.CompareTag("Player"))
        {
            collider.gameObject.SendMessage("ApplyDamage", damageOnEnter, SendMessageOptions.DontRequireReceiver);
        }
    }
}
