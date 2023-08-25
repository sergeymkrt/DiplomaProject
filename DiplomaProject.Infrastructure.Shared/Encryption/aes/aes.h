//
// Created by Sergey Mkrtumyan on 24.08.23.
//

#ifndef DIPLOMAENCRYPTION_AES_H
#define DIPLOMAENCRYPTION_AES_H

int generateAESKey(unsigned char *aes_key, unsigned char *iv, int key_size_bytes);
int encryptAES(const unsigned char *input_buffer, int input_length, unsigned char *key, unsigned char *iv, unsigned char *output_buffer);
int decryptAES(const unsigned char *input_buffer, int input_length, unsigned char *key, unsigned char *iv, unsigned char *output_buffer);

#endif //DIPLOMAENCRYPTION_AES_H
