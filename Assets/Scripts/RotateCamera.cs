using UnityEngine;

public class RotateCamera : MonoBehaviour
{

    [Header("Speed")]
    [SerializeField] float sensitivity = 300f;

    private float angleX;
    private float angleY;
    private Transform pivot;
    private Vector3 pivotEulers;
    private void Awake()
    {
        pivot = transform.GetChild(0);
        pivotEulers = pivot.rotation.eulerAngles;
    }
    void Update()
    {
        CameraControl();
    }

    void CameraControl()
    {
        if (Input.GetMouseButton(0))
        {

            float deltaRotX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity;
            float deltaRotY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity;

            angleX += deltaRotX;
            angleY -= deltaRotY;

            angleX = Mathf.Clamp(angleX, -45, 45);
            angleY = Mathf.Clamp(angleY, -45, 45);

            transform.localRotation = Quaternion.Euler(0, angleX, 0);
            pivot.localRotation = Quaternion.Euler(angleY, pivotEulers.y, pivotEulers.z);

           
        }
    }

}