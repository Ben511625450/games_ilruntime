using UnityEngine;
using UnityEngine.UI;

namespace Hotfix.Hall
{
    public class GWPanel : PanelBase
    {
        private Button closeBtn;
        private Button CopyBtn;

        private Image GWImage;
        private Text GWURL;
        private Button maskBtn;

        public GWPanel(): base(UIType.Middle, nameof(GWPanel))
        {

        }

        protected override void FindComponent()
        {
            closeBtn = transform.FindChildDepth<Button>("Content/CloseBtn");
            maskBtn = transform.FindChildDepth<Button>("Mask");
            CopyBtn = transform.FindChildDepth<Button>("Content/CopyBtn");

            GWImage = transform.FindChildDepth<Image>("Content/Image");
            GWURL = transform.FindChildDepth<Text>("Content/GWURL");
            Init();
        }

        protected override void AddListener()
        {
            closeBtn.onClick.RemoveAllListeners();
            closeBtn.onClick.Add(CloseGWPanel);
            maskBtn.onClick.RemoveAllListeners();
            maskBtn.onClick.Add(CloseGWPanel);

            CopyBtn.onClick.RemoveAllListeners();
            CopyBtn.onClick.Add(() =>
            {
                var str = GWURL.text;
                ToolHelper.SetText(str);
                ToolHelper.PopSmallWindow("官网复制成功");
            });
        }

        private void Init()
        {
            GWURL.text = GameLocalMode.Instance.GWData.GWUrl;
        }

        private void CloseGWPanel()
        {
            UIManager.Instance.CloseUI<GWPanel>();
        }
    }
}