using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    [SerializeField] private Transform m_playerTransform;
    [SerializeField] private Transform m_orientationTransform;
    [SerializeField] private Transform m_playerVisualTransform;
    [SerializeField, Range(0.01f, 1f)] private float rotationLerpSpeed = 0.15f;

    private void LateUpdate()
    {
        if (!m_playerTransform || !m_orientationTransform || !m_playerVisualTransform)
            return;

        // Kamera → Oyuncu yönü (Y eksenini sıfırla)
        Vector3 viewDirection = m_playerTransform.position - transform.position;
        viewDirection.y = 0f;

        if (viewDirection.sqrMagnitude > 0.001f)
            m_orientationTransform.forward = viewDirection.normalized;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 inputDirection = m_orientationTransform.forward * verticalInput +
                                 m_orientationTransform.right * horizontalInput;

        if (inputDirection.sqrMagnitude > 0.001f)
        {
            inputDirection.Normalize();
            m_playerVisualTransform.forward = Vector3.Slerp(
                m_playerVisualTransform.forward,
                inputDirection,
                rotationLerpSpeed
            );
        }
    }
}