﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartSchedule.Application.Exceptions;
using SmartSchedule.Persistence;

namespace SmartSchedule.Application.Friends.Commands.RemoveFriendRequest
{
    public class RemoveFriendCommand : IRequest
    {
        public int FriendId { get; set; }
        public int UserId { get; set; }

        public class Handler : IRequestHandler<RemoveFriendCommand, Unit>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(RemoveFriendCommand request, CancellationToken cancellationToken)
            {
                var vResult = await new RemoveFriendCommandValidator().ValidateAsync(request, cancellationToken);
                if (!vResult.IsValid)
                {
                    throw new FluentValidation.ValidationException(vResult.Errors);
                }

                var friendRequest = await _context.Friends.FirstOrDefaultAsync(x => (x.FirstUserId.Equals(request.UserId)
                                                                                && x.SecoundUserId.Equals(request.FriendId))
                                                                                || (x.FirstUserId.Equals(request.FriendId)
                                                                                && x.SecoundUserId.Equals(request.UserId)));

                if (friendRequest == null || !friendRequest.Type.Equals(Domain.Enums.FriendshipTypes.friends))
                {
                    throw new NotFoundException("FriendRequest", request.FriendId);
                }
                if (friendRequest != null)
                {
                    _context.Friends.Remove(friendRequest);
                }
                await _context.SaveChangesAsync();

                return await Unit.Task;
            }
        }
    }
}