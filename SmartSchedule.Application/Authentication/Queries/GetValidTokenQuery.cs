﻿namespace SmartSchedule.Application.Authentication.Queries.Authentication
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using SmartSchedule.Application.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Authentication;
    using SmartSchedule.Application.DTO.Authentication.Queries;
    using SmartSchedule.Application.Exceptions;
    using SmartSchedule.Application.Helpers;
    using SmartSchedule.Application.Interfaces;

    public class GetValidTokenQuery : IRequest<JwtTokenModel>
    {
        public LoginRequest Login { get; set; }

        public GetValidTokenQuery(LoginRequest login)
        {
            Login = login;
        }

        public class Handler : IRequestHandler<GetValidTokenQuery, JwtTokenModel>
        {
            private IUnitOfWork _uow;
            private IJwtService _jwt;

            public Handler(IUnitOfWork uow, IJwtService jwt)
            {
                _uow = uow;
                _jwt = jwt;
            }

            public async Task<JwtTokenModel> Handle(GetValidTokenQuery request, CancellationToken cancellationToken)
            {
                var user = await _uow.UsersRepository.FirstOrDefaultAsync(x => x.Email.Equals(request.Login.Email));
                if (user == null)
                {
                    throw new NotFoundException(request.Login.Email, -1);
                }
                else if (!PasswordHelper.ValidatePassword(request.Login.Password, user.Password))
                {
                    throw new ValidationException();
                }

                return _jwt.GenerateJwtToken(user.Email, user.Id, false);
            }
        }
    }
}
