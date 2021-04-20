namespace NCloud.Plugins.GitHub
{
    using System.Net.Http;
    using NCloud.Core.Abstractions;
    using NCloud.Core.Utils;

    /// <summary>
    /// Defines the <see cref="GitHubFileManagerProvider" />.
    /// </summary>
    public class GitHubFileManagerProvider : IFileManagerProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubFileManagerProvider"/> class.
        /// </summary>
        /// <param name="helper">The helper<see cref="ISystemHelper"/>.</param>
        /// <param name="gitHubClient">The gitHubClient<see cref="GitHubClient"/>.</param>
        public GitHubFileManagerProvider(ISystemHelper helper, GitHubClient gitHubClient)
        {
            this.helper = helper;
            this.gitHubClient = gitHubClient;
        }

        /// <summary>
        /// Defines the helper.
        /// </summary>
        private readonly ISystemHelper helper;

        /// <summary>
        /// Defines the gitHubClient.
        /// </summary>
        private readonly GitHubClient gitHubClient;

        /// <summary>
        /// The GetFileMangerBaseIdByUrl.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetFileMangerBaseIdByUrl(string url)
        {
            return helper.CreateFileManagerBaseId(url);
        }

        /// <summary>
        /// The GreateFileManager.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileManager"/>.</returns>
        public IFileManager GreateFileManager(string url, out string id)
        {
            var project = UrlUtils.GetParam(url, "project");
            var owner = UrlUtils.GetParam(url,"owner");
            var displayName = helper.GetFileManagerDisplayName(url);
            id = helper.CreateFileManagerBaseId(url);
            return new GitHubFileManager(helper, gitHubClient, displayName, project, owner, id, helper.GetParentBaseId(url), helper.GetParentId(url));
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
        /// The GetSupportType.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetSupportType()
        {
            return "github";
        }
    }
}
