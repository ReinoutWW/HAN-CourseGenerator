using AutoMapper;
using HAN.Repositories.Interfaces;
using HAN.Services.Attributes;
using HAN.Services.DTOs;
using HAN.Services.Interfaces;
using HAN.Services.Validation;
using File = HAN.Data.Entities.File;

namespace HAN.Services;

public class FileService(IFileRepository fileRepository, IMapper mapper, IValidationService validationService) : IFileService
{
    [ValidateEntities]
    public FileDto CreateFile(FileDto file)
    {
        validationService.Validate(file);
        
        var courseEntity = mapper.Map<File>(file);
        
        fileRepository.Add(courseEntity);

        return mapper.Map<FileDto>(courseEntity);
    }
}