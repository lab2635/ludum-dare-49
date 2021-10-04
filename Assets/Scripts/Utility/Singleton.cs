namespace Utility
{
    public class Singleton<T> where T : new()
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (Singleton<T>.instance == null)
                {
                    Singleton<T>.instance = new T();
                }
                return Singleton<T>.instance;
            }
        }

        public static bool Exists
        {
            get { return Singleton<T>.Instance != null; }
        }
    }
}
