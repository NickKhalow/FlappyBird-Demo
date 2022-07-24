using System;
using System.Threading;
using System.Threading.Tasks;
using AlexKo.Framework.Entities;
using AlexKo.Framework.Nodes;
using AlexKo.Framework.ObjectPooling;
using AlexKo.Framework.Utils;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Example.FlappyBorb.Scripts.Tubes
{
    public class TubeGenerator : NodeBehaviour<TubeGenerator.Context>
    {
        private ObjectPool<Tube> _objectPoolTubes;

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

        public record Context(
            Params Params,
            Tube.Params TubeParams,
            ReactiveCollection<Tube> InstantiatedTubes) : AbstractContext;

        private float RandomSpawnOffset => Random.Range(Ctx.Params._minSpawnOffset, Ctx.Params._maxSpawnOffset);

        protected override void ApplyContext(Context context)
        {
            _objectPoolTubes = new ObjectPool<Tube>(
                () =>
                {
                    var tube = Instantiate(Ctx.Params._tubePrefab, transform);
                    tube.AttachTo(tube, new Tube.Context(Ctx.TubeParams));
                    Ctx.InstantiatedTubes.Add(tube);
                    return tube;
                },
                tube =>
                {
                    var t = tube.transform;
                    t.position = transform.position + Vector3.up * RandomSpawnOffset;
                    t.rotation = Quaternion.identity;
                }
            );
        }

        public async Task StartEmit(CancellationToken cancellationToken)
        {
            while (this)
            {
                await Random.Range(Ctx.Params._minDelay, Ctx.Params._maxDelay).Await();
                cancellationToken.ThrowIfCancellationRequested();
                var tube = _objectPoolTubes.GetNewOrCached();
                DisposeAfterDelay(tube, cancellationToken);
            }
        }

        private async void DisposeAfterDelay(PooledObject<Tube> tube, CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(Ctx.Params._disposeDelay), cancellationToken);
                tube.ReturnToCached();
            }
            catch (OperationCanceledException)
            {
                //ignore
            }
        }
    }
}