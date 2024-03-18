using DiplomaProject.Application.Exceptions;
using DiplomaProject.Application.Models;
using DiplomaProject.Domain.Enums;
using DiplomaProject.Domain.Services.External;

namespace DiplomaProject.Application.UseCases.Authentication.Commands;

public class VerifyEmailCommand(string token, string email) : BaseCommand
{
    public string Token { get; set; } = token;
    public string Email { get; set; } = email;


    public class VerifyEmailCommandHandler(IMapper mapper,
        ICurrentUser currentUser,
        IAuthenticationService authenticationService)
        : BaseCommandHandler<VerifyEmailCommand>(currentUser, mapper)
    {
        public override async Task<ResponseModel> Handle(VerifyEmailCommand request,
            CancellationToken cancellationToken)
        {
            // var user = await userManager.FindByEmailAsync(request.Email);
            // if (user == null)
            // {
            //     throw new BadRequestException("User not found");
            // }
            // var result = await userManager.ConfirmEmailAsync(user, request.Token);
            var result = await authenticationService.VerifyEmail(request.Token, request.Email);
            if (result.Item1)
            {
                return ResponseModel.Create(ResponseCode.Success);
            }
            throw new BadRequestException(result.Item2.First().Description);
        }
    }
}