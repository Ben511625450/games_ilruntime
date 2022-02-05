using System.Collections.Generic;

namespace Hotfix
{
    public delegate void EventAction(params object[] args);

    public class EventArgs
    {
        public string EventName;
        public object[] args;
    }

    /// <summary>
    /// 事件管理器
    /// </summary>
    public class EventComponent : SingletonILEntity<EventComponent>
    {
        private Dictionary<string, EventAction> _mEventDic = new Dictionary<string, EventAction>();
        private Queue<EventArgs> _queue = new Queue<EventArgs>();
        private readonly object _lockObject = new object();

        protected override void Awake()
        {
            base.Awake();
            ClearListener();
        }

        protected override void Update()
        {
            base.Update();
            if (_queue.Count <= 0) return;
            lock (_lockObject)
            {
                while (_queue.Count > 0)
                {
                    var e = _queue.Dequeue();
                    if (_mEventDic.TryGetValue(e.EventName, out EventAction action))
                    {
                        action?.Invoke(e.args);
                    }
                }
            }
        }

        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="action">事件委托函数</param>
        public void AddListener(string eventName, EventAction action)
        {
            if (action == null) return;
            if (!_mEventDic.ContainsKey(eventName))
            {
                _mEventDic.Add(eventName, null);
            }

            _mEventDic[eventName] += action;
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="action">事件委托函数</param>
        public void RemoveListener(string eventName, EventAction action)
        {
            if (action == null) return;
            if (!_mEventDic.ContainsKey(eventName)) return;
            _mEventDic[eventName] -= action;
            if (_mEventDic[eventName] == null) ClearListener(eventName);
        }

        /// <summary>
        /// 出发事件异步
        /// </summary>
        /// <param name="evenName">事件名</param>
        /// <param name="args">事件参数</param>
        public void DispatchListener(string evenName, params object[] args)
        {
            EventArgs eventArgs = new EventArgs() {EventName = evenName, args = args};
            DispatchListener(eventArgs);
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="args">事件类型</param>
        public void DispatchListener(EventArgs args)
        {
            if (args == null) return;
            _queue.Enqueue(args);
        }

        /// <summary>
        /// 触发事件同步
        /// </summary>
        /// <param name="evenName">事件名</param>
        /// <param name="args">事件参数</param>
        public void DispatchImmediatelyListener(string evenName, params object[] args)
        {
            _mEventDic.TryGetValue(evenName, out EventAction action);
            action?.Invoke(args);
        }

        /// <summary>
        /// 触发事件同步
        /// </summary>
        /// <param name="args">事件类型</param>
        public void DispatchImmediatelyListener(EventArgs args)
        {
            _mEventDic.TryGetValue(args.EventName, out EventAction action);
            action?.Invoke(args.args);
        }

        /// <summary>
        /// 清理事件管理器
        /// </summary>
        public void ClearListener()
        {
            List<EventAction> actions = new List<EventAction>(_mEventDic.Values);
            for (int i = 0; i < actions.Count; i++)
            {
                actions[i] = null;
            }

            _mEventDic.Clear();
            _queue.Clear();
        }

        /// <summary>
        /// 移除某一个类型所有事件
        /// </summary>
        /// <param name="eventName"></param>
        public void ClearListener(string eventName)
        {
            if (!_mEventDic.ContainsKey(eventName)) return;
            _mEventDic[eventName] = null;
            _mEventDic.Remove(eventName);
        }
    }
}