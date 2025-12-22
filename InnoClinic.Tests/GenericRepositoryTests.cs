using AutoFixture;
using InnoClinic.DAL.Entities;
using InnoClinic.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Tests;

public class GenericRepositoryTests
{
    private readonly TaskDbContext _taskDbContext;
    
    private readonly IGenericRepository<User>  _genericRepository;

    private readonly IFixture _fixture;
    
    private readonly User _seedUser; 
    
    public GenericRepositoryTests()
    {
        _fixture = new Fixture();
        _seedUser = _fixture.Create<User>();
        _taskDbContext = GetTaskDbContext();
        _genericRepository = new GenericRepository<User>(_taskDbContext);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnEntity_WhenEntityExists()
    {
        // Arrange
        var expectedId = _seedUser.Id;

        // Act
        var result = await _genericRepository.GetByIdAsync(expectedId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedId, result.Id);
        Assert.Equal(_seedUser.Name, result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenEntityDoesNotExist()
    {
        // Arrange
        var randomId = Guid.NewGuid();

        // Act
        var result = await _genericRepository.GetByIdAsync(randomId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddEntityToDatabase()
    {
        // Arrange
        var newUser = _fixture.Create<User>(); // AutoFixture сам заполнит required string Name

        // Act
        var createdUser = await _genericRepository.CreateAsync(newUser);
        
        var userInDb = await _taskDbContext.Users.FindAsync(newUser.Id);

        // Assert
        Assert.NotNull(createdUser);
        Assert.Equal(newUser.Id, createdUser.Id);
        Assert.Equal(newUser.Name, createdUser.Name);
        Assert.NotNull(userInDb);
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyEntityInDatabase()
    {
        // Arrange
        var newName = "Updated Name Value";
        
        _seedUser.Name = newName;

        // Act
        var updatedUser = await _genericRepository.UpdateAsync(_seedUser);
        
        // Assert
        Assert.Equal(newName, updatedUser.Name);
        
        var userInDb = await _taskDbContext.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == _seedUser.Id);
            
        Assert.NotNull(userInDb);
        Assert.Equal(newName, userInDb.Name);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveEntity_WhenEntityExists()
    {
        // Arrange
        var idToDelete = _seedUser.Id;

        // Act
        var isDeleted = await _genericRepository.DeleteAsync(idToDelete);

        // Assert
        Assert.True(isDeleted);
        
        var userInDb = await _taskDbContext.Users.FindAsync(idToDelete);
        Assert.Null(userInDb); 
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenEntityDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var isDeleted = await _genericRepository.DeleteAsync(nonExistentId);

        // Assert
        Assert.False(isDeleted);
    }

    [Fact]
    public async Task GetByConditionAsync_ShouldReturnMatchingEntities()
    {
        // Arrange
        var targetName = "TargetUser";
        
        var targetUser = _fixture.Build<User>()
            .With(u => u.Name, targetName)
            .Create();
        
        _seedUser.Name = "OtherUser";
        _taskDbContext.Users.Update(_seedUser);
        
        await _taskDbContext.Users.AddAsync(targetUser);
        await _taskDbContext.SaveChangesAsync();

        // Act
        var result = await _genericRepository.GetByConditionAsync(u => u.Name == targetName);

        // Assert
        var userFromDb = Assert.Single(result); // Должен найтись ровно 1
        Assert.Equal(targetUser.Id, userFromDb.Id);
        Assert.Equal(targetName, userFromDb.Name);
    }
    
    private TaskDbContext GetTaskDbContext()
    {
        var options = new DbContextOptionsBuilder()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        var context = new TaskDbContext(options);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        context.Users.Add(_seedUser);
        context.SaveChanges();
        
        return context;
    }
}
