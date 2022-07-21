using AlexKo.Framework.Entities;
using AlexKo.Framework.Lazy;
using AlexKo.Framework.Nodes;
using AlexKo.Framework.ReactiveTriggers;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Example.FlappyBorb.Scripts.UI
{
    [RequireComponent(typeof(Text))]
    public class RecordCounter : NodeBehaviour<RecordCounter.Context>
    {
        private readonly LazyGet<Text> _lazyText = new();
        
        public record Context(IReadonlyReactiveTrigger BirdPassedTube, IntReactiveProperty CountPassed) : AbstractContext;

        protected override void ApplyContext(Context context)
        {
            context.BirdPassedTube.SubscribeWithSkipFirst(IncreaseCount).AddTo(this);
            context.CountPassed.Subscribe(UpdateText);
        }

        private void IncreaseCount()
        {
            Ctx.CountPassed.Value += 1;
        }

        private void UpdateText(int count)
        {
            _lazyText.Get(this).text = count.ToString();
        }
    }
}