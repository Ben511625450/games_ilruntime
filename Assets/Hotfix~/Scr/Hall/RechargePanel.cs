using UnityEngine;
using UnityEngine.UI;

namespace Hotfix.Hall
{
    public class RechargePanel : PanelBase
    {
        private Button closeBtn;

        private Button DKCZBtn;
        private Transform mainPanel;
        private Button maskCloseBtn;

        public RechargePanel() : base(UIType.Middle, nameof(RechargePanel))
        {

        }

        protected override void FindComponent()
        {
            mainPanel = transform.FindChildDepth("mainPanel");

            DKCZBtn = mainPanel.FindChildDepth<Button>("DKCZBtn");
            closeBtn = mainPanel.FindChildDepth<Button>("CloseBtn");

            maskCloseBtn = transform.FindChildDepth<Button>("Mask");
        }

        protected override void AddListener()
        {
            closeBtn.onClick.RemoveAllListeners();
            closeBtn.onClick.Add(CloseShopPanel);

            maskCloseBtn.onClick.RemoveAllListeners();
            maskCloseBtn.onClick.Add(CloseShopPanel);

            DKCZBtn.onClick.RemoveAllListeners();
            DKCZBtn.onClick.Add(OnClickDKCZ);
        }

        private void OnClickDKCZ()
        {
            ILMusicManager.Instance.PlayBtnSound();
            if (GameLocalMode.Instance.SCPlayerInfo.IsVIP == 1)
            {
                ToolHelper.PopSmallWindow("VIP不能进行该操作");
                return;
            }
            UIManager.Instance.ReplaceUI<ExchangePanel>();
        }

        private void CloseShopPanel()
        {
            UIManager.Instance.Close();
        }
    }
}