using UnityEngine;
using UnityEngine.InputSystem;

public class AimPlayerScript : MonoBehaviour
{

    public GameObject PlayerModel;

    [SerializeField]
    Vector3 hitpoint;

    Camera mainCamera;

    Ray mouseRayCast;

    [SerializeField]
    Quaternion rotation;

    float height;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        rotation = PlayerModel.transform.rotation;
        height = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        mouseRayCast = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if(Physics.Raycast(mouseRayCast, out var hitInfo, 100f))
        {
            hitpoint = hitInfo.point;
        }

        var target = new Vector3(hitpoint.x, height, hitpoint.z);

        PlayerModel.transform.LookAt(target);
    }
}
