using UnityEngine;
using UnityEngine.UI;

public class HolyWheatCollectible : MonoBehaviour, ICollectiable
{
    [SerializeField] PlayerController _playerController;
    [SerializeField] WheatDesignSO _wheatDesign;
    [SerializeField] private PlayerStateUI _playerStateUI;
    
    private RectTransform _playerBoosterTransform;
    private Image _playerBoosterImage;

    private void Awake()
    {
        _playerBoosterTransform = _playerStateUI.GetBoosterJumpTransform();
        _playerBoosterImage = _playerBoosterTransform.GetComponent<Image>();
    }
    public void Collect()
    {
        _playerController.SetJumpForce(_wheatDesign.IncreaseDescraseMultiplier, _wheatDesign.ResetBoostDuration);
        _playerStateUI.PlayBoosterUIAnimation(_playerBoosterTransform,_playerBoosterImage,_playerStateUI.GetHolyBoosterImage(),_wheatDesign.ActiveSprite, 
            _wheatDesign.PassiveSprite, _wheatDesign.ActiveWheatSprite, _wheatDesign.PassiveWheatSprite, _wheatDesign.ResetBoostDuration);
        Destroy(this.gameObject);
    }
}
