using HAN.Services.DTOs;

namespace HAN.Services;

public interface IFileService
{
    FileDto CreateFile(FileDto file);
}