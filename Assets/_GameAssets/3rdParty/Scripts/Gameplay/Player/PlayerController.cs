using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public event Action OnJump;
    
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
    [SerializeField] private float m_airMultiplier = 1.5f;

    [Header("Sliding Settings")]
    [SerializeField] private KeyCode m_slideKey;
    [SerializeField] private float m_slideMultiplier;
    [SerializeField] private bool m_isSliding;
    [SerializeField] private float m_slideDrag = 1.37f;
    [SerializeField] private float m_groundDrag = 8f;
    [SerializeField] private float m_airDrag = 3f;

    // Components & State
    private Rigidbody m_rigidbody;
    private float m_verticalInput;
    private float m_horizontalInput;
    private Vector3 m_moveDirection;
    private StateController m_stateController;


    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_stateController = GetComponent<StateController>();
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
        SetStates();
        LimitPlayerSpeed();

        // Debugging
        Debug.DrawRay(transform.position, Vector3.down * (m_playerHeight * 0.5f + 0.2f), IsGrounded() ? Color.green : Color.red);
        
    }

    private void FixedUpdate()
    {
        SetPlayerMovement();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void SetInputs()
    {
        m_verticalInput = Input.GetAxis("Vertical");
        m_horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(m_slideKey))
        {
            m_isSliding = true;
        }
        else if (Input.GetKeyDown(m_moveKey))
        {
            m_isSliding = false;
        }
        else if (Input.GetKey(m_jumpKey) && IsGrounded() && m_canJump)
        {
            m_canJump = false;
            SetPlayerJumping();
            Invoke(nameof(ResetJumping), m_jumpCooldown);
        }
    }

    private void SetStates()
    {
        var isGrounded = IsGrounded();
        var movementDirection = MovementDirection();
        var isSliding = IsSliding();
        var currentState = m_stateController.GetCurrentState();

        // Öncelik sırası: Havada > Kayıyor > Normal hareket
        var newState = currentState switch
        {
            _ when movementDirection == Vector3.zero && isGrounded && !isSliding => PlayerState.Idle,
            _ when movementDirection != Vector3.zero && isGrounded && !isSliding => PlayerState.Move,
            _ when movementDirection != Vector3.zero && isGrounded && isSliding => PlayerState.Slide,
            _ when movementDirection == Vector3.zero && isGrounded && isSliding => PlayerState.SlideIdle,
            _ when !m_canJump && !isGrounded => PlayerState.Jump,
            _ => currentState
        };

        // State değiştiyse güncelle
        if (newState != currentState)
        {
            m_stateController.ChangeState(newState);
        }
        
    }

    private void SetPlayerMovement()
    {
        m_moveDirection = m_orientTransform.forward * m_verticalInput + m_orientTransform.right * m_horizontalInput;

        float forceMultiplier = m_stateController.GetCurrentState() switch
        {
            PlayerState.Move => 1f,
            PlayerState.Slide => m_slideMultiplier,
            PlayerState.Jump => m_airMultiplier,
            _ => m_rigidbody.linearDamping
        };
        
        m_rigidbody.AddForce(m_moveDirection.normalized * (m_movementSpeed * forceMultiplier), ForceMode.Force);
    }

    private void SetPlayerDrag()
    {
        m_rigidbody.linearDamping = m_stateController.GetCurrentState() switch
        {
            PlayerState.Move => m_groundDrag,
            PlayerState.Slide => m_slideDrag,
            PlayerState.Jump => m_airDrag,
            _ => m_groundDrag
        };
    }

    private void LimitPlayerSpeed()
    {
        Vector3 flatVelocity = new Vector3(m_rigidbody.linearVelocity.x, 0, m_rigidbody.linearVelocity.z);
        if (flatVelocity.magnitude > m_movementSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * m_movementSpeed;
            m_rigidbody.linearVelocity = new Vector3(limitedVelocity.x, m_rigidbody.linearVelocity.y, limitedVelocity.z);
        }
    }

    private void SetPlayerJumping()
    {
        OnJump?.Invoke();
        m_rigidbody.linearVelocity = new Vector3(m_rigidbody.linearVelocity.x, 0f, m_rigidbody.linearVelocity.z);
        m_rigidbody.AddForce(transform.up * m_jumpForce, ForceMode.Impulse);
    }

    private void ResetJumping()
    {
        m_canJump = true;
    }

    private Vector3 MovementDirection()
    {
        return m_moveDirection.normalized;
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, (m_playerHeight * 0.5f) + 0.2f, m_groundLayer);
    }

    private bool IsSliding()
    {
        return m_isSliding;
    }
}
