namespace NCloud.Server.Errors
{
    using Furion.FriendlyException;

    /// <summary>
    /// Defines the ErrorCodes.
    /// </summary>
    [ErrorCodeType]
    public enum ErrorCodes
    {
        /// <summary>
        /// Defines the FILE_PATH_INVALID.
        /// </summary>
        [ErrorCodeItemMetadata("文件路径：{0} 非法", ErrorCode = "00001")]
        FILE_PATH_INVALID,
        /// <summary>
        /// Defines the FILE_NOT_FOUND.
        /// </summary>
        [ErrorCodeItemMetadata("文件未找到", ErrorCode = "00002")]
        FILE_NOT_FOUND,
        /// <summary>
        /// Defines the FILE_ACCESS_ERROR.
        /// </summary>
        [ErrorCodeItemMetadata("文件访问失败", ErrorCode = "00003")]
        FILE_ACCESS_ERROR,

        /// <summary>
        /// Defines the FILE_OPT_NOT_SUPPORT.
        /// </summary>
        [ErrorCodeItemMetadata("文件操作不支持", ErrorCode = "00004")]
        FILE_OPT_NOT_SUPPORT,
        /// <summary>
        /// Defines the SERVER_ERROR.
        /// </summary>
        [ErrorCodeItemMetadata("服务器运行异常", ErrorCode = "10000")]
        SERVER_ERROR
    }
}
