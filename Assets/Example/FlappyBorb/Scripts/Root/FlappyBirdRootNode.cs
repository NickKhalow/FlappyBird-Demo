using System.Threading;
using AlexKo.Framework.Nodes;
using AlexKo.UI.InputPanels;
using Example.FlappyBorb.Scripts.Gameplay;
using Example.FlappyBorb.Scripts.Tubes;
using Example.FlappyBorb.Scripts.UI;
using UnityEngine.SceneManagement;

namespace Example.FlappyBorb.Scripts.Root
{
    public class FlappyBirdRootNode : RootNode<FlappyBirdConfig, FlappyBirdDependencies, FlappyBirdReactiveState>
    {
        public override async void Initialize(
            FlappyBirdConfig config,
            FlappyBirdDependencies dependencies,
            FlappyBirdReactiveState reactiveState)
        {
            var pointerDown = dependencies._inputHandler.PointerDown;

            dependencies._bird.AttachTo(this, new Bird.Context(
                config._birdParams,
                pointerDown,
                reactiveState.birdCollideTube,
                reactiveState.birdPassTube));
            dependencies._inputHandler.SetContext(new InputHandler.Context(config._inputHandlerParams));
            dependencies._tubeGenerator.AttachTo(this, new TubeGenerator.Context(
                config._tubeGeneratorParams,
                config._tubeParams,
                reactiveState._instantiatedTubes));
            dependencies._restartButton.AttachTo(this, new RestartButton.Context());
            dependencies._recordCounter.AttachTo(this, new RecordCounter.Context(
                reactiveState.birdPassTube,
                reactiveState._passedTubesCount));

            var cts = new CancellationTokenSource();

            dependencies._restartButton.Hide();

            var emitting = dependencies._tubeGenerator.StartEmit(cts.Token);

            await reactiveState.birdCollideTube.WaitNextTriggered(CancellationToken.None);

            dependencies._bird.BlockInput();
            dependencies._bird.Freeze();
            cts.Cancel();

            foreach (var tube in reactiveState._instantiatedTubes)
            {
                tube.Freeze();
            }

            dependencies._restartButton.Show();
            await dependencies._restartButton.AwaitForClick();
            SceneManager.LoadScene(0);
        }
    }
}