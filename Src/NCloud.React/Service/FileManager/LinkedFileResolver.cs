namespace NCloud.React.Service.FileManager
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using NCloud.Core.Abstractions;
    using NCloud.Core.Model;
    using NCloud.Core.Utils;

    /// <summary>
    /// Defines the <see cref="LinkedFileResolver" />.
    /// </summary>
    public class LinkedFileResolver
    {
        /// <summary>
        /// Defines the provider.
        /// </summary>
        private readonly IServiceProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkedFileResolver"/> class.
        /// </summary>
        /// <param name="provider">The provider<see cref="IServiceProvider"/>.</param>
        public LinkedFileResolver(IServiceProvider provider)
        {
            this.provider = provider;
        }

        /// <summary>
        /// The ResolveLinkedFile.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="getContent">The getContent<see cref="Func{string}"/>.</param>
        /// <param name="extension">The extension<see cref="string"/>.</param>
        /// <param name="baseId">The baseId<see cref="string"/>.</param>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <param name="info">The info<see cref="NCloudFileInfo"/>.</param>
        /// <returns>The <see cref="NCloudFileInfo"/>.</returns>
        public bool TryResolveLinkedFile(string name, Func<string> getContent, string extension, string baseId, string id, out NCloudFileInfo info)
        {
            var fileManagerFactory = provider.GetService<IFileManagerFactory>();
            var helper = provider.GetService<ISystemHelper>();
            try
            {
                bool resovled = false;
                if (extension == ".nln")
                {
                    string url = getContent();
                    string connetion = UrlUtils.HasQuery(url) ? "&" : "?";
                    var fileManager = fileManagerFactory.GetFileManagerByUrl($"{url}{connetion}parentBaseId={baseId}&parentId=${id}&displayName={name}");
                    info = fileManager.GetFileById(fileManager.GetRootId());
                    resovled = true;
                }
                else if (extension == ".url")
                {
                    string content = getContent();
                    var parser = new IniParser.Parser.IniDataParser();
                    var inicontent = parser.Parse(content);
                    string url = inicontent["InternetShortcut"]["URL"];
                    string connetion = UrlUtils.HasQuery(url) ? "&" : "?";
                    var fileManager = fileManagerFactory.GetFileManagerByUrl("external://singlton");
                    string innerUrl = $"{url}{connetion}_npbid={baseId}&_npid=${id}&_nd={name}";
                    info = fileManager.GetFileById(helper.GetFileIdByPath(innerUrl));
                    resovled = true;
                }
                else
                {
                    resovled = false;
                    info = null;
                }
                return resovled;
            }
            catch (Exception)
            {
                info = null;
                return false;
            }
        }
    }
}
