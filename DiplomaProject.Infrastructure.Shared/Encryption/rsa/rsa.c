//
// Created by Sergey Mkrtumyan on 24.08.23.
//

#include <openssl/rsa.h>
#include <openssl/pem.h>
#include "rsa.h"

struct RSAKeyPair generateRSAKeyPair(unsigned char **private_key_buffer, int* private_key_length,unsigned char **public_key_buffer, int* public_key_length, int key_size){
    RSA *rsa = NULL;
    BIO *priv_bio = NULL, *pub_bio = NULL;

    // Generate RSA key pair
    rsa = RSA_new();
    if (!rsa) {
        goto cleanup;
    }
    if (!RSA_generate_key_ex(rsa, key_size, NULL, NULL)) {
        goto cleanup;
    }

    // Write private key to buffer
    priv_bio = BIO_new(BIO_s_mem());
    if (!priv_bio) {
        goto cleanup;
    }
    if (!PEM_write_bio_RSAPrivateKey(priv_bio, rsa, NULL, NULL, 0, NULL, NULL)) {
        goto cleanup;
    }

    const char* private_key_data;
    *private_key_length = BIO_get_mem_data(priv_bio, &private_key_data);
    *private_key_buffer = (unsigned char *) malloc(*private_key_length);
    if (*private_key_buffer == NULL) {
        printf("Failed to allocate memory for private key buffer.\n");
        goto cleanup;
    }
    memcpy(*private_key_buffer, private_key_data, *private_key_length);



    // Write public key to buffer
    pub_bio = BIO_new(BIO_s_mem());
    if (!pub_bio) {
        goto cleanup;
    }
    if (!PEM_write_bio_RSA_PUBKEY(pub_bio, rsa)) {
        goto cleanup;
    }
    const char* public_key_data;
    *public_key_length = BIO_get_mem_data(pub_bio, &public_key_data);
    *public_key_buffer = (unsigned char *) malloc(*public_key_length);
    if (*public_key_buffer == NULL) {
        printf("Failed to allocate memory for public key buffer.\n");
        goto cleanup;
    }
    memcpy(*public_key_buffer, public_key_data, *public_key_length);

cleanup:
    if (rsa) RSA_free(rsa);
    if (priv_bio) BIO_free(priv_bio);
    if (pub_bio) BIO_free(pub_bio);
    return (struct RSAKeyPair) { *private_key_buffer, *private_key_length, *public_key_buffer, *public_key_length };
}