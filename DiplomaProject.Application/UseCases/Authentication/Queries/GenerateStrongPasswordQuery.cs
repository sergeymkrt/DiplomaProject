using DiplomaProject.Domain.Services.External;

namespace DiplomaProject.Application.UseCases.Authentication.Queries;

public class GenerateStrongPasswordQuery : BaseQuery<string>
{
    public class GenerateStrongPasswordQueryHandler(IMapper mapper, ICurrentUser currentUser)
        : BaseQueryHandler<GenerateStrongPasswordQuery>(mapper, currentUser)
    {
        public override Task<string> Handle(GenerateStrongPasswordQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}