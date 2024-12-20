using HAN.Services.DTOs;

namespace HAN.Services.Interfaces;

public interface IFileService
{
    FileDto CreateFile(FileDto file);
}