namespace HAN.Services.Interfaces;

public interface IFileTypeWriter
{
    void WriteContent(string fileName, string content);
}