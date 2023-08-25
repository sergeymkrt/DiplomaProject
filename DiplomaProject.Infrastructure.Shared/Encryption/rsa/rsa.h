//
// Created by Sergey Mkrtumyan on 24.08.23.
//

#ifndef DIPLOMAENCRYPTION_RSA_H
#define DIPLOMAENCRYPTION_RSA_H

#include <stdlib.h>

struct RSAKeyPair {
    unsigned char *private_key;
    int private_key_length;
    unsigned char *public_key;
    int public_key_length;
};

struct RSAKeyPair generateRSAKeyPair(unsigned char **private_key_buffer, int* private_key_length,unsigned char **public_key_buffer, int* public_key_length, int key_size);

#endif //DIPLOMAENCRYPTION_RSA_H
