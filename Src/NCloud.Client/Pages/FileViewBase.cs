namespace NCloud.Client.Pages
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AntDesign;
    using Microsoft.AspNetCore.Components;
    using NCloud.Client.Display;
    using NCloud.Shared.Client;
    using NCloud.Shared.DTO.Response;

    /// <summary>
    /// Defines the <see cref="FileViewBase" />.
    /// </summary>
    public class FileViewBase : AntComponentBase
    {
        /// <summary>
        /// Gets or sets the BaseId.
        /// </summary>
        [Parameter]
        public string BaseId { get; set; }

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        [Parameter]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the ParentId.
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// Gets or sets the ParentBaseId.
        /// </summary>
        public string ParentBaseId { get; set; }

        /// <summary>
        /// Defines the response.
        /// </summary>
        public ListFileResponse response;

        /// <summary>
        /// Defines the loading.
        /// </summary>
        public bool loading = true;

        /// <summary>
        /// Defines the fileList.
        /// </summary>
        public List<FileDisplayInfo> fileList = new List<FileDisplayInfo>();

        /// <summary>
        /// Defines the Cwd.
        /// </summary>
        public FileDisplayInfo Cwd = new FileDisplayInfo();

        /// <summary>
        /// Gets or sets the FileApiClient
        /// Defines the fileApiClient.......
        /// </summary>
        [Inject]
        public FileApiClient FileApiClient { get; set; }

        /// <summary>
        /// Gets or sets the NavigationManager
        /// Defines the navigationManager.......
        /// </summary>
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Gets or sets the MessageService.
        /// </summary>
        [Inject]
        public MessageService MessageService { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsRoot.
        /// </summary>
        public bool IsRoot { get; set; } = false;

        /// <summary>
        /// The OnInitializedAsync.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        protected override async Task OnInitializedAsync()
        {
            await Reload(this.BaseId, this.Id);
        }

        /// <summary>
        /// The ItemClick.
        /// </summary>
        /// <param name="item">The item<see cref="FileDisplayInfo"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task ItemClick(FileDisplayInfo item)
        {
            if (item.Directory)
            {
                await Reload(item.BaseId, item.Id);
            }
        }

        /// <summary>
        /// The BackwardClick.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task BackwardClick()
        {
            await Reload(this.ParentBaseId, this.ParentId);
        }

        /// <summary>
        /// The Reload.
        /// </summary>
        /// <param name="baseId">The baseId<see cref="string"/>.</param>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task Reload(string baseId = null, string id = null)
        {

            RESTfulResult<ListFileResponse> res;
            if (string.IsNullOrEmpty(baseId) || string.IsNullOrEmpty(id))
            {
                res = await FileApiClient.GetRootAsync();
            }
            else
            {
                res = await FileApiClient.GetByBaseIdAndId(baseId, id);
            }
            if (!res.Succeeded)
            {
                await this.MessageService.Error((string)res.Errors);
            }
            else
            {
                this.BaseId = baseId;
                this.Id = id;
                this.response = res.Data;
                this.Cwd = new FileDisplayInfo(res.Data.CWD);
                this.fileList = res.Data.Children.Select(a => new FileDisplayInfo(a)).ToList();
                this.IsRoot = (string.IsNullOrEmpty(this.Cwd.ParentId)) || this.Cwd.ParentId == this.Cwd.Id;
                this.ParentBaseId = this.Cwd.ParentBaseId;
                this.ParentId = this.Cwd.ParentId;
                this.loading = false;
                this.NavigationManager.NavigateTo($"./files/{baseId}/{id}");
            }

        }
    }
}
