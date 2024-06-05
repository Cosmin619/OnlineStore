using Microsoft.AspNetCore.Mvc;
using Moq;
using OnlineStore.Controllers;
using OnlineStore.Data;
using OnlineStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class ProductsControllerTests
{
    private readonly Mock<IRepository<ProductModel>> _mockRepo;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _mockRepo = new Mock<IRepository<ProductModel>>();
        _controller = new ProductsController(_mockRepo.Object);
    }

    [Fact]
    public async Task GetProducts_ReturnsOkResult_WithAListOfProducts()
    {
        // Arrange
        var products = new List<ProductModel>
        {
            new ProductModel { Id = 1, Name = "Product 1", Description = "Description 1", Price = 10.0m, ImageUrl = "http://example.com/image1.png" },
            new ProductModel { Id = 2, Name = "Product 2", Description = "Description 2", Price = 20.0m, ImageUrl = "http://example.com/image2.png" }
        };
        _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(products);

        // Act
        var result = await _controller.GetProducts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnProducts = Assert.IsType<List<ProductModel>>(okResult.Value);
        Assert.Equal(2, returnProducts.Count);
    }
}