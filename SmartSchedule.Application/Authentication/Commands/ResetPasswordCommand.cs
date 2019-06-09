﻿namespace SmartSchedule.Application.Authentication.Commands.ResetPassword
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Application.Helpers;
    using SmartSchedule.Application.Interfaces;

    public class ResetPasswordCommand : IRequest
    {
        public string Token { get; set; }
        public string Password { get; set; }

        public ResetPasswordCommand(string token, string password)
        {
            Token = token;
            Password = password;
        }

        public class Handler : IRequestHandler<ResetPasswordCommand>
        {
            private IUnitOfWork _uow;
            private IJwtService _jwt;

            public Handler(IUnitOfWork uow, IJwtService jwt)
            {
                _uow = uow;
                _jwt = jwt;
            }
            public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
            {
                _jwt.ValidateStringToken(request.Token);
                if(!_jwt.IsResetPasswordToken(request.Token))
                {
                    throw new ValidationException();
                }

                int userId = _jwt.GetUserIdFromToken(request.Token);
                var user = await _uow.UsersRepository.FirstOrDefaultAsync(x => x.Id.Equals(userId));
                //TODO: ValidatePassword

                user.Password = PasswordHelper.CreateHash(request.Password);
                _uow.UsersRepository.Update(user);
                await _uow.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}
