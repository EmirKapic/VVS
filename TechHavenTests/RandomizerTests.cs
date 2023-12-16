using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechHaven.Models;
using TechHaven.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

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
            // Read data from JSON file
            try
            {
                string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "products.json");
                string jsonContent = File.ReadAllText(jsonFilePath);
                products = JsonSerializer.Deserialize<List<Product>>(jsonContent);
                randomizer = new Randomizer(products);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
            }

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

