//
// Created by Sergey Mkrtumyan on 24.08.23.
//

#ifndef DIPLOMAENCRYPTION_MAINFUNCTIONS_H
#define DIPLOMAENCRYPTION_MAINFUNCTIONS_H


int generate(unsigned char *key, unsigned char *iv, int keyLength);
struct RSAKeyPair generateRSA();
int encrypt(const unsigned char *input_buffer, int input_length, unsigned char *key, unsigned char *iv, unsigned char *output_buffer);
int decrypt(const unsigned char *input_buffer, int input_length, unsigned char *key, unsigned char *iv, unsigned char *output_buffer);
#endif //DIPLOMAENCRYPTION_MAINFUNCTIONS_H
