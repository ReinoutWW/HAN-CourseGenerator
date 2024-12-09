﻿using HAN.Data.Entities;
using HAN.Repositories.Interfaces;
using HAN.Tests.Base;
using Microsoft.Extensions.DependencyInjection;

namespace HAN.Tests.Persistence;

public class EvlRepositoryTests : TestBase
{
    private const int SeedEvlCount = 2;
    private readonly IEvlRepository _repository;

    public EvlRepositoryTests()
    {
        _repository = ServiceProvider.GetRequiredService<IEvlRepository>();

        TestDbSeeder.SeedEvls(Context, SeedEvlCount);
    }

    [Fact]
    public void AddEvl_ShouldAddEvl()
    {
        var evl = new Evl()
        {
            Name = "Test Evl",
            Description = "Test Description",
        };
        
        var createdEvl = _repository.CreateEvl(evl);
        _repository.SaveChanges();
        
        Assert.NotNull(createdEvl);
        Assert.Equal(evl.Name, createdEvl.Name);
    }

    [Fact]
    public void CreateEvl_ShouldReturnEvl()
    {
        const int evlId = 1;
        var evl = _repository.GetEvlById(evlId);
        
        Assert.NotNull(evl);
        Assert.Equal(evlId, evl.Id);
    }

    [Fact]
    public void CreateEvl_ShouldThrowException_WhenEvlAlreadyExists()
    {
        var evl = new Evl() { Id = 1, Name = $"{Guid.NewGuid()}" };
        Assert.ThrowsAny<Exception>(() => _repository.CreateEvl(evl));        
    }

    [Fact]
    public void CreateEvl_ShouldThrowException_WhenEvlIsNull()
    {
        var expectedException = Record.Exception(() =>
        {
            _repository.CreateEvl(null!);
            _repository.SaveChanges();
        });

        Assert.NotNull(expectedException);
        Assert.IsType<ArgumentException>(expectedException);
    }

    private void AddEvlExpectValidationException(Evl newEvl)
    {
        var expectedException = Record.Exception(() =>
        {
            _repository.CreateEvl(newEvl);
            _repository.SaveChanges();
        });

        Assert.NotNull(expectedException);
        Assert.IsType<AggregateException>(expectedException);
    }
}