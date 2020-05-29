using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ConsoleReadFileInfo.Tests
{
    [TestClass]
    public class TestProgram
    {
        [TestMethod]
        public void CheckPathTest()
        {
            // Arrange            
            string path = "D:\\Temp";

            // Act
            bool result = Program.CheckPath(path);

            // Assert
            Assert.IsTrue(result);
        }   
    }
}
