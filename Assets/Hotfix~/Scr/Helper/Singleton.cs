using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotfix
{
    public class Singleton<T>: HotfixComponent where T:Singleton<T>
    {
        protected static T m_Instance;
        public static T Instance
        {
            get
            {
                return m_Instance;
            }
        }
        protected override void Awake()
        {
            base.Awake();
            m_Instance = this as T;

        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            m_Instance = null;
        }
    }
}
