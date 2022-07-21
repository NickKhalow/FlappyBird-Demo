using System;
using System.Threading.Tasks;
using AlexKo.Framework.Entities;
using AlexKo.Framework.Lazy;
using AlexKo.Framework.Nodes;
using AlexKo.Framework.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Example.FlappyBorb.Scripts.UI
{
    [RequireComponent(typeof(Button))]
    public class RestartButton : NodeBehaviour<RestartButton.Context>
    {
        private readonly LazyGet<Button> _lazyButton = new();

        public record Context : AbstractContext;

        public Task AwaitForClick() => _lazyButton.Get(this).AwaitForClick();

        public void Show()
        {
            _lazyButton.Get(this).gameObject.SetActive(true);
        }

        public void Hide()
        {
            _lazyButton.Get(this).gameObject.SetActive(false);
        }
    }
}