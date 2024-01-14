using System.Collections;
#if UNITY_ENV
using UnityEngine;

namespace SangoUtils_Extensions_UnityEngine.Core
{
    public class CoroutineDriver : MonoBehaviour
    {
        internal static CoroutineDriver _driver;

        internal static CoroutineDriver Driver
        {
            get
            {
                if (null == _driver)
                {
                    _driver = FindObjectOfType(typeof(CoroutineDriver)) as CoroutineDriver;
                    if (null == _driver)
                    {
                        GameObject gameObject = new GameObject("[CoroutineDriver]");
                        _driver = gameObject.AddComponent<CoroutineDriver>();
                        DontDestroyOnLoad(gameObject);
                    }
                }
                return _driver;
            }
        }

        private void Awake()
        {
            if (null != _driver && _driver != this)
            {
                Destroy(gameObject);
            }
        }

        public static Coroutine Run(IEnumerator target)
        {
            return Driver.StartCoroutine(target);
        }
    }
}
#endif