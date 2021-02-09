namespace NCloud.React.Options
{
    using Furion.ConfigurableOptions;

    /// <summary>
    /// Defines the <see cref="RootDriverOptions" />.
    /// </summary>
    public class RootDriverOptions : IConfigurableOptions
    {
        /// <summary>
        /// Gets or sets the Url.
        /// </summary>
        public string Url { get; set; } = "virtual://root";
    }
}
