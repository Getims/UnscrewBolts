using Scripts.GameLogic.Sound;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using AudioType = Scripts.Core.Enums.AudioType;

namespace Scripts.UI.Common.Settings
{
    public class SoundButton : MonoBehaviour
    {
        [SerializeField]
        private AudioType _audioType = AudioType.Sound;

        [SerializeField]
        private Button _soundButton;

        [SerializeField]
        private Image _onBackground;

        [SerializeField]
        private Image _onIcon;

        [SerializeField]
        private Image _offIcon;

        private ISoundService _soundService;

        [Inject]
        public void Construct(ISoundService soundService) =>
            _soundService = soundService;

        public virtual void UpdateInfo()
        {
            bool isSoundOn = _audioType == AudioType.Sound ? _soundService.IsSoundOn : _soundService.IsMusicOn;
            SetSprite(isSoundOn);
        }

        private void Start() =>
            _soundButton.onClick.AddListener(OnSoundClick);

        private void OnDestroy() =>
            _soundButton.onClick.RemoveListener(OnSoundClick);

        private void SetSprite(bool isOn)
        {
            _onBackground.enabled = isOn;
            _onIcon.enabled = isOn;
            _offIcon.enabled = !isOn;
        }

        private void OnSoundClick()
        {
            bool newState;
            if (_audioType == AudioType.Sound)
            {
                newState = !_soundService.IsSoundOn;
                _soundService.SwitchSoundState(newState);
            }
            else
            {
                newState = !_soundService.IsMusicOn;
                _soundService.SwitchMusicState(newState);
            }

            SetSprite(newState);
        }
    }
}