namespace BMS.Core.Common
{
    /// <summary>
    /// 제네릭 싱글톤 클래스 - ex) Singleton<List<int>>.Instance.Add(1);
    /// </summary>
    /// <typeparam name="T">object</typeparam>
    public sealed class Singleton<T> where T : class, new()
    {
        private Singleton()
        {
        }

        // 싱글톤 객체 조회 메소드
        public static T Instance
        {
            get
            {
                return _instance.Value;
            }
        }
        private static readonly Lazy<T> _instance = new Lazy<T>(() => new T());
    }
}
