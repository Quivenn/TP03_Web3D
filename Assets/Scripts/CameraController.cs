using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private Camera _camera;
    [SerializeField] private PlayerMovement _playermouvement;
    public Transform player;


    public float distance = 5f;       // Distance initiale
    public float minDistance = 2f;    // Zoom min
    public float maxDistance = 10f;   // Zoom max

    public float xSpeed = 480f; // Sensibilité horizontale
    public float ySpeed = 320f;  // Sensibilité verticale

    public float yMinLimit = -20f; // Angle vertical minimum
    public float yMaxLimit = 60f;  // Angle vertical maximum

    private float x = 0f;
    private float y = 0f;

    private void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

    }

    private void LateUpdate()
    {
        if (player == null) return;

        // Zoom avec molette
        distance -= Input.GetAxis("Mouse ScrollWheel") * 2f;
        distance = Mathf.Clamp(distance, minDistance, maxDistance); //Bloquer une valeur entre minDistance et maxDistance

        // Rotation avec souris
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
            y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;
            y = Mathf.Clamp(y, yMinLimit, yMaxLimit);
        }

        Quaternion rotation = Quaternion.Euler(y, x, 0);


        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + player.position;

        transform.rotation = rotation;
        transform.position = position;

    }


}
