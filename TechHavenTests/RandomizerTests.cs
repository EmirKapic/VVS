using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechHaven.Models;
using TechHaven.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TechHaven.Tests.Services
{
    [TestClass]
    public class RandomizerTests
    {
        private Randomizer randomizer;
        private List<Product> products;

        [TestInitialize]
        public void Setup()
        {
            products = new List<Product>
            {
                new Product { Model = "Product1" },
                new Product { Model = "Product2" },
                new Product { Model = "Product3" }
            };
            randomizer = new Randomizer(products);
        }

        [TestMethod]
        public void TestRandomizerRandomization()
        {
            // Act
            var randomizedProducts1 = randomizer.GetRandomized();
            var randomizedProducts2 = randomizer.GetRandomized();

            // Assert
            CollectionAssert.AreEquivalent(products, randomizedProducts1, "Randomized list should contain the same elements as the original list.");
            CollectionAssert.AreEquivalent(products, randomizedProducts2, "Randomized list should contain the same elements as the original list.");
        }
    }
}

