﻿namespace NCloud.Core.Abstractions
{
    /// <summary>
    /// Defines the <see cref="IFileManagerProvider" />.
    /// </summary>
    public interface IFileManagerProvider
    {
        /// <summary>
        /// The GetFileManager.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="IFileManager"/>.</returns>
        IFileManager GreateFileManager(string url);

        /// <summary>
        /// The IsSupport.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool IsSupport(string url);
    }
}