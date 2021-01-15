namespace NCloud.React.Options
{
    using System.Collections.Generic;
    using Furion.ConfigurableOptions;

    /// <summary>
    /// Defines the <see cref="DevelopInitFilesOptions" />.
    /// </summary>
    public class DevelopInitFilesOptions : IConfigurableOptions
    {
        /// <summary>
        /// Gets or sets the Urls.
        /// </summary>
        public List<string> Urls { get; set; }
    }
}
