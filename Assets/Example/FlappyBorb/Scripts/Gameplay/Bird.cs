using System;
using AlexKo.Framework.Entities;
using AlexKo.Framework.Nodes;
using AlexKo.Framework.ReactiveTriggers;
using AlexKo.UI.InputPanels;
using UniRx;
using UnityEngine;

namespace Example.FlappyBorb.Scripts.Gameplay
{
    public class Bird : NodeBehaviour<Bird.Context>
    {
        [SerializeField] private Rigidbody _rigidbody;

        private IDisposable _disposableForInput;

        [Serializable]
        public class Params
        {
            public float _tapPower = 10;
            public ForceMode _forceMode = ForceMode.VelocityChange;
        }

        public record Context(
            Params Params,
            IReadonlyReactiveTrigger OnPointerDown,
            IWriteonlyReactiveTrigger BirdCollide,
            IWriteonlyReactiveTrigger PassTube) : AbstractContext;

        protected override void ApplyContext(Context context)
        {
            _disposableForInput = context.OnPointerDown.SubscribeWithSkipFirst(ApplyUpForce).AddTo(this);
        }

        public void BlockInput()
        {
            _disposableForInput.Dispose();
        }

        public void Freeze()
        {
            _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }

        private void ApplyUpForce()
        {
            _rigidbody.AddForce(Ctx.Params._tapPower * Vector3.up, Ctx.Params._forceMode);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Ctx.BirdCollide.Notify();
        }

        private void OnTriggerExit(Collider other)
        {
            Ctx.PassTube.Notify();
        }
    }
}