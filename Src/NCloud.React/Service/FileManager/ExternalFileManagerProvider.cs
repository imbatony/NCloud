namespace NCloud.React.Service.FileManager
{
    using System.Net.Http;
    using NCloud.Core.Abstractions;
    using NCloud.Core.Utils;

    /// <summary>
    /// Defines the <see cref="ExternalFileManagerProvider" />.
    /// </summary>
    public class ExternalFileManagerProvider : IFileManagerProvider
    {
        /// <summary>
        /// Defines the baseId.
        /// </summary>
        private readonly string baseId;

        /// <summary>
        /// Defines the fileManager.
        /// </summary>
        private readonly ExternalFileManager fileManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalFileManagerProvider"/> class.
        /// </summary>
        /// <param name="systemHelper">The systemHelper<see cref="ISystemHelper"/>.</param>
        public ExternalFileManagerProvider(ISystemHelper systemHelper)
        {
            this.baseId = systemHelper.CreateFileManagerBaseId("external://singlton");
            this.fileManager = new ExternalFileManager(baseId, systemHelper);
        }

        /// <summary>
        /// The GetFileMangerBaseIdByUrl.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetFileMangerBaseIdByUrl(string url)
        {
            return this.baseId;
        }

        /// <summary>
        /// The GetSupportType.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetSupportType()
        {
            return "external";
        }

        /// <summary>
        /// The GreateFileManager.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileManager"/>.</returns>
        public IFileManager GreateFileManager(string url, out string id)
        {
            id = baseId;
            return this.fileManager;
        }

        /// <summary>
        /// The IsSupport.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool IsSupport(string url)
        {
            return UrlUtils.GetUrlSchema(url) == GetSupportType();
        }

        /// <summary>
        /// The ToConfigUrl.
        /// </summary>
        /// <param name="message">The message<see cref="HttpRequestMessage"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string ToConfigUrl(HttpRequestMessage message)
        {
            throw new System.NotImplementedException();
        }
    }
}
