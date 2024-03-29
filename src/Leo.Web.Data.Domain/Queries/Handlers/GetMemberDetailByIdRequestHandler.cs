﻿// MIT License

using AutoMapper;
using Leo.Data.Domain.Dtos;
using MediatR;

namespace Leo.Web.Data.Queries.Handlers
{
    internal sealed class GetCustomerDetailByIdRequestHandler : IRequestHandler<GetCustomerDetailByIdRequest, CustomerDetailDto>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetCustomerDetailByIdRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _uow = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CustomerDetailDto> Handle(GetCustomerDetailByIdRequest request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                throw new ArgumentException(nameof(request.Id));
            }

            var detail = await _uow.CustomerDetailRepository.GetByIdAsync(request.Id).ConfigureAwait(false);
            return _mapper.Map<CustomerDetailDto>(detail);
        }
    }
}
