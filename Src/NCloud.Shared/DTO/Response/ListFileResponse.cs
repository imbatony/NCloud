namespace NCloud.Shared.DTO.Response
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
        public NCloudFileInfo CWD { get; set; }

        /// <summary>
        /// Gets or sets the Children.
        /// </summary>
        public IEnumerable<NCloudFileInfo> Children { get; set; }
    }
}
