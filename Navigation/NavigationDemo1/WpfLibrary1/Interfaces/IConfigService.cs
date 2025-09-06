namespace Common.Core.Interfaces {
    public interface IConfigService {
        /// <summary>
        /// 加载配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        Task<T> LoadAsync<T>(string fileName) where T : class, new();

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="fileName">文件名</param>
        /// <param name="config">配置实例</param>
        Task SaveAsync<T>(string fileName, T config) where T : class, new();

        /// <summary>
        /// 更新配置：先读，修改后再保存
        /// </summary>
        Task UpdateAsync<T>(string fileName, Action<T> updater) where T : class, new();

        // 🔽 新增 XML 支持
        Task<T> LoadXmlAsync<T>(string fileName) where T : class, new();
        Task SaveXmlAsync<T>(string fileName, T config) where T : class, new();
        Task UpdateXmlAsync<T>(string fileName, Action<T> updater) where T : class, new();
	}
}
