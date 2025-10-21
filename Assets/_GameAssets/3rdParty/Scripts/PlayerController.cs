using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform m_orientTransform;

    [Header("Movement Speed")]
    [SerializeField] private KeyCode m_moveKey;
    [SerializeField] private float m_movementSpeed;

    [Header("Jump Settings")]
    [SerializeField] private KeyCode m_jumpKey;
    [SerializeField] private float m_jumpForce;
    [SerializeField] private float m_playerHeight;
    [SerializeField] private bool m_canJump = true;
    [SerializeField] private LayerMask m_groundLayer;
    [SerializeField] private float m_jumpCooldown = 0.2f;

    [Header("Sliding Settings")]
    [SerializeField] private KeyCode m_slideKey;
    [SerializeField] private float m_slideMultiplier;
    [SerializeField] private bool m_isSliding;
    [SerializeField] private float m_dampingValue = 1.37f;
    [SerializeField] private float m_groundDrag = 8f;

    // Components & State
    private Rigidbody m_rigidbody;
    private float m_verticalInput;
    private float m_horizontalInput;
    private Vector3 m_moveDirection;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        if (m_rigidbody == null)
        {
            Debug.LogError("PlayerController: Rigidbody not found!");
            enabled = false;
            return;
        }

        m_rigidbody.freezeRotation = true;
    }

    private void Update()
    {
        SetInputs();
        SetPlayerDrag();
        LimitPlayerSpeed();

        // Debugging
        Debug.DrawRay(transform.position, Vector3.down * (m_playerHeight * 0.5f + 0.2f), Color.red);

        if (m_rigidbody != null)
        {
            Debug.Log("Speed: " + m_rigidbody.linearVelocity.magnitude);
        }
    }

    private void FixedUpdate()
    {
        SetPlayerMovement();
    }

    private void SetInputs()
    {
        m_verticalInput = Input.GetAxis("Vertical");
        m_horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(m_slideKey))
        {
            m_isSliding = true;
            Debug.Log("Slide");
        }
        else if (Input.GetKeyDown(m_moveKey))
        {
            m_isSliding = false;
            Debug.Log("Not Sliding");
        }
        else if (Input.GetKey(m_jumpKey) && IsGrounded() && m_canJump)
        {
            m_canJump = false;
            SetPlayerJumping();
            Invoke(nameof(ResetJumping), m_jumpCooldown);
        }
    }

    private void SetPlayerMovement()
    {
        if (m_orientTransform == null || m_rigidbody == null)
            return;

        m_moveDirection = m_orientTransform.forward * m_verticalInput + m_orientTransform.right * m_horizontalInput;

        float currentSpeed = m_isSliding ? m_movementSpeed * m_slideMultiplier : m_movementSpeed;
        m_rigidbody.AddForce(m_moveDirection.normalized * currentSpeed, ForceMode.Force);
    }

    private void SetPlayerDrag()
    {
        if (m_rigidbody == null)
            return;

        m_rigidbody.linearDamping = m_isSliding ? m_dampingValue : m_groundDrag;
    }

    private void LimitPlayerSpeed()
    {
        if (m_rigidbody == null)
            return;

        Vector3 flatVelocity = new Vector3(m_rigidbody.linearVelocity.x, 0, m_rigidbody.linearVelocity.z);
        if (flatVelocity.magnitude > m_movementSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * m_movementSpeed;
            m_rigidbody.linearVelocity = new Vector3(limitedVelocity.x, m_rigidbody.linearVelocity.y, limitedVelocity.z);
        }
    }

    private void SetPlayerJumping()
    {
        if (m_rigidbody == null)
            return;

        m_rigidbody.linearVelocity = new Vector3(m_rigidbody.linearVelocity.x, 0, m_rigidbody.linearVelocity.z);
        m_rigidbody.AddForce(transform.up * m_jumpForce, ForceMode.Impulse);
    }

    private void ResetJumping()
    {
        m_canJump = true;
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, m_playerHeight * 0.5f + 0.2f, m_groundLayer);
    }
}
