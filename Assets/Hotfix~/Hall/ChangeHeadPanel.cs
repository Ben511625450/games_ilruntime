using LuaFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix.Hall
{
    public class ChangeHeadPanel : PanelBase
    {
        private Button CloseBtn;

        private ScrollRect HeadSelectArea;

        private int selectHeadIndex;
        private Button SureBtn;


        public ChangeHeadPanel() : base(UIType.Middle, nameof(ChangeHeadPanel))
        {

        }

        public override void Create(params object[] args)
        {
            base.Create(args);
            InitHead();
        }


        protected override void FindComponent()
        {
            SureBtn = transform.FindChildDepth<Button>("SureBtn");

            CloseBtn = transform.FindChildDepth<Button>("CloseBtn");

            HeadSelectArea = transform.FindChildDepth<ScrollRect>("SelectArea");
        }

        /// <summary>
        /// 初始化头像显示
        /// </summary>
        private void InitHead()
        {
            GameObject headIcons = ILGameManager.Instance.HeadIcons.gameObject;
            selectHeadIndex = 0;

            for (var i = 0; i < headIcons.transform.childCount; i++)
            {
                Transform child;
                if (i < HeadSelectArea.content.childCount)
                {
                    child = HeadSelectArea.content.GetChild(i);
                }
                else
                {
                    child = Object.Instantiate(HeadSelectArea.content.GetChild(0).gameObject).transform;
                    child.SetParent(HeadSelectArea.content);
                    child.localPosition = Vector3.zero;
                    child.localScale = Vector3.one;
                }

                if (i == GameLocalMode.Instance.SCPlayerInfo.FaceID - 1)
                {
                    selectHeadIndex = i;
                    child.FindChildDepth("Select").gameObject.SetActive(true);
                }
                else
                {
                    child.FindChildDepth("Select").gameObject.SetActive(false);
                }

                child.FindChildDepth("Bg").gameObject.SetActive(true);
                child.GetComponent<Image>().sprite = headIcons.transform.GetChild(i).GetComponent<Image>().sprite;
                child.FindChildDepth("Bg").GetComponent<Image>().sprite =
                    headIcons.transform.GetChild(i).GetComponent<Image>().sprite;

                child.GetComponent<Button>().onClick.RemoveAllListeners();
                child.GetComponent<Button>().onClick.Add(() =>
                {
                    SelectHeadCall(child, selectHeadIndex);
                    selectHeadIndex = child.GetSiblingIndex();
                });
            }
        }

        /// <summary>
        ///     选择头像
        /// </summary>
        /// <param name="go"></param>
        /// <param name="index"></param>
        private void SelectHeadCall(Transform go, int index)
        {
            DebugHelper.Log("index====" + index);
            ILMusicManager.Instance.PlayBtnSound();
            HeadSelectArea.content.GetChild(index).FindChildDepth("Select").gameObject.SetActive(false);
            HeadSelectArea.content.GetChild(index).FindChildDepth("Bg").gameObject.SetActive(true);
            go.transform.FindChildDepth("Select").gameObject.SetActive(true);
        }

        protected override void AddListener()
        {
            SureBtn.onClick.RemoveAllListeners();
            SureBtn.onClick.Add(ChangeHeadCall);

            CloseBtn.onClick.RemoveAllListeners();
            CloseBtn.onClick.Add(CloseChangeHeadPanelCall);
        }

        private void ChangeHeadCall()
        {
            ILMusicManager.Instance.PlayBtnSound();

            var bytebuffer = new ByteBuffer();
            bytebuffer.WriteByte(selectHeadIndex + 1);
            HotfixGameComponent.Instance.Send(DataStruct.PersonalStruct.MDM_3D_PERSONAL_INFO,
                DataStruct.PersonalStruct.SUB_3D_CS_ChangeHeader, bytebuffer, SocketType.Hall);
        }

        private void CloseChangeHeadPanelCall()
        {
            ILMusicManager.Instance.PlayBtnSound();
            UIManager.Instance.Close();
        }
    }
}