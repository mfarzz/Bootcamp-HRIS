using HRIS.API.Data.Repositories.Interfaces;
using HRIS.API.Entities;

namespace HRIS.API.Data.Repositories.Implementations;

public class PositionRepository(HrisDbContext context) : Repository<Position>(context), IPositionRepository;