using System;
using System.Threading;
using System.Threading.Tasks;
using AlexKo.Framework.Entities;
using AlexKo.Framework.Nodes;
using AlexKo.Framework.Utils;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Example.FlappyBorb.Scripts.Tubes
{
    public class TubeGenerator : NodeBehaviour<TubeGenerator.Context>
    {
        [Serializable]
        public class Params
        {
            public float _minDelay = 0.3f;
            public float _maxDelay = 0.3f;

            [Space] public float _minSpawnOffset = -1f;
            public float _maxSpawnOffset = 0.3f;

            [Space] public Tube _tubePrefab;

            [Space] public float _disposeDelay = 10f;
        }

        public record Context
            (Params Params, Tube.Params TubeParams, ReactiveCollection<Tube> InstantiatedTubes) : AbstractContext;

        private float RandomSpawnOffset => Random.Range(Ctx.Params._minSpawnOffset, Ctx.Params._maxSpawnOffset);

        public async Task StartEmit(CancellationToken cancellationToken)
        {
            while (this)
            {
                await Random.Range(Ctx.Params._minDelay, Ctx.Params._maxDelay).Await();
                cancellationToken.ThrowIfCancellationRequested();
                var tube = Instantiate(
                    Ctx.Params._tubePrefab,
                    transform.position + Vector3.up * RandomSpawnOffset,
                    Quaternion.identity);
                tube.AttachTo(this, new Tube.Context(Ctx.TubeParams));
                Ctx.InstantiatedTubes.Add(tube);
            }
        }

        private async void DisposeAfterDelay(Tube tube, CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(Ctx.Params._disposeDelay), cancellationToken);
                tube.Dispose();
            }
            catch (OperationCanceledException)
            {
                //ignore
            }
        }
    }
}