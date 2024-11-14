namespace Game.Core
{
    public class Singleton<T> where T : class, new()
    {
        private static T m_instance = null;

        public static T Instance()
        {
            if (m_instance == null)
            {
                m_instance = new T();
            }

            return m_instance;
        }

        public virtual void OnDisable()
        {
            m_instance = null;
        }
    }
}