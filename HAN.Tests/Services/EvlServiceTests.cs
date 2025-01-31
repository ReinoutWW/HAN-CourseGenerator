﻿using HAN.Services;
using HAN.Services.DTOs;
using HAN.Services.Interfaces;
using HAN.Tests.Base;
using Microsoft.Extensions.DependencyInjection;

namespace HAN.Tests.Services;

public class EvlServiceTests: TestBase
{
    private readonly IEvlService _evlService;
    
    public EvlServiceTests()
    {
        _evlService = ServiceProvider.GetRequiredService<IEvlService>();
    }

    [Fact]
    public void CreateEvl_ShouldCreateEvl()
    {
        EvlDto evl = new()
        {
            Name = "Test Evl",
            Description = "Test Description",
        };

        var createdEvl = _evlService.CreateEvl(evl);
        
        Assert.NotNull(createdEvl);
        Assert.Equal(evl.Name, createdEvl.Name);
        Assert.Equal(evl.Description, createdEvl.Description);
    }

    [Fact]
    public void CreateEvl_ShouldThrowException_WhenNameIsNull()
    {   
        Assert.Throws<ArgumentNullException>(() => _evlService.CreateEvl(null!));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(".")]
    [InlineData("industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of sheets containing Lorem Ipsum passages, and more rec")]
    [InlineData(null)]
    public void CreateEvl_ShouldThrowException_WhenNameIsInvalid(string invalidName)
    {
        EvlDto evl = new()
        {
            Name = invalidName,
            Description = "Test Description"
        };
        CreateEvlExpectValidationException(evl);
    }

    [Theory]
    [InlineData("\"Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur?\"\n\n1914 translation by H. Rackham\n\"But I must explain to you how all this mistaken idea of denouncing pleasure and praising pain was born and I will give you a complete account of the system, and expound the actual teachings of the great explorer of the truth, the master-builder of human happiness. No one rejects, dislikes, or avoids pleasure itself, because it is pleasure, but because those who do not know how to pursue pleasure rationally encounter consequences that are extremely painful. Nor again is there anyone who loves or pursues or desires to obtain pain of itself, because it is pain, but because occasionally circumstances occur in which toil and pain can procure him some great pleasure. To take a trivial example, which of us ever undertakes laborious physical exercise, except to obtain some advantage from it? But who has any right to find fault with a man who chooses to enjoy a pleasure that has no annoying consequences, or one who avoids a pain that produces no resultant pleasure?\"")]
    public void CreateEvl_ShouldThrowException_WhenDescriptionIsInvalid(string invalidDescription)
    {
        EvlDto evl = new()
        {
            Name = "Valid name",
            Description = invalidDescription
        };
        
        CreateEvlExpectValidationException(evl);
    }

    private void CreateEvlExpectValidationException(EvlDto evl)
    {
        var expectedException = Record.Exception(() =>
        {
            _evlService.CreateEvl(evl);
        });

        Assert.NotNull(expectedException);
        Assert.IsType<HAN.Services.Exceptions.ValidationException>(expectedException);
    }
}