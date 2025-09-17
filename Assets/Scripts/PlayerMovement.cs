using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    private float jumpforce = 300f;
    private float movingspeed = 1f;
    private Rigidbody rb;
    private bool isGrounded = false;
    public CameraController cameraController;
    private Quaternion lastRotation;
    private Animator animator;

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

        animator = GetComponent<Animator>();
    }

    // Détection des collisions avec le sol
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("IsGrounded", true);
        }
    }



    void Update()
    {
        // --- Gestion du saut ---
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpforce);
            isGrounded = false;
            animator.SetBool("IsGrounded", false);
            animator.SetTrigger("Jump");
           
            Debug.Log("Espace pressé -> Saut !");
        }

        // --- Déplacement ---
        float moveX = 0f;
        float moveZ = 0f;

        if (Input.GetKey(KeyCode.W)) moveZ = 1f;
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
            }
            else if (Input.GetMouseButton(1))
            {
                Vector3 stableForward = lastRotation * Vector3.forward;
                Vector3 stableRight = lastRotation * Vector3.right;

                Vector3 moveDirectionStable = stableForward * Movement.z + stableRight * Movement.x;
                moveDirectionStable.Normalize();

                rb.MovePosition(rb.position + moveDirectionStable * movingspeed * Time.deltaTime);
            }
            else
            {
                rb.MovePosition(rb.position + moveDirection * movingspeed * Time.deltaTime);
                transform.rotation = Quaternion.LookRotation(moveDirection);
            }
        }

        // --- Animation : update des vitesses ---
        animator.SetFloat("xVelocity", rb.linearVelocity.x);
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        Debug.Log($"{rb.linearVelocity.y}");
    }
}
