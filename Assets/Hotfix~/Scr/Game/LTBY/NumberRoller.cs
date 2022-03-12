using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix.LTBY
{
    public class NumberRoller : ILHotfixEntity
    {
        public Text _bindText;
        public TextMeshProUGUI _bindTMPText;
        private bool isRich;

        public bool _useSplit;

        private long _baseNum;

        private long _deltaNum;

        private long _toNum;

        private Tweener key = null;

        private bool isTMP;

        private bool isInit = false;

        public string text
        {
            get { return _baseNum.ToString(); }
            set
            {
                FindComponent();
                if (_useSplit)
                {
                    if (isTMP)
                        _bindTMPText.text = isRich ? ToolHelper.ShowRichText(value, true) : ToolHelper.ShowThousandText(value);
                    else _bindText.text = ToolHelper.ShowThousandText(value);
                }
                else
                {
                    if (isTMP) _bindTMPText.text = isRich ? ToolHelper.ShowRichText($"{value}") : $"{value}";
                    else _bindText.text = $"{value}";
                }
            }
        }

        protected override void FindComponent()
        {
            base.FindComponent();
            if(isInit) return;
            isTMP = false;
            _bindText = gameObject.GetComponent<Text>();
            if (_bindText != null) return;
            _bindTMPText = gameObject.GetComponent<TextMeshProUGUI>();
            isTMP = true;
            isInit = true;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (key == null || !key.IsActive()) return;
            key?.Kill();
            key = null;
        }

        public void Init(bool isSplite = false, bool isRich = true)
        {
            _useSplit = isSplite;
            this.isRich = isRich;
            _toNum = 0;
            _deltaNum = 0;
            _baseNum = 0;
            FindComponent();
        }

        public void RollBy(long deltaNum, float time)
        {
            _toNum += deltaNum;
            if (key != null && key.IsActive()) key?.Kill();
            _baseNum = _deltaNum;
            key = DOTween.To(value =>
            {
                float v = Mathf.Ceil(value);
                if (_useSplit)
                {
                    if (isTMP)
                    {
                        if (_bindTMPText == null) return;
                        _bindTMPText.text = isRich ? ToolHelper.ShowRichText($"{v:F0}", true) : ToolHelper.ShowThousandText($"{v:F0}");
                    }
                    else
                    {
                        if (_bindText == null) return;
                        _bindText.text = ToolHelper.ShowThousandText($"{v:F0}");
                    }
                }
                else
                {
                    if (isTMP)
                    {
                        if (_bindTMPText == null) return;
                        _bindTMPText.text = isRich ? ToolHelper.ShowRichText($"{v:F0}", false) : $"{v:F0}";
                    }
                    else
                    {
                        if (_bindText == null) return;
                        _bindText.text = $"{v:F0}";
                    }
                }
            }, _baseNum, _toNum, time).SetEase(Ease.Linear).OnComplete(() =>
            {
                key = null;
                if (_useSplit)
                {
                    if (isTMP)
                    {
                        if (_bindTMPText == null) return;
                        _bindTMPText.text = isRich ? ToolHelper.ShowRichText(_toNum, true) : ToolHelper.ShowThousandText(_toNum);
                    }
                    else
                    {
                        if (_bindText == null) return;
                        _bindText.text = ToolHelper.ShowThousandText(_toNum);
                    }
                }
                else
                {
                    if (isTMP)
                    {
                        if (_bindTMPText == null) return;
                        _bindTMPText.text = isRich ? ToolHelper.ShowRichText(_toNum, false) : $"{_toNum}";
                    }
                    else
                    {
                        if (_bindText == null) return;
                        _bindText.text = $"{_toNum}";
                    }
                }
            });
            key?.SetAutoKill();
        }

        public void RollTo(long to, float time)
        {
            _toNum = to;
            if (key != null && key.IsActive()) key?.Kill();
            _baseNum = _deltaNum;
            key = DOTween.To(value =>
            {
                float v = Mathf.Ceil(value);
                if (_useSplit)
                {
                    if (isTMP)
                    {
                        if (_bindTMPText == null) return;
                        _bindTMPText.text = isRich ? ToolHelper.ShowRichText($"{v:F0}", true) : ToolHelper.ShowThousandText($"{v:F0}");
                    }
                    else
                    {
                        if (_bindText == null) return;
                        _bindText.text = ToolHelper.ShowThousandText($"{v:F0}");
                    }
                }
                else
                {
                    if (isTMP)
                    {
                        if (_bindTMPText == null) return;
                        _bindTMPText.text = isRich ? ToolHelper.ShowRichText($"{v:F0}") : $"{v:F0}";
                    }
                    else
                    {
                        if (_bindText == null) return;
                        _bindText.text = $"{v:F0}";
                    }
                }
            }, _baseNum, _toNum, time).OnComplete(() =>
            {
                key = null;
                if (_useSplit)
                {
                    if (isTMP)
                    {
                        if (_bindTMPText == null) return;
                        _bindTMPText.text = isRich ? ToolHelper.ShowRichText(_toNum, true) : ToolHelper.ShowThousandText(_toNum);
                    }
                    else
                    {
                        if (_bindText == null) return;
                        _bindText.text = ToolHelper.ShowThousandText(_toNum);
                    }
                }
                else
                {
                    if (isTMP)
                    {
                        if (_bindTMPText == null) return;
                        _bindTMPText.text = isRich ? ToolHelper.ShowRichText(_toNum) : $"{_toNum}";
                    }
                    else
                    {
                        if (_bindText == null) return;
                        _bindText.text = $"{_toNum}";
                    }
                }
            });
            key?.SetAutoKill();
        }

        public void RollFromTo(long from, long to, float time)
        {
            _baseNum = from;
            _toNum = to;
            if (key != null && key.IsActive()) key?.Kill();
            if (_useSplit)
            {
                if (isTMP)
                {
                    if (_bindTMPText == null) return;
                    _bindTMPText.text = isRich ? ToolHelper.ShowRichText(from, true) : ToolHelper.ShowThousandText(from);
                }
                else
                {
                    if (_bindText == null) return;
                    _bindText.text = ToolHelper.ShowThousandText(from);
                }
            }
            else
            {
                if (isTMP)
                {
                    if (_bindTMPText == null) return;
                    _bindTMPText.text = isRich ? ToolHelper.ShowRichText($"{from:F0}", true) : $"{from:F0}";
                }
                else
                {
                    if (_bindText == null) return;
                    _bindText.text = $"{from:F0}";
                }
            }

            key = DOTween.To(value =>
            {
                float v = Mathf.Ceil(value);
                if (_useSplit)
                {
                    if (isTMP)
                    {
                        if (_bindTMPText == null) return;
                        _bindTMPText.text = isRich ? ToolHelper.ShowRichText($"{v:F0}", true) : ToolHelper.ShowThousandText($"{v:F0}");
                    }
                    else
                    {
                        if (_bindText == null) return;
                        _bindText.text = ToolHelper.ShowThousandText($"{v:F0}");
                    }
                }
                else
                {
                    if (isTMP)
                    {
                        if (_bindTMPText == null) return;
                        _bindTMPText.text = isRich ? ToolHelper.ShowRichText($"{v:F0}") : $"{v:F0}";
                    }
                    else
                    {
                        if (_bindText == null) return;
                        _bindText.text = $"{v:F0}";
                    }
                }
            }, _baseNum, _toNum, time).OnComplete(() =>
            {
                key = null;

                if (_useSplit)
                {
                    if (isTMP)
                    {
                        if (_bindTMPText == null) return;
                        _bindTMPText.text = isRich ? ToolHelper.ShowRichText(_toNum, true) : ToolHelper.ShowThousandText(_toNum);
                    }
                    else
                    {
                        if (_bindText == null) return;
                        _bindText.text = ToolHelper.ShowThousandText(_toNum);
                    }
                }
                else
                {
                    if (isTMP)
                    {
                        if (_bindTMPText == null) return;
                        _bindTMPText.text = isRich ? ToolHelper.ShowRichText($"{_toNum}") : $"{_toNum}";
                    }
                    else
                    {
                        if (_bindText == null) return;
                        _bindText.text = $"{_toNum}";
                    }
                }
            });
            key?.SetAutoKill();
        }

        public long GetFinalNum()
        {
            return _toNum;
        }
    }
}