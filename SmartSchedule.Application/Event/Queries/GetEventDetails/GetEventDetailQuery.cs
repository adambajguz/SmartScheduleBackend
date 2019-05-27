﻿namespace SmartSchedule.Application.Event.Queries.GetEventDetails
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.DTO.Event.Commands;
    using SmartSchedule.Application.Exceptions;

    public class GetEventDetailQuery : IRequest<UpdateEventRequest>
    {
        public IdRequest Data { get; set; }

        public GetEventDetailQuery()
        {

        }

        public GetEventDetailQuery(IdRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<GetEventDetailQuery, UpdateEventRequest>
        {
            private readonly IUnitOfWork _uow;

            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<UpdateEventRequest> Handle(GetEventDetailQuery request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;

                var entity = await _uow.EventsRepository.GetByIdAsync(data.Id);

                if (entity == null)
                {
                    throw new NotFoundException(nameof(Domain.Entities.Event), data.Id);
                }

                return UpdateEventRequest.Create(entity);
            }
        }
    }
}
