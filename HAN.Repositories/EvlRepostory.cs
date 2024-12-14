using HAN.Data;
using HAN.Data.Entities;
using HAN.Repositories.Interfaces;

namespace HAN.Repositories;

public class EvlRepostory(AppDbContext context) : GenericRepository<Evl>(context), IEvlRepository
{ }