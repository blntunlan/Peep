using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action OnPlayerJumped;
    public event Action<PlayerState> OnPlayerStateChanged;
    
    [Header("References")]
    [SerializeField] private Transform m_orientTransform;

    [Header("Movement Speed")]
    [SerializeField] private KeyCode m_moveKey = KeyCode.LeftShift;
    [SerializeField] private float m_movementSpeed = 10f;
    
    [Header("Jump Settings")]
    [SerializeField] private KeyCode m_jumpKey = KeyCode.Space;
    [SerializeField] private float m_jumpForce = 5f;
    [SerializeField] private float m_playerHeight = 2f;
    [SerializeField] private bool m_canJump = true;
    [SerializeField] private LayerMask m_groundLayer;
    [SerializeField] private float m_jumpCooldown = 0.2f;
    [SerializeField] private float m_airMultiplier = 0.4f; // 1.5'ten 0.4'e düşürüldü

    [Header("Sliding Settings")]
    [SerializeField] private KeyCode m_slideKey = KeyCode.LeftControl;
    [SerializeField] private float m_slideMultiplier = 1.5f;
    [SerializeField] private bool m_isSliding;
    [SerializeField] private float m_slideDrag = 1.37f;
    [SerializeField] private float m_groundDrag = 8f;
    [SerializeField] private float m_airDrag = 3f;

    // Components & State
    private float _startingMovementSpeed, _startingJumpForce;
    private Rigidbody m_rigidbody;
    private float m_verticalInput;
    private float m_horizontalInput;
    private Vector3 m_moveDirection;
    private StateController m_stateController;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_stateController = GetComponent<StateController>();
        m_rigidbody.freezeRotation = true;
        
        _startingJumpForce = m_jumpForce;
        _startingMovementSpeed = m_movementSpeed;
    }

    private void Update()
    {
        if (GameManager.Instance.GetCurrentGameState() != GameState.Play && GameManager.Instance.GetCurrentGameState() != GameState.Resume)
            return;
        SetInputs();
        SetPlayerDrag();
        SetStates();
        LimitPlayerSpeed();
        
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.GetCurrentGameState() != GameState.Play && GameManager.Instance.GetCurrentGameState() != GameState.Resume)
            return;
        SetPlayerMovement();
    }

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
        else if (Input.GetKeyDown(m_jumpKey) && m_canJump && IsGrounded())
        {
            m_canJump = false;
            SetPlayerJumping();
            Invoke("ResetJumping", m_jumpCooldown);
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void SetStates()
    {
        var isGrounded = IsGrounded();
        var movementDirection = MovementDirection();
        var isSliding = IsSliding();
        var currentState = m_stateController.GetCurrentState();

        var newState = currentState switch
        {
            _ when movementDirection == Vector3.zero && isGrounded && !IsSliding() => PlayerState.Idle,
            _ when movementDirection != Vector3.zero && isGrounded && !IsSliding() => PlayerState.Move,
            _ when movementDirection != Vector3.zero && isGrounded && IsSliding() => PlayerState.Slide,
            _ when movementDirection == Vector3.zero && isGrounded && IsSliding() => PlayerState.SlideIdle,
            _ when !m_canJump && !isGrounded => PlayerState.Jump,
            _ => currentState
        };

        if (newState != currentState)
        {
            m_stateController.ChangeState(newState);
            OnPlayerStateChanged?.Invoke(newState);
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
            _ => 1f
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
            _ => m_rigidbody.linearDamping
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
        // Slide sırasında zıplarsa slide'ı bitir
        m_isSliding = false;
        
        OnPlayerJumped?.Invoke();
        m_rigidbody.linearVelocity = new Vector3(m_rigidbody.linearVelocity.x, 0f, m_rigidbody.linearVelocity.z);
        m_rigidbody.AddForce(transform.up * m_jumpForce, ForceMode.Impulse);
    }



    #region Helper Functions

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, (m_playerHeight * 0.5f) + 0.2f, m_groundLayer);
    }
    private void ResetJumping()
    {
        m_canJump = true;
    }

    private Vector3 MovementDirection()
    {
        return m_moveDirection.normalized;
    }

    private bool IsSliding()
    {
        return m_isSliding;
    }

    public void SetMovementSpeed(float speed, float duration)
    {
        m_movementSpeed += speed;
        Invoke(nameof(ResetMovementSpeed), duration);
    }
    
    private void ResetMovementSpeed()
    {
        m_movementSpeed = _startingMovementSpeed;
    }

    public void SetJumpForce(float force, float duration)
    {
        m_jumpForce += force;
        Invoke(nameof(ResetJumpForce), duration);
    }

    private void ResetJumpForce()
    {
        m_jumpForce = _startingJumpForce;   
    }

    public Rigidbody GetPlayerRigidbody()
    {
        return m_rigidbody;   
    }
    #endregion
 
}