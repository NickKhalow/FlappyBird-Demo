using AlexKo.Framework.Config;
using AlexKo.UI.InputPanels;
using Example.FlappyBorb.Scripts.Gameplay;
using Example.FlappyBorb.Scripts.Tubes;
using UnityEngine;

namespace Example.FlappyBorb.Scripts.Root
{
    [CreateAssetMenu(fileName = "Flappy Bird Config", menuName = "FlappyBird/Config", order = 0)]
    public class FlappyBirdConfig : RootConfig
    {
        public Bird.Params _birdParams;
        public InputHandler.Params _inputHandlerParams;
        public TubeGenerator.Params _tubeGeneratorParams;
        public Tube.Params _tubeParams;
    }
}