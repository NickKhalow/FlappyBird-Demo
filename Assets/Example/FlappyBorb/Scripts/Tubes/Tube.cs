using System;
using AlexKo.Framework.Entities;
using AlexKo.Framework.Nodes;
using UnityEngine;

namespace Example.FlappyBorb.Scripts.Tubes
{
    public class Tube : NodeBehaviour<Tube.Context>
    {
        [Serializable]
        public class Params
        {
            public Vector3 _speed;
        }
        
        public record Context(Params Params) : AbstractContext;
        
        
        public void Freeze()
        {
            enabled = false;
        }

        private void Update()
        {
            transform.Translate(Ctx.Params._speed * Time.deltaTime);
        }

        protected override void OnDispose()
        {
            Destroy(gameObject);
        }
    }
}