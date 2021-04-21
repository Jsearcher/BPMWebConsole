namespace BPMWebConsole.Factory.CustomSession
{
    /// <summary>
    /// Session強型別包裝介面
    /// </summary>
    /// <typeparam name="T">目標包裝型別</typeparam>
    public interface ISessionWapper<T>
    {
        /// <summary>
        /// 目標模型物件
        /// </summary>
        T Model { get; set; }
    }
}
