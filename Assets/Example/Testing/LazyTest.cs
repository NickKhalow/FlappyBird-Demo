using AlexKo.Framework.Lazy;
using UnityEngine;

namespace Example.Testing
{
    public class LazyTest : MonoBehaviour
    {
        private readonly LazyGet<Transform> _lazyTransform = new();

        private void Start()
        {
            const int iterations = 10_000_000;

            for (int i = 0; i < iterations; i++)
            {
                new LazyGet<Transform>().Get(this);
            }

            var passed = Time.realtimeSinceStartup;
            Debug.Log($"Total {passed}");

            for (var i = 0; i < iterations; i++)
            {
                _lazyTransform.Get(this);
            }

            Debug.Log($"Total {Time.realtimeSinceStartup - passed}");
        }
    }
}