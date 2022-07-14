using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AimTurrentScript : MonoBehaviour
{

    public GameObject Turret;

    new Camera camera;

    [SerializeField]
    Vector3 hitpoint;

    Ray mouseRayCast;

    [SerializeField]
    Quaternion rotation;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        rotation = Turret.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        mouseRayCast = camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if(Physics.Raycast(mouseRayCast, out var hitInfo, 100f))
        {
            hitpoint = hitInfo.point;
        }

        var target = new Vector3(hitpoint.x, hitpoint.y, hitpoint.z);

        Turret.transform.LookAt(target);
        //Turret.transform.Rotate(new Vector3(-90f, 0, 180f));
    }
}
