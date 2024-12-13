using HAN.Data.Entities;
using HAN.Repositories;
using HAN.Repositories.Interfaces;
using HAN.Tests.Base;
using Microsoft.Extensions.DependencyInjection;

namespace HAN.Tests.Persistence;

public class GenericRepositoryTests : TestBase
{
    private readonly IGenericRepository<ExampleEntity> _repository;

    public GenericRepositoryTests()
    {
        _repository = ServiceProvider.GetRequiredService<IGenericRepository<ExampleEntity>>();
    }

    [Fact]
    public void CreateEntity_ShouldCreateEntity()
    {
        var entity = new ExampleEntity()
        {
            Name = "TestName"
        };

        _repository.Add(entity);

        Assert.NotEqual(0, entity.Id);
    }

    [Fact]
    public void GetEntityById_ShouldReturnEntity()
    {
        var entity = new ExampleEntity()
        {
            Name = "TestName"
        };

        _repository.Add(entity);

        var createdEntity = _repository.GetById(entity.Id);
        
        Assert.NotNull(createdEntity);
        Assert.Equal(entity.Id, createdEntity.Id);
    }
    
    [Fact]
    public void GetEntityById_ShouldReturnNull_WhenEntityDoesNotExist()
    {
        var entity = _repository.GetById(0);
        
        Assert.Null(entity);
    }
    
    [Fact]
    public void DeleteEntity_ShouldDeleteEntity()
    {
        var entity = new ExampleEntity()
        {
            Name = "TestName"
        };

        _repository.Add(entity);
        _repository.Delete(entity);

        var deletedEntity = _repository.GetById(entity.Id);
        
        Assert.Null(deletedEntity);
    }
    
    [Fact]
    public void UpdateEntity_ShouldUpdateEntity()
    {
        var entity = new ExampleEntity()
        {
            Name = "TestName"
        };

        _repository.Add(entity);

        entity.Name = "UpdatedName";
        _repository.Update(entity);

        var updatedEntity = _repository.GetById(entity.Id);
        
        Assert.NotNull(updatedEntity);
        Assert.Equal(entity.Name, updatedEntity.Name);
    }

    [Fact]
    public void GetAll_ShouldReturnAll()
    {   
        var entities = new List<ExampleEntity>
        {
            new ExampleEntity { Name = "TestName1" },
            new ExampleEntity { Name = "TestName2" },
            new ExampleEntity { Name = "TestName3" }
        };

        entities.ForEach(entity => _repository.Add(entity));

        var allEntities = _repository.GetAll().ToList();
        
        Assert.NotNull(allEntities);
        Assert.Equal(entities.Count, allEntities.Count);
    }
}