using UnityEngine;

public class SpatulaBooster : MonoBehaviour, IBoostable
{
    [SerializeField] private float _jumpForce;
    [SerializeField] Animator _animator;

    private bool _isActivating;

    public void Boost(PlayerController player)
    {
        if (_isActivating == true) return;
        
        Rigidbody rb = player.GetPlayerRigidbody();
        PlayerBoostAnimation();
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(transform.forward * _jumpForce, ForceMode.Impulse);
        _isActivating = true;
        Invoke(nameof(SpatulaAnimReset), 0.2f);
        
    }
    
    private void PlayerBoostAnimation()
    {
        _animator.SetTrigger(Consts.Spatula.SPATULE);
    }

    private void SpatulaAnimReset()
    {
        _isActivating = false;
    }
}
