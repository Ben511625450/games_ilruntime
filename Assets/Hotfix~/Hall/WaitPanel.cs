using UnityEngine.UI;

namespace Hotfix.Hall
{
    public class WaitPanel : PanelBase
    {
        public WaitPanel() : base(UIType.Top, nameof(WaitPanel))
        {
        }

        private Text content;

        protected override void FindComponent()
        {
            base.FindComponent();
            content = transform.FindChildDepth<Text>($"Text");
        }

        public override void Create(params object[] args)
        {
            base.Create(args);
            if (args.Length <= 0)
            {
                content.text = $"加载中…";
            }
            else
            {
                content.text = args[0] == null ? $"加载中…" : args[0].ToString();
            }
        }

        public void SetContent(string msg)
        {
            content.text = msg;
        }
    }
}