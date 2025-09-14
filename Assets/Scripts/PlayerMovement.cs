using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private float jumpforce = 300f;
    private float movingspeed = 1f;
    private Rigidbody rb;
    private bool isGrounded;
    public CameraController cameraController;
    private Quaternion lastRotation;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (cameraController == null && Camera.main != null)
        {
            cameraController = Camera.main.GetComponent<CameraController>();
        }

        if (cameraController == null)
        {
            Debug.LogError("CameraController non assigné ou introuvable !");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpforce);
            isGrounded = false;
            Debug.Log("Espace pressé -> Saut !");
        }

        float moveX = 0f;
        float moveZ = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            moveZ = 1f; ;
            Debug.Log("Z");
        }


        if (Input.GetKey(KeyCode.S)) moveZ = -1f;

        if (Input.GetKey(KeyCode.D)) moveX = 1f;

        if (Input.GetKey(KeyCode.A)) moveX = -1f;

        Vector3 Movement = new Vector3(moveX, 0, moveZ).normalized * movingspeed;

        if (Movement.magnitude > 0.1f)
        {
            Transform camTransform = cameraController.transform;
            Vector3 camForward = camTransform.forward;
            Vector3 camRight = camTransform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();
            Vector3 moveDirection = camForward * Movement.z + camRight * Movement.x;
            moveDirection.Normalize();
            if (Input.GetMouseButton(0))
            {
                rb.MovePosition(rb.position + moveDirection * movingspeed * Time.deltaTime);
                transform.rotation = Quaternion.LookRotation(moveDirection);
                lastRotation = transform.rotation;
                Debug.Log("CliqueGauche");
            }
            
            else if (Input.GetMouseButton(1))
            {
                Vector3 stableForward = lastRotation * Vector3.forward;
                Vector3 stableRight = lastRotation * Vector3.right;

                Vector3 moveDirectionStable = stableForward * Movement.z + stableRight * Movement.x;
                moveDirectionStable.Normalize();

                // Déplacement selon la dernière orientation, sans la modifier
                rb.MovePosition(rb.position + moveDirectionStable * movingspeed * Time.deltaTime);
                Debug.Log("CliqueDroit");
            }
            else if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1))
            {
                rb.MovePosition(rb.position + moveDirection * movingspeed * Time.deltaTime);
                transform.rotation = Quaternion.LookRotation(moveDirection);
                Debug.Log("Test");
            
            }
        }



    }

}
