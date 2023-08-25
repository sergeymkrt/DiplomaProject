//
// Created by Sergey Mkrtumyan on 24.08.23.
//

#include <stdlib.h>
#include "mainFunctions.h"
#include "../aes/aes.h"
#include "../rsa/rsa.h"
#define KEY_SIZE 2048


int generate(unsigned char *key, unsigned char *iv, int keyLength) {
    return generateAESKey(key, iv, keyLength);
}

struct RSAKeyPair generateRSA(){
    unsigned char *private_key_buffer = NULL;
    unsigned char *public_key_buffer = NULL;
    int* private_key_length = malloc(sizeof(int));
    int* public_key_length = malloc(sizeof(int));
    return generateRSAKeyPair(&private_key_buffer, private_key_length, &public_key_buffer, public_key_length, KEY_SIZE);
}

int encrypt(const unsigned char *input_buffer, int input_length, unsigned char *key, unsigned char *iv, unsigned char *output_buffer) {
    return encryptAES(input_buffer, input_length, key, iv, output_buffer);
}

int decrypt(const unsigned char *input_buffer, int input_length, unsigned char *key, unsigned char *iv, unsigned char *output_buffer) {
    return decryptAES(input_buffer, input_length, key, iv, output_buffer);
}

