using HAN.Services.DTOs;

namespace HAN.Tests.Base;

public class CourseDtoBuilder
{
    private CourseDto _courseDto;
    
    public CourseDtoBuilder()
    {
        _courseDto = new CourseDto();
    }
    
    public CourseDtoBuilder WithId(int id)
    {
        _courseDto.Id = id;
        return this;
    }
    
    public CourseDtoBuilder WithName(string name)
    {
        _courseDto.Name = name;
        return this;
    }
    
    public CourseDtoBuilder WithDescription(string description)
    {
        _courseDto.Description = description;
        return this;
    }
    
    public CourseDtoBuilder WithEvls(List<EvlDto> evls)
    {
        _courseDto.Evls = evls;
        return this;
    }
    
    public CourseDtoBuilder WithCreatedEvls(int count)
    {
        var evls = new List<EvlDto>();
        for (int i = 0; i < count; i++)
        {
            var evl = DbEntityCreator<EvlDto>.CreateEntity();
            evls.Add(evl);
        }
        _courseDto.Evls = evls;
        return this;
    }
    
    public CourseDto Build()
    {
        return _courseDto;
    }
}