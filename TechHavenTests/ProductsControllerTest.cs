﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using TechHaven.Controllers;
using TechHaven.Data;
using TechHaven.Models;
using TechHaven.Services;

namespace TechHavenTests
{
    [TestClass]
    public class ProductsControllerTest
    {
        private ProductsController productController;

        public ProductsControllerTest()
        {
            var productList = new List<Product>();
            productList.Add(new Product() { Id = 1, Model = "testModel1" });

            var mockDbSet = new Mock<DbSet<Product>>();

            mockDbSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(productList.AsQueryable().Provider);
            mockDbSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(productList.AsQueryable().Expression);
            mockDbSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(productList.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(productList.AsQueryable().GetEnumerator());

            var applicationDbContextMock = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            applicationDbContextMock.Setup(mbox  => mbox.Product).Returns(mockDbSet.Object);

            productController = new ProductsController(applicationDbContextMock.Object, null, null);
        }

        [TestMethod]
        public void TestProductExists()
        {
            Assert.IsTrue(productController.ProductExists(1));
        }

        [TestMethod]
        public void TestDecodeSortTypeHighestFirst()
        {
            Assert.AreEqual(SortType.HighestFirst, productController.DecodeSortType("HighestFirst"));
        }

        [TestMethod]
        public void TestDecodeSortTypeLowestFirst()
        {
            Assert.AreEqual(SortType.LowestFirst, productController.DecodeSortType("LowestFirst"));
        }

        [TestMethod]
        public void TestDecodeSortTypeAlphabetical()
        {
            Assert.AreEqual(SortType.Alphabetical, productController.DecodeSortType(""));
        }

    }
}
