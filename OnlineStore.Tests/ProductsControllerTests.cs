using Microsoft.AspNetCore.Mvc;
using Moq;
using OnlineStore.Controllers;
using OnlineStore.Data;
using OnlineStore.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OnlineStore.Tests
{
    public class ProductsControllerTests
    {
        private readonly ProductsController _controller;
        private readonly Mock<IRepository<ProductModel>> _repositoryMock;
        private List<ProductModel> _products;

        public ProductsControllerTests()
        {
            _repositoryMock = new Mock<IRepository<ProductModel>>();

            _products = new List<ProductModel>
            {
                new ProductModel { Id = 1, Name = "Product 1", Description = "Description 1", Price = 10.99M, ImageUrl = "https://via.placeholder.com/150" },
                new ProductModel { Id = 2, Name = "Product 2", Description = "Description 2", Price = 20.99M, ImageUrl = "https://via.placeholder.com/150" }
            };

            _repositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(_products);
            _repositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((int id) => _products.FirstOrDefault(p => p.Id == id));
            _repositoryMock.Setup(repo => repo.Add(It.IsAny<ProductModel>())).Returns((ProductModel product) =>
            {
                product.Id = _products.Max(p => p.Id) + 1;
                _products.Add(product);
                return Task.CompletedTask;
            });
            _repositoryMock.Setup(repo => repo.Update(It.IsAny<ProductModel>())).Returns((ProductModel product) =>
            {
                var existingProduct = _products.FirstOrDefault(p => p.Id == product.Id);
                if (existingProduct != null)
                {
                    existingProduct.Name = product.Name;
                    existingProduct.Description = product.Description;
                    existingProduct.Price = product.Price;
                    existingProduct.ImageUrl = product.ImageUrl;
                }
                return Task.CompletedTask;
            });
            _repositoryMock.Setup(repo => repo.Delete(It.IsAny<int>())).Returns((int id) =>
            {
                var product = _products.FirstOrDefault(p => p.Id == id);
                if (product != null)
                {
                    _products.Remove(product);
                }
                return Task.CompletedTask;
            });

            _controller = new ProductsController(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetProducts_ReturnsAllProducts()
        {
            // Act
            var result = await _controller.GetProducts();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<ProductModel>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<List<ProductModel>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetProduct_ExistingProduct_ReturnsProduct()
        {
            // Act
            var result = await _controller.GetProduct(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ProductModel>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<ProductModel>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async Task GetProduct_NonExistingProduct_ReturnsNotFound()
        {
            // Act
            var result = await _controller.GetProduct(99);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ProductModel>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task PostProduct_ValidProduct_AddsProduct()
        {
            // Arrange
            var newProduct = new ProductModel
            {
                Name = "Product 3",
                Description = "Description 3",
                Price = 30.99M,
                ImageUrl = "https://via.placeholder.com/150"
            };

            // Act
            var result = await _controller.PostProduct(newProduct);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ProductModel>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<ProductModel>(createdAtActionResult.Value);
            Assert.Equal(newProduct.Name, returnValue.Name);

            var product = _products.FirstOrDefault(p => p.Name == newProduct.Name);
            Assert.NotNull(product);
            Assert.Equal(newProduct.Name, product.Name);
        }

        [Fact]
        public async Task PutProduct_ExistingProduct_UpdatesProduct()
        {
            // Arrange
            var updatedProduct = new ProductModel
            {
                Id = 1,
                Name = "Updated Product 1",
                Description = "Updated Description 1",
                Price = 15.99M,
                ImageUrl = "https://via.placeholder.com/150"
            };

            // Act
            var result = await _controller.PutProduct(1, updatedProduct);

            // Assert
            Assert.IsType<NoContentResult>(result);

            var product = _products.FirstOrDefault(p => p.Id == 1);
            Assert.NotNull(product);
            Assert.Equal("Updated Product 1", product.Name);
        }

        [Fact]
        public async Task PutProduct_NonExistingProduct_ReturnsNotFound()
        {
            // Arrange
            var updatedProduct = new ProductModel
            {
                Id = 99,
                Name = "Updated Product 99",
                Description = "Updated Description 99",
                Price = 15.99M,
                ImageUrl = "https://via.placeholder.com/150"
            };

            _repositoryMock.Setup(repo => repo.Update(It.IsAny<ProductModel>())).ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.PutProduct(99, updatedProduct);

            // Assert
            var actionResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_ExistingProduct_RemovesProduct()
        {
            // Act
            var result = await _controller.DeleteProduct(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            var product = _products.FirstOrDefault(p => p.Id == 1);
            Assert.Null(product);
        }

        [Fact]
        public async Task DeleteProduct_NonExistingProduct_ReturnsNotFound()
        {
            // Act
            var result = await _controller.DeleteProduct(99);

            // Assert
            var actionResult = Assert.IsType<NotFoundResult>(result);
        }
    }
}