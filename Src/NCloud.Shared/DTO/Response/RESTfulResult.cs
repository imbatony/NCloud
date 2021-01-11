namespace NCloud.Shared.DTO.Response
{
    /// <summary>
    /// Defines the <see cref="RESTfulResult{T}" />.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    public class RESTfulResult<T>
    {
        //
        // Summary:
        //     状态码
        /// <summary>
        /// Gets or sets the StatusCode.
        /// </summary>
        public int? StatusCode { get; set; }

        //
        // Summary:
        //     数据
        /// <summary>
        /// Gets or sets the Data.
        /// </summary>
        public T Data { get; set; }

        //
        // Summary:
        //     执行成功
        /// <summary>
        /// Gets or sets a value indicating whether Succeeded.
        /// </summary>
        public bool Succeeded { get; set; }

        //
        // Summary:
        //     错误信息
        /// <summary>
        /// Gets or sets the Errors.
        /// </summary>
        public object Errors { get; set; }

        //
        // Summary:
        //     附加数据
        /// <summary>
        /// Gets or sets the Extras.
        /// </summary>
        public object Extras { get; set; }

        //
        // Summary:
        //     时间戳
        /// <summary>
        /// Gets or sets the Timestamp.
        /// </summary>
        public long Timestamp { get; set; }
    }
}
