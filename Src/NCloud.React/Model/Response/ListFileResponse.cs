namespace NCloud.React.Model.Response
{
    using System.Collections.Generic;
    using NCloud.Core.Model;

    /// <summary>
    /// Defines the <see cref="ListFileResponse" />.
    /// </summary>
    public class ListFileResponse
    {
        /// <summary>
        /// Gets or sets the CWD.
        /// </summary>
        public NCloudFileInfo Cwd { get; set; }

        /// <summary>
        /// Gets or sets the Children.
        /// </summary>
        public List<NCloudFileInfo> Children { get; set; }
    }
}
