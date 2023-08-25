using System.Runtime.InteropServices;
using DiplomaProject.Infrastructure.Shared.Encryption.Structs;

namespace DiplomaProject.Infrastructure.Shared.Encryption;

public static class Encryption
{
    [DllImport("Encryption/libEncryption", CallingConvention = CallingConvention.Cdecl)]
    public static extern RSAKeyPair generateRSA();
}