using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.UI.Tutorial
{
    public class TutorialController
    {
        private int _currentStep = 0;
        private List<TutorialStep> _steps;
        private TutorialHand _tutorialHand;
        private TutorialText _tutorialText;
        private Action _onTutorialComplete;

        public void StartTutorial(List<TutorialStep> steps, TutorialHand tutorialHand, TutorialText tutorialText,
            Action OnTutorialComplete)
        {
            _onTutorialComplete = OnTutorialComplete;
            _tutorialText = tutorialText;
            _tutorialHand = tutorialHand;
            _steps = steps;
            _currentStep = 0;

            SetupStep();
        }

        private void OnStepComplete()
        {
            _currentStep += 1;
            if (_currentStep >= _steps.Count)
                OnTutorialComplete();
            else
                SetupStep();
        }

        private void SetupStep()
        {
            _tutorialHand.ShowHand(_steps[_currentStep].BoltPosition);
            _tutorialText.ShowText(_steps[_currentStep].Title, _steps[_currentStep].BoltPosition);
        }

        private void OnTutorialComplete()
        {
            _tutorialHand.Hide();
            _tutorialText.Hide();
            _onTutorialComplete?.Invoke();
        }

        public void OnScrewBolt(Vector2 boltPosition)
        {
            if (_steps[_currentStep].StepType != StepType.ScrewBolt)
                return;

            if (Vector2.Distance(boltPosition, _steps[_currentStep].BoltPosition) < 0.1f)
                OnStepComplete();
        }

        public void OnUnscrewBolt(Vector2 boltPosition)
        {
            if (_steps[_currentStep].StepType != StepType.UnscrewBolt)
                return;

            if (Vector2.Distance(boltPosition, _steps[_currentStep].BoltPosition) < 0.1f)
                OnStepComplete();
        }
    }
}