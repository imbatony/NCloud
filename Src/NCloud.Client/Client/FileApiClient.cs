namespace NCloud.Shared.Client
{
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using NCloud.Shared.DTO.Response;

    /// <summary>
    /// Defines the <see cref="FileApiClient" />.
    /// </summary>
    public class FileApiClient
    {
        /// <summary>
        /// Defines the httpClient.
        /// </summary>
        private readonly HttpClient httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileApiClient"/> class.
        /// </summary>
        /// <param name="httpClient">The httpClient<see cref="HttpClient"/>.</param>
        public FileApiClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        /// <summary>
        /// The GetRootAsync.
        /// </summary>
        /// <returns>The <see cref="Task{ RESTfulResult{ListFileResponse}}"/>.</returns>
        public Task<RESTfulResult<ListFileResponse>> GetRootAsync()
        {
            return this.httpClient.GetFromJsonAsync<RESTfulResult<ListFileResponse>>("/api/file/root");
        }

        /// <summary>
        /// The GetByBaseIdAndId.
        /// </summary>
        /// <param name="baseId">The baseId<see cref="string"/>.</param>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{RESTfulResult{ListFileResponse}}"/>.</returns>
        public Task<RESTfulResult<ListFileResponse>> GetByBaseIdAndId(string baseId, string id)
        {
            return this.httpClient.GetFromJsonAsync<RESTfulResult<ListFileResponse>>($"/api/file/files/{baseId}/{id}");
        }
    }
}
