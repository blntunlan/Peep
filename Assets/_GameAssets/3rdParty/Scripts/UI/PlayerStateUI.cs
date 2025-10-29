using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateUI : MonoBehaviour
{
    [Header("References")] 
    [SerializeField]private PlayerController _playerController;
    [SerializeField]private RectTransform _playerWalkingTransform;
    [SerializeField]private RectTransform _playerSlidingTransfrom;
    [SerializeField]private RectTransform _boosterSpeedTransform;
    [SerializeField]private RectTransform _boosterJumpTransform;
    [SerializeField]private RectTransform _boosterSlowTransform;
    
    [Header("Images")]
    [SerializeField] private Image _goldBoosterImage;
    [SerializeField] private Image _holyBoosterImage;
    [SerializeField] private Image _rottenBoosterImage;
    
    
    [Header("Sprites")]
    [SerializeField] private Sprite _playerWalkingActiveSprite;
    [SerializeField] private Sprite _playerWalkingPassiveSprite;
    [SerializeField] private Sprite _playerSlidingActiveSprite;
    [SerializeField] private Sprite _playerSlidingPassiveSprite;
    
    [Header("Animation Settings")]
    [SerializeField] private float _moveDuration;
    [SerializeField] private Ease _moveEas;

    public RectTransform GetBoosterSpeedTransform() => _boosterSpeedTransform;
    public RectTransform GetBoosterJumpTransform() => _boosterJumpTransform;
    public RectTransform GetBoosterSlowTransform() => _boosterSlowTransform;
    
    public Image GetGoldBoosterImage() => _goldBoosterImage;
    public Image GetHolyBoosterImage() => _holyBoosterImage;
    public Image GetRottenBoosterImage() => _rottenBoosterImage;
    
    
    private Image _playerWalkingImage;
    private Image _playerSlidingImage;


    private void Awake()
    {
        _playerWalkingImage = _playerWalkingTransform.GetComponent<Image>();
        _playerSlidingImage = _playerSlidingTransfrom.GetComponent<Image>();

    }

    private void Start()
    {
        _playerController.OnPlayerStateChanged += PlayerControllerOnOnPlayerStateChanged;
        SetStateUI(_playerWalkingActiveSprite,_playerSlidingPassiveSprite,_playerWalkingTransform,_playerSlidingTransfrom);
        
    }

    private void PlayerControllerOnOnPlayerStateChanged(PlayerState playerState)
    {
        switch (playerState)
        {
            case PlayerState.Idle:
            case PlayerState.Move:
                SetStateUI(_playerWalkingActiveSprite,_playerSlidingPassiveSprite,_playerWalkingTransform,_playerSlidingTransfrom);
                break;
            case PlayerState.SlideIdle:
            case PlayerState.Slide:
                SetStateUI(_playerWalkingPassiveSprite,_playerSlidingActiveSprite,_playerSlidingTransfrom,_playerWalkingTransform);
                break;
        }
    }
    
    private void SetStateUI(Sprite playerWalkingSprite, Sprite playerSlidingSprite, RectTransform activeTransform, RectTransform passiveTransform)
    {
        _playerWalkingImage.sprite = playerWalkingSprite;
        _playerSlidingImage.sprite = playerSlidingSprite;
        activeTransform.DOAnchorPosX(-25, _moveDuration).SetEase(_moveEas);
        passiveTransform.DOAnchorPosX(-90, _moveDuration).SetEase(_moveEas);
    }

    private void OnDisable()
    {
        _playerController.OnPlayerStateChanged -= PlayerControllerOnOnPlayerStateChanged;
    }

    private IEnumerator SetBoosterUserInterface(RectTransform activeTransfrom, Image boosterImage, Image wheatImage, Sprite activeSprite, Sprite passiveSprite, Sprite activeWheatSprite, Sprite passiveWheatSprite, float duration)
    {
        boosterImage.sprite = activeSprite;
        wheatImage.sprite = activeWheatSprite;
        activeTransfrom.DOAnchorPosX(25, duration).SetEase(_moveEas);
        yield return new WaitForSeconds(duration);
        boosterImage.sprite = passiveSprite;
        wheatImage.sprite = passiveWheatSprite;
        activeTransfrom.DOAnchorPosX(90, duration).SetEase(_moveEas);
        
        yield return new WaitForSeconds(duration);
        
        boosterImage.sprite = passiveSprite;
        wheatImage.sprite = passiveWheatSprite;
        activeTransfrom.DOAnchorPosX(90, duration).SetEase(_moveEas);
    }

    public void PlayBoosterUIAnimation(RectTransform activeTransfrom, Image boosterImage, Image wheatImage,
        Sprite activeSprite, Sprite passiveSprite, Sprite activeWheatSprite, Sprite passiveWheatSprite, float duration)
    {
        StartCoroutine(SetBoosterUserInterface(activeTransfrom, boosterImage, wheatImage, activeSprite, passiveSprite,
            activeWheatSprite, passiveWheatSprite, duration));
    }
    
    
}


