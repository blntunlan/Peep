using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    [SerializeField] private Transform m_playerTransform;
    [SerializeField] private Transform m_orientationTransform;
    [SerializeField] private Transform m_playerVisualTransform;
    [SerializeField] private float rotationSpeed = 10f;
    
    private void Update()
    {
        Vector3 viewDirection = m_playerTransform.position - new Vector3(transform.position.x, transform.position.y, transform.position.z);
        m_orientationTransform.forward = viewDirection.normalized;
        
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        Vector3 inputDirection = m_orientationTransform.forward * verticalInput + m_orientationTransform.right * horizontalInput;

        if (inputDirection != Vector3.zero)
        {
            m_playerVisualTransform.forward = Vector3.Slerp(m_playerVisualTransform.forward,  inputDirection.normalized, rotationSpeed * Time.deltaTime);
        }
    }
}
