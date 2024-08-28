using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.UI.Common.UIAnimator.Main
{
    public class UIAnimator : MonoBehaviour
    {
        [SerializeField]
#if UNITY_EDITOR
        [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "@ElementName", Expanded = true)]
        [OnStateUpdate(nameof(UpdateTime))]
#endif
        List<AnimationGroup> _animations = new List<AnimationGroup>();

        [SerializeField]
        private bool _isLooped = false;

        [SerializeField]
        private bool _useAnimationsOnStop;

        [SerializeField, ShowIf(nameof(_useAnimationsOnStop))]
        private AnimationGroup _stopGroup;

        private Coroutine _playCO;

        [Button]
        public void Play()
        {
            if (_playCO != null)
                StopCoroutine(_playCO);
            if (isActiveAndEnabled)
                _playCO = StartCoroutine(PlayCO());
        }

        [Button]
        public void Stop()
        {
            StopPlay();
            _stopGroup.Play();
        }

        public float GetAnimatorWorkTime()
        {
            float result = 0;
            foreach (var animationGroup in _animations)
            {
                if (animationGroup != null)
                    result = animationGroup.PlayEndTime;
            }

            return result;
        }

        private void OnDestroy()
        {
            StopPlay();
        }

        private IEnumerator PlayCO()
        {
            float lastGroupTime = 0;
            foreach (var animationGroup in _animations)
            {
                yield return new WaitForSeconds(animationGroup.PlayDelay);
                animationGroup.Play();
                lastGroupTime = animationGroup.PlayEndTime - animationGroup.PlayDelay;
            }

            if (_isLooped)
            {
                yield return new WaitForSeconds(lastGroupTime);
                _playCO = StartCoroutine(PlayCO());
            }
        }

        private void StopPlay()
        {
            bool saveLoop = _isLooped;
            _isLooped = false;
            if (_playCO != null)
                StopCoroutine(_playCO);

            foreach (var animationGroup in _animations)
                animationGroup.Stop();
            _isLooped = saveLoop;

            _stopGroup.Stop();
        }

#if UNITY_EDITOR
        private void UpdateTime()
        {
            float _startTime = 0;
            foreach (var animationGroup in _animations)
            {
                animationGroup.SetStartTime(_startTime);
                _startTime += animationGroup.PlayDelay;
            }
        }
#endif
    }
}