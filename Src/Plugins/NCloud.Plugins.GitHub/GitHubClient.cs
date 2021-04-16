namespace NCloud.Plugins.GitHub
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    /// <summary>
    /// Defines the <see cref="GitHubClient" />.
    /// </summary>
    public class GitHubClient
    {
        /// <summary>
        /// Defines the client.
        /// </summary>
        private readonly HttpClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubClient"/> class.
        /// </summary>
        /// <param name="httpClient">The httpClient<see cref="HttpClient"/>.</param>
        public GitHubClient(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("https://api.github.com/");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
            httpClient.DefaultRequestHeaders.Add("User-Agent", "NCloud");
            this.client = httpClient;
        }

        /// <summary>
        /// The GetData.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <param name="owner">The owner<see cref="string"/>.</param>
        /// <param name="repo">The repo<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{string}"/>.</returns>
        public async Task<(GitHubFileContent, bool)> GetFile(string path, string owner, string repo)
        {
            var content = await client.GetStringAsync($"/repos/{owner}/{repo}/contents/{path}");
            if (string.IsNullOrEmpty(content))
            {
                return (null, false);
            }
            else
            {
                var isObject = content.StartsWith('{');
                if (isObject)
                {
                    var github = JsonConvert.DeserializeObject<GitHubFileContent>(content);
                    DecodeContent(github);
                    return (github, true);
                }
                else
                {
                    return (null, false);
                }
            }
        }

        /// <summary>
        /// The GetData.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <param name="owner">The owner<see cref="string"/>.</param>
        /// <param name="repo">The repo<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{string}"/>.</returns>
        public async Task<List<GitHubFileContent>> GetFiles(string path, string owner, string repo)
        {
            var content = await client.GetStringAsync($"/repos/{owner}/{repo}/contents/{path}");
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }
            else
            {
                var isObject = content.StartsWith('{');
                if (isObject)
                {
                    return null;
                }
                else
                {
                    var list = JsonConvert.DeserializeObject<List<GitHubFileContent>>(content);
                    list.ForEach(github =>
                    {
                        DecodeContent(github);
                    });
                    return list;
                }
            }
        }

        /// <summary>
        /// The DecodeContent.
        /// </summary>
        /// <param name="content">The content<see cref="GitHubFileContent"/>.</param>
        public static void DecodeContent(GitHubFileContent content)
        {
            if (!string.IsNullOrEmpty(content.Content))
            {
                byte[] c = Convert.FromBase64String(content.Content);
                var str = Encoding.Default.GetString(c);
                content.Content = str;
            }
        }
    }
}
