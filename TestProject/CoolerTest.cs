using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassLibrary;
using System;

namespace TestProject
{
    [TestClass]
    public class CoolerTest
    {
        // Arrange 
        Cooler cooler = new Cooler();      

        [TestMethod]
        [DataRow(100, 100, true)]  // int capacity, int storage, bool excepted
        [DataRow(101, 100, false)]
        //[DataRow(100, 101, false)]
        public void CoolerIsFullTest(int capacity, int storage, bool excepted)
        {
            // Act
            cooler.Capacity = capacity;
            cooler.Storage = storage;
            var actual = cooler.CoolerIsFull();

            // Assert
            Assert.AreEqual(excepted, actual);           
        }

        //[TestMethod]
        //[ExpectedException(typeof(ArgumentException))]  // must be the same as the cooler class
        //public void IsStorageMoreThanCapacityTest()
        //{
        //    // Act
        //    cooler.Capacity = 100;
        //    cooler.Storage = 101;
        //}

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]  // must be the same as the cooler class
        public void IsStorageAndCapacityLessThan0Test()
        {
            // Act
            cooler.Storage = -1;
            cooler.Capacity = -1;
        }


        [TestMethod]
        [DataRow(100, 1, 2)]  // int capacity, int storage, int excepted (new Storage)
        [DataRow(100, 99, 100)]
        public void AddWineTest(int capacity, int storage, int excepted)
        {
            // Act
            cooler.Capacity = capacity;
            cooler.Storage = storage;
            var actual = cooler.AddWine();

            // Assert
            Assert.AreEqual(excepted, actual);            
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]  // the type of Exception should be the same as in Lib
        public void AddWineExceptionTest()
        {
            // Act
            cooler.Capacity = 100;
            cooler.Storage = 100;
            cooler.AddWine();

            // Assert don't need, because of [ExpectedException(typeof(Exception))] 
        }

        [TestMethod]
        [DataRow(16, "Warning: The temp is greater than 15.")]  // int temp, string info expected
        [DataRow(4, "Warning: The temp is less than 5.")]
        [DataRow(15, "")]
        [DataRow(5, "")]

        public void isTempMoreThan15Test(int temp, string expected)
        {
            // Act
            var actual = cooler.CheckTemp(temp);

            // Assert
            Assert.AreEqual(expected, actual);
        }

    }
}
