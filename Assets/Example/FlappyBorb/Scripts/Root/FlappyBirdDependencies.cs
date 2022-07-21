using System;
using AlexKo.Framework.Config;
using AlexKo.UI.InputPanels;
using Example.FlappyBorb.Scripts.Gameplay;
using Example.FlappyBorb.Scripts.Tubes;
using Example.FlappyBorb.Scripts.UI;
using UnityEngine;

namespace Example.FlappyBorb.Scripts.Root
{
    [Serializable]
    public class FlappyBirdDependencies : RootDependencies
    {
        public Bird _bird;
        public InputHandler _inputHandler;
        public TubeGenerator _tubeGenerator;
        
        [Header("UI")]
        public RestartButton _restartButton;
        public RecordCounter _recordCounter;
    }
}