namespace NCloud.Server.Test
{
    using System;
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NCloud.Server.Service.FileManager;

    [TestClass]
    public class LocalFileManagerTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var fileManager = new LocalFileManager(new NopeIdGenerator(), Path.GetTempPath(),"local://测试");
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
