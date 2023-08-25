using System.Runtime.InteropServices;
using System.Text;
using DiplomaProject.Domain.Services.Encryption;
using DiplomaProject.Domain.Services.External;

namespace DiplomaProject.Application.UseCases.Test.Queries;

public class EncryptionTestQuery : BaseQuery<Tuple<string,string>>
{
    public EncryptionTestQuery()
    {
    }
    
    public class EncryptionTestQueryHandler : BaseQueryHandler<EncryptionTestQuery>
    {
        private readonly IEncryptionService _encryptionService;
        public EncryptionTestQueryHandler(IMapper mapper, IIdentityUserService identityService,
            IEncryptionService encryptionService) : base(mapper, identityService)
        {
            _encryptionService = encryptionService;
        }

        public override Task<Tuple<string,string>> Handle(EncryptionTestQuery request, CancellationToken cancellationToken)
        {
            var result = _encryptionService.GenerateRSA();
            
            byte[] privateKey = new byte[result.private_key_length];
            Marshal.Copy(result.private_key, privateKey, 0, result.private_key_length);

            byte[] publicKey = new byte[result.public_key_length];
            Marshal.Copy(result.public_key, publicKey, 0, result.public_key_length);
            
            var privateKeyString = Encoding.UTF8.GetString(privateKey);
            var publicKeyString = Encoding.UTF8.GetString(publicKey);
            
            //cleanup unmanaged memory allocations
            Marshal.FreeHGlobal(result.private_key);
            Marshal.FreeHGlobal(result.public_key);
            
            return Task.FromResult(new Tuple<string,string>(publicKeyString, privateKeyString));
        }
    }
}