using HAN.Data;
using HAN.Repositories.Interfaces;
using File = HAN.Data.Entities.File;

namespace HAN.Repositories;

public class FileRepository(AppDbContext context) : GenericRepository<File>(context), IFileRepository
{
    
}