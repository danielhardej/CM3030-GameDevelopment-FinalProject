using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour
{

    public GameObject Bullet;

    private GameObject _bulletInstance;

    public Transform SpawnPoint;

    public void OnFire()
    {
        _bulletInstance = Instantiate(Bullet, SpawnPoint.position, SpawnPoint.rotation);
    }
}
