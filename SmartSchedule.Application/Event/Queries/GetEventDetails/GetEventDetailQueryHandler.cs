﻿namespace SmartSchedule.Application.Event.Queries.GetEventDetails
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Persistence;

    public class GetEventDetailQueryHandler : IRequestHandler<GetEventDetailQuery, EventDetailModel>
    {
        private readonly SmartScheduleDbContext _context;

        public GetEventDetailQueryHandler(SmartScheduleDbContext context)
        {
            _context = context;
        }
        public async Task<EventDetailModel> Handle(GetEventDetailQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Events.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Event), request.Id);
            }

            return EventDetailModel.Create(entity);
        }
    }
}
