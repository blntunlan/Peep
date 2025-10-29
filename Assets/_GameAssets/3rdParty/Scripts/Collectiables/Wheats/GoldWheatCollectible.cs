using UnityEngine;
using UnityEngine.UI;

public class GoldWheatCollectible : MonoBehaviour, ICollectiable
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private WheatDesignSO _wheatDesign;
    [SerializeField] private PlayerStateUI _playerStateUI;
    
    private RectTransform _playerBoosterTransform;
    private Image _playerBoosterImage;

    private void Awake()
    {
        _playerBoosterTransform = _playerStateUI.GetBoosterSpeedTransform();
        _playerBoosterImage = _playerBoosterTransform.GetComponent<Image>();
    }

    public void Collect()
    {
        _playerController.SetMovementSpeed(_wheatDesign.IncreaseDescraseMultiplier, _wheatDesign.ResetBoostDuration);
       _playerStateUI.PlayBoosterUIAnimation(_playerBoosterTransform,_playerBoosterImage,_playerStateUI.GetGoldBoosterImage(),_wheatDesign.ActiveSprite, 
           _wheatDesign.PassiveSprite, _wheatDesign.ActiveWheatSprite, _wheatDesign.PassiveWheatSprite, _wheatDesign.ResetBoostDuration);
        Destroy(this.gameObject);
    }
}
