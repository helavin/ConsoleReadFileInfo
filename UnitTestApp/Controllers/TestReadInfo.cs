using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleReadFileInfo.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleReadFileInfo.Model;

using Moq;

namespace ConsoleReadFileInfo.Tests
{
    [TestClass()]
    public class TestReadInfo
    {
        private static Queue<string> Pathes { get; set; } = new Queue<string>();
        private static Queue<InfoFile> InfoFiles { get; set; } = new Queue<InfoFile>();

        private IReadInfo _readInfo;
        private static readonly string _path = "D:\\Temp";
        private static readonly string _path1 = "D:\\Temp1";
        private readonly string _path2 = "D:\\Temp2";
        private readonly InfoFile _infoFile1 = new InfoFile(_path, "file1.txt", 123);
        private readonly InfoFile _infoFile2 = new InfoFile(_path, "file2.txt", 456);

        [TestInitialize]
        public void Initialize()
        {
            _readInfo = Mock.Of<IReadInfo>();


        }

        [TestMethod()]
        public void GetPathesTest()
        {
            // Arrange
            Pathes.Clear();
            Mock.Get(_readInfo)
                .Setup(x => x.GetPathes(It.IsAny<string>(), It.IsAny<Queue<string>>()))
                .Callback(() =>
                {
                    Pathes.Enqueue(_path1);
                    Pathes.Enqueue(_path2);
                });

            // Act
            _readInfo.GetPathes(_path, Pathes);

            // Assert
            Assert.IsTrue(Pathes.Count == 2);
            Assert.IsTrue(Pathes.Contains(_path1));
            Assert.IsTrue(Pathes.ElementAt(1).Equals(_path2));
        }

        [TestMethod]
        public void GetFileInfoTest()
        {
            // Arrange
            Pathes.Clear();
            Pathes.Enqueue(_path1);
            Pathes.Enqueue(_path2);

            Mock.Get(_readInfo)
                .Setup(x => x.GetFileInfo(It.IsAny<Queue<string>>(), It.IsAny<Queue<InfoFile>>()))
                .Callback(() =>
                {
                    Pathes.Dequeue();
                    InfoFiles.Enqueue(_infoFile1);
                    InfoFiles.Enqueue(_infoFile2);
                });

            // Act
            _readInfo.GetFileInfo(Pathes, InfoFiles);

            // Assert
            Assert.IsTrue(Pathes.Count == 1);
            Assert.IsTrue(Pathes.Contains(_path2));
            Assert.IsTrue(InfoFiles.Count() == 2);
            Assert.IsTrue(InfoFiles.Contains(_infoFile2));
        }

        [TestMethod]
        public void CreateInfoFileTest()
        {
            // Arrange
            Mock.Get(_readInfo)
                .Setup(x => x.CreateInfoFile(It.IsAny<string>()))
                .Returns((string x) => _infoFile1);

            //Act
            var returnInfo = _readInfo.CreateInfoFile("filePath");

            //Assert
            Assert.IsNotNull(returnInfo);
            Assert.AreEqual(_infoFile1, returnInfo);
        }

    }
}