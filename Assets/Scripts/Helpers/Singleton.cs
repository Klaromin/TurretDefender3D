using UnityEngine;

namespace VertigoDemo.Helpers
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        public static T Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("There's more than one " +name+ " " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }
            Instance = this as T;
        }
    }
}
