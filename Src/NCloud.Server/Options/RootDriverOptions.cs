namespace NCloud.Server.Options
{
    using Furion.ConfigurableOptions;
    using NCloud.Server.Service.Driver;

    /// <summary>
    /// Defines the <see cref="RootDriverOptions" />.
    /// </summary>
    public class RootDriverOptions : IConfigurableOptions
    {
        /// <summary>
        /// Gets or sets the Url.
        /// </summary>
        public string Url { get; set; } = "root://root";
    }
}
