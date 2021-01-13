namespace NCloud.Server.Test
{
    using System;
    using System.IO;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using NCloud.Core.Abstractions;
    using NCloud.Server.Service.FileManager;

    /// <summary>
    /// Defines the <see cref="LocalFileManagerTest" />.
    /// </summary>
    [TestClass]
    public class LocalFileManagerTest
    {
        /// <summary>
        /// The TestMethod1.
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            var fileManager = new LocalFileManager(new Mock<ISystemHelper>().Object, Path.GetTempPath(), "测试","", "", "", new Mock<ILogger<LocalFileManager>>().Object);
            var rootId = fileManager.GetRootId();
            var files = fileManager.GetFiles(rootId);

            foreach (var file in files)
            {
                Console.WriteLine($"Name:{file.Name}");
                Console.WriteLine($"\tSize:{file.Size}");
                Console.WriteLine($"\tId:{file.Id}");
                Console.WriteLine($"\tParentId:{file.ParentId}");
                Console.WriteLine($"\tExt:{file.Ext}");
                Console.WriteLine($"\tType:{file.Type}");
            }
        }
    }
}
