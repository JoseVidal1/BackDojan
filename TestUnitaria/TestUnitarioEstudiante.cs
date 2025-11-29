using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DAL;
using System;


namespace TestUnitaria
{
    [TestClass]
    public sealed class TestUnitarioEstudiante
    {
        [TestMethod]
        public void TestRegistroEstudiante()
        {
            var mockEstudiante = new Mock<IDBEstudiante>();
        }
    }
}
