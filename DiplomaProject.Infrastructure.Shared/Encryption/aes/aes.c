//
// Created by Sergey Mkrtumyan on 24.08.23.
//

#include <openssl/evp.h>
#include <openssl/rand.h>

int generateAESKey(unsigned char *key, unsigned char *iv,  int keyLength) {
    if(RAND_bytes(key, keyLength)) return 1;
    if(RAND_bytes(iv, keyLength)) return 1;
    return 0;
}

int encryptAES(const unsigned char *input_buffer, int input_length, unsigned char *key, unsigned char *iv, unsigned char *output_buffer) {
    EVP_CIPHER_CTX *ctx;
    int len;

    // Create and initialize the context
    ctx = EVP_CIPHER_CTX_new();
    EVP_EncryptInit_ex(ctx, EVP_aes_128_cbc(), NULL, key, iv);

    // Encrypt the buffer
    EVP_EncryptUpdate(ctx, output_buffer, &len, input_buffer, input_length);

    // Finalize encryption
    int ciphertext_len = len;
    EVP_EncryptFinal_ex(ctx, output_buffer + len, &len);
    ciphertext_len += len;

    // Clean up
    EVP_CIPHER_CTX_free(ctx);

    return ciphertext_len;
}

int decryptAES(const unsigned char *input_buffer, int input_length, unsigned char *key, unsigned char *iv, unsigned char *output_buffer) {
    EVP_CIPHER_CTX *ctx;
    int len;

    // Create and initialize the context
    ctx = EVP_CIPHER_CTX_new();
    EVP_DecryptInit_ex(ctx, EVP_aes_128_cbc(), NULL, key, iv);

    // Decrypt the buffer
    EVP_DecryptUpdate(ctx, output_buffer, &len, input_buffer, input_length);

    // Finalize decryption
    int plaintext_len = len;
    EVP_DecryptFinal_ex(ctx, output_buffer + len, &len);
    plaintext_len += len;

    // Clean up
    EVP_CIPHER_CTX_free(ctx);

    return plaintext_len;
}