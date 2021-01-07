namespace NCloud.Server.Errors
{
    using Furion.FriendlyException;

    /// <summary>
    /// Defines the ErrorCodes.
    /// </summary>
    public enum ErrorCodes
    {
        /// <summary>
        /// Defines the FILE_NOT_FOUND.
        /// </summary>
        [ErrorCodeItemMetadata("文件未找到", ErrorCode = "NotFound")]
        FILE_NOT_FOUND,
        /// <summary>
        /// Defines the FILE_ACCESS_ERROR.
        /// </summary>
        [ErrorCodeItemMetadata("文件访问失败", ErrorCode = "Error")]
        FILE_ACCESS_ERROR,
        /// <summary>
        /// Defines the SERVER_ERROR.
        /// </summary>
        [ErrorCodeItemMetadata("服务器运行异常", ErrorCode = "Error")]
        SERVER_ERROR
    }
}
