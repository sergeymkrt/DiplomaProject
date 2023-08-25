using System.Runtime.InteropServices;

namespace DiplomaProject.Infrastructure.Shared.Encryption.Structs;

[StructLayout(LayoutKind.Sequential)]
public struct RSAKeyPair
{
    public IntPtr private_key; // Use IntPtr for byte arrays
    public int private_key_length;
    public IntPtr public_key;  // Use IntPtr for byte arrays
    public int public_key_length;
}