namespace NCloud.Server.Test
{
    using NCloud.Core.Abstractions;

    class NopeIdGenerator : IFileIdGenerator
    {
        public string DecodePath(string id)
        {
            return id;
        }

        public string EncodedPath(string path)
        {
            return path;
        }
    }
}
