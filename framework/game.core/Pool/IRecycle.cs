namespace Game.Core
{
        /// <summary>
        /// 当ObjectPool没有主动设置OnGet,OnRelease,OnDestroy时才会调用对应的IRecycle方法
        /// </summary>
        public interface IRecycle
        {
            void OnGet();
            void OnRelease();
            void OnDestroy();
        }
}