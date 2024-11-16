using Moq;
using Xunit;
using RecipeBookMvc.Repositories.Implementation;
using RecipeBookMvc.Models.Domain;
using RecipeBookMvc.Models.DTO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public class RecipeServiceTests
{
    private readonly Mock<DatabaseContext> _mockContext;
    private readonly RecipeService _service;

    public RecipeServiceTests()
    {
        _mockContext = new Mock<DatabaseContext>();
        _service = new RecipeService(_mockContext.Object);
    }
    [Fact]
    public void Add_ValidRecipe_ReturnsTrue()
    {
        var recipe = new Recipe { Id = 1, Title = "Cake", Categorys = new List<int> { 1, 2 },Ingredients="test",Instruction="test",Complexity="2112" };
        _mockContext.Setup(ctx => ctx.Recipe.Add(It.IsAny<Recipe>()));
        _mockContext.Setup(ctx => ctx.SaveChanges()).Returns(1);
        var result = _service.Add(recipe);
        Assert.True(result);
        _mockContext.Verify(ctx => ctx.Recipe.Add(It.IsAny<Recipe>()), Times.Once);
        _mockContext.Verify(ctx => ctx.SaveChanges(), Times.Exactly(2)); 
    }
    [Fact]
    public void Delete_ExistingRecipe_ReturnsTrue()
    {
        // Arrange
        var recipe = new Recipe { Id = 1, Title = "Cake" };
        _mockContext.Setup(ctx => ctx.Recipe.Find(1)).Returns(recipe);
        _mockContext.Setup(ctx => ctx.SaveChanges()).Returns(1);

        // Act
        var result = _service.Delete(1);

        // Assert
        Assert.True(result);
        _mockContext.Verify(ctx => ctx.Recipe.Remove(It.IsAny<Recipe>()), Times.Once);
        _mockContext.Verify(ctx => ctx.SaveChanges(), Times.Once);
    }
    [Fact]
    public void GetById_ExistingId_ReturnsRecipe()
    {
        // Arrange
        var recipe = new Recipe { Id = 1, Title = "Cake" };
        _mockContext.Setup(ctx => ctx.Recipe.Find(1)).Returns(recipe);

        // Act
        var result = _service.GetById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Cake", result.Title);
    }
    [Fact]
    public void List_ReturnsRecipeList()
    {
        // Arrange
        var recipes = new List<Recipe>
    {
        new Recipe { Id = 1, Title = "Cake" },
        new Recipe { Id = 2, Title = "Pie" }
    }.AsQueryable();

        var mockSet = new Mock<DbSet<Recipe>>();
        mockSet.As<IQueryable<Recipe>>().Setup(m => m.Provider).Returns(recipes.Provider);
        mockSet.As<IQueryable<Recipe>>().Setup(m => m.Expression).Returns(recipes.Expression);
        mockSet.As<IQueryable<Recipe>>().Setup(m => m.ElementType).Returns(recipes.ElementType);
        mockSet.As<IQueryable<Recipe>>().Setup(m => m.GetEnumerator()).Returns(recipes.GetEnumerator());

        _mockContext.Setup(ctx => ctx.Recipe).Returns(mockSet.Object);

        // Act
        var result = _service.List();

        // Assert
        Assert.Equal(2, result.RecipeList.Count());
    }
}
