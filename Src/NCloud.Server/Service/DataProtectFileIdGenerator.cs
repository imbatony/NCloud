namespace NCloud.Server.Service
{
    using Microsoft.AspNetCore.DataProtection;
    using NCloud.Core.Abstractions;

    /// <summary>
    /// Defines the <see cref="DataProtectFileIdGenerator" />.
    /// </summary>
    public class DataProtectFileIdGenerator : IFileIdGenerator
    {
        /// <summary>
        /// Defines the dataProtector.
        /// </summary>
        private readonly IDataProtector dataProtector;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProtectFileIdGenerator"/> class.
        /// </summary>
        /// <param name="dataProtectorProvider">The dataProtector<see cref="IDataProtectionProvider"/>.</param>
        public DataProtectFileIdGenerator(IDataProtectionProvider dataProtectorProvider)
        {
            this.dataProtector = dataProtectorProvider.CreateProtector(nameof(DataProtectFileIdGenerator));
        }

        /// <summary>
        /// The DecodePath.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string DecodePath(string id)
        {
            return dataProtector.Unprotect(id);
        }

        /// <summary>
        /// The EncodedPath.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string EncodedPath(string path)
        {
            return dataProtector.Protect(path);
        }
    }
}
