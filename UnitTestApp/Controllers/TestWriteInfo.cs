using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ConsoleReadFileInfo.Controllers;
using ConsoleReadFileInfo.Model;
using Moq;
using System.IO;

namespace UnitTestApp.Controllers
{
    [TestClass]
    public class TestWriteInfo
    {
        private IWriteInfo _writeInfo;
        private const string _path = "D:\\Temp";
        private readonly string _fullpath = Path.Combine(_path, "infoFiles.xml");
        private readonly bool dirExists = Directory.Exists(_path);
        private readonly InfoFile _infoFile1 = new InfoFile(_path, "file1.txt", 123);
        private static XDocument _xdoc;
        private XElement _root;

        [TestInitialize]
        public void Initialize()
        {
            _writeInfo = Mock.Of<IWriteInfo>();
        }

        [TestMethod]
        public void WriteFileInfoTest()
        {
            // Arrange
            _ = Mock.Get(_writeInfo)
                .Setup(x => x.WriteFileInfo(It.IsAny<string>(), It.IsAny<InfoFile>()))
                .Callback(() => MoqXmlDoc());

            // Act
            _writeInfo.WriteFileInfo(_path, _infoFile1);

            // Assert
            Assert.IsNotNull(_xdoc);
            Assert.IsTrue(_xdoc.Elements().Count() == 1);
            Assert.IsTrue(_root.Elements().Count() == 1);
            Assert.IsTrue(_xdoc.Root.Name == _root.Name);
            Assert.IsTrue(_root.Elements().FirstOrDefault().Element("Name").Value == _infoFile1.Name);
            
            if (dirExists)
                Assert.IsTrue(new FileInfo(_fullpath).Exists);
        }

        private void MoqXmlDoc()
        {
            _root = new XElement("InfoFiles");

            XElement infoFileElem = new XElement("InfoFile");
            XElement nameElem = new XElement("Name", _infoFile1.Name);
            XElement lengthElem = new XElement("Length", _infoFile1.Length.ToString());

            infoFileElem.Add(nameElem);
            infoFileElem.Add(lengthElem);

            _xdoc = new XDocument();
            _root.Add(infoFileElem);
            _xdoc.Add(_root);

            if (dirExists)
                _xdoc.Save(_fullpath);
        }
    }
}
