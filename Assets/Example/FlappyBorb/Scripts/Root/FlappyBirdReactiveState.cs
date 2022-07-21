using System;
using AlexKo.Framework.Config;
using AlexKo.Framework.ReactiveTriggers;
using Example.FlappyBorb.Scripts.Tubes;
using UniRx;

namespace Example.FlappyBorb.Scripts.Root
{
    [Serializable]
    public class FlappyBirdReactiveState : RootReactiveState
    {
        public ReactiveTrigger birdCollideTube = new();
        public ReactiveTrigger birdPassTube = new();
        public IntReactiveProperty _passedTubesCount = new();
        public ReactiveCollection<Tube> _instantiatedTubes = new();
    }
}