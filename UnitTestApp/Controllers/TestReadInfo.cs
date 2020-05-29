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
        private readonly string _path = "D:\\Temp";
        private readonly string _path1 = "D:\\Temp1";
        private readonly string _path2 = "D:\\Temp2";

        [TestInitialize]
        public void Initialize()
        {
            _readInfo = Mock.Of<IReadInfo>();

            Mock.Get(_readInfo)
                .Setup(x => x.GetPathes(It.IsAny<string>(), It.IsAny<Queue<string>>()))
                .Callback(() =>
                {
                    Pathes.Enqueue(_path1);
                    Pathes.Enqueue(_path2);
                });
        }

        [TestMethod()]
        public void GetPathesTest()
        {
            // Act
            _readInfo.GetPathes(_path, Pathes);

            // Assert
            Assert.IsTrue(Pathes.Count == 2);
            Assert.IsTrue(Pathes.Contains(_path1));
            Assert.IsTrue(Pathes.ElementAt(1).Equals(_path2));
        }
        
    }
}