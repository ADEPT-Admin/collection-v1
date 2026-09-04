using ACTCore.CollectionService.Domain.Entities.Collections;
using ACTCore.CollectionService.Domain.Entities.Contracts;
using ACTCore.CollectionService.Domain.Entities.Securities;
using ACTCore.CollectionService.Infrastructure.Interface;
using AutoMapper;
using Microsoft.Data.SqlClient;
using SharedKernel.Models;
using System.Linq.Expressions;

namespace ACTCore.CollectionService.Application
{
    public class ContractService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ContractService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Contract> GetAsync(Expression<Func<Contract, bool>> filter = null, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<Contract>().GetAsync(filter, includeProperties: includeProperties, asNoTracking: asNoTracking);
        }

        public async Task<IEnumerable<Contract>> GetListAsync(Expression<Func<Contract, bool>> filter = null, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<Contract>().GetAllAsync(filter, includeProperties: includeProperties, asNoTracking: asNoTracking);
        }

        public async Task<ContractPerson> GetContractPersonAsync(Expression<Func<ContractPerson, bool>> filter = null, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<ContractPerson>().GetAsync(filter, includeProperties: includeProperties, asNoTracking: asNoTracking);
        }

        public async Task<ContractPhone> GetContractPhoneAsync(Expression<Func<ContractPhone, bool>> filter = null, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<ContractPhone>().GetAsync(filter, includeProperties: includeProperties, asNoTracking: asNoTracking);
        }

        public async Task<IEnumerable<ContractPerson>> GetListContractPersonAsync(Expression<Func<ContractPerson, bool>> filter = null, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<ContractPerson>().GetAllAsync(filter, includeProperties: includeProperties, asNoTracking: asNoTracking);
        }

        public async Task<IEnumerable<ContractPhone>> GetListContractPhoneAsync(Expression<Func<ContractPhone, bool>> filter = null, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<ContractPhone>().GetAllAsync(filter, includeProperties: includeProperties, asNoTracking: asNoTracking);
        }

        public async Task<PagedResult<ContractAddress>> GetListAddressPaginationAsync(Expression<Func<ContractAddress, bool>> filter = null,
            int pageSize = 0, int pageNumber = 1,
            string sortColumn = "", string sortDirection = "asc",
            FilterContainer filterColumns = null,
            string includeProperties = "", bool asNoTracking = true)
        {
            var repository = _unitOfWork.Repository<ContractAddress>();

            var items = await repository.GetAllPagedAsync(filter,
                pageSize: pageSize, pageNumber: pageNumber,
                sortColumn: sortColumn, sortDirection: sortDirection, filterColumns: filterColumns,
                includeProperties: includeProperties, asNoTracking: asNoTracking);

            return items;
        }

        public async Task<PagedResult<ContractPayment>> GetListPaymentPaginationAsync(Expression<Func<ContractPayment, bool>> filter = null,
            int pageSize = 0, int pageNumber = 1,
            string sortColumn = "", string sortDirection = "asc",
            FilterContainer filterColumns = null,
            string includeProperties = "", bool asNoTracking = true)
        {
            var repository = _unitOfWork.Repository<ContractPayment>();

            var items = await repository.GetAllPagedAsync(filter,
                pageSize: pageSize, pageNumber: pageNumber,
                sortColumn: sortColumn, sortDirection: sortDirection, filterColumns: filterColumns,
                includeProperties: includeProperties, asNoTracking: asNoTracking);

            return items;
        }
        public async Task<PagedResult<ContractPhone>> GetListPhonePaginationAsync(Expression<Func<ContractPhone, bool>> filter = null,
            int pageSize = 0, int pageNumber = 1,
            string sortColumn = "", string sortDirection = "asc",
            FilterContainer filterColumns = null,
            string includeProperties = "", bool asNoTracking = true)
        {
            var repository = _unitOfWork.Repository<ContractPhone>();

            var items = await repository.GetAllPagedAsync(filter,
                pageSize: pageSize, pageNumber: pageNumber,
                sortColumn: sortColumn, sortDirection: sortDirection, filterColumns: filterColumns,
                includeProperties: includeProperties, asNoTracking: asNoTracking);

            return items;
        }

        public async Task<List<Dictionary<string, object>>> GetContractOverdueSummaryAsync(string contractNo)
        {
            var repository = _unitOfWork.Repository<ContractOverdue>();
            var items = await repository.ExecuteDynamicStoredProcedureAsync(
                "sp_GetContractOverdueSummary", 
                new SqlParameter("@ContractNo", contractNo)
            );
            return items;
        }

        public async Task<IEnumerable<ContractOverdue>> GetListContractOverduesAsync(Expression<Func<ContractOverdue, bool>> filter = null, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<ContractOverdue>().GetAllAsync(filter, includeProperties: includeProperties, asNoTracking: asNoTracking);
        }
    }
}
