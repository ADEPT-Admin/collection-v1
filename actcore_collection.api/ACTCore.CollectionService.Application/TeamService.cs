using ACTCore.CollectionService.Domain.Entities.Masters;
using ACTCore.CollectionService.Infrastructure.Interface;
using AutoMapper;
using System.Linq.Expressions;

namespace ACTCore.CollectionService.Application
{
    public class TeamService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TeamService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Team> AddAsync(Team entity)
        {
            await _unitOfWork.Repository<Team>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Team entity)
        {
            await _unitOfWork.Repository<Team>().DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Team>> GetListAsync(Expression<Func<Team, bool>> filter = null, bool asNoTracking = true)
        {
            // Include all navigation properties
            return await _unitOfWork.Repository<Team>().GetAllAsync(
                filter,
                includeProperties: "EmployeeProfiles,ColTeamGroups,ColTeamMappingAreas",
                asNoTracking: asNoTracking
            );
        }

        public async Task<Team> GetAsync(Expression<Func<Team, bool>> filter = null, bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<Team>().GetAsync(
                filter,
                includeProperties: "EmployeeProfiles,ColTeamGroups,ColTeamMappingAreas",
                asNoTracking: asNoTracking
            );
        }

        public async Task<Team> GetByIdAsync(int teamId, bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<Team>().GetAsync(
                x => x.TeamId == teamId,
                includeProperties: "EmployeeProfiles,ColTeamGroups,ColTeamMappingAreas",
                asNoTracking: asNoTracking
            );
        }

        public async Task<Team> UpdateAsync(Team entity)
        {
            await _unitOfWork.Repository<Team>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
    }
}