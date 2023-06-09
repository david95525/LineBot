namespace LineBot.Dtos
{
    public interface ISwaggerExample<T>
    {
        /// <summary> 
        /// 取得範例 
        /// </summary>
        /// <returns>回傳結果</returns>
        T GetExamples();
    }
}
