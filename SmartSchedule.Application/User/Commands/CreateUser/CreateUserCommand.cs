﻿namespace SmartSchedule.Application.User.Commands.CreateUser
{
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using MediatR;
    using SmartSchedule.Application.Helpers;
    using SmartSchedule.Persistence;

    public class CreateUserCommand : IRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public class Handler : IRequestHandler<CreateUserCommand, Unit>
        {
            private readonly SmartScheduleDbContext _context;

            public Handler(SmartScheduleDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                var hash = new HashedPassword(PasswordHelper.CreateHash(request.Password));
                var vResult = await new CreateUserCommandValidator(_context).ValidateAsync(request, cancellationToken);
                if (!vResult.IsValid)
                {
                    throw new ValidationException(vResult.Errors);
                }

                var entity = new Domain.Entities.User
                {
                    Email = request.Email,
                    Name = request.UserName,
                    Password = hash.ToSaltedPassword()
                };
                _context.Users.Add(entity);

                await _context.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
