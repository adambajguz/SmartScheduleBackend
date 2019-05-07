﻿namespace SmartSchedule.Application.Friends.Commands.RejectFriendRequest
{
    using FluentValidation;
    using Microsoft.EntityFrameworkCore;
    using SmartSchedule.Application.DTO.Friends.Commands;
    using SmartSchedule.Persistence;

    public class RejectFriendRequestCommandValidator : AbstractValidator<AcceptOrRejectFriendInvitationRequest>
    {
        public RejectFriendRequestCommandValidator(SmartScheduleDbContext context)
        {
            RuleFor(x => x.RequestingUserId).NotEmpty().MustAsync(async (request, val, token) =>
            {
                var friendRequest = await context.Friends.FirstOrDefaultAsync(x => (x.FirstUserId.Equals(request.RequestingUserId)
                                                                                && x.SecoundUserId.Equals(request.RequestedUserId)
                                                                                && x.Type.Equals(Domain.Enums.FriendshipTypes.pending_first_secound))
                                                                                || (x.FirstUserId.Equals(request.RequestedUserId)
                                                                                && x.SecoundUserId.Equals(request.RequestingUserId)
                                                                                && x.Type.Equals(Domain.Enums.FriendshipTypes.pending_secound_first)));

                if (friendRequest != null)
                {
                    return true;
                }

                return false;
            }).WithMessage("Friend request from that user doesn't exists");
        }
    }
}
