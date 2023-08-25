//
// Created by Sergey Mkrtumyan on 24.08.23.
//

#include <stdio.h>
#include <stdlib.h>

#ifndef DIPLOMAENCRYPTION_UTILS_H
#define DIPLOMAENCRYPTION_UTILS_H

char* ucharArrayToHexString(const unsigned char arr[], size_t length) {
    char* hexString = (char*)malloc(length * 2 + 1); // Each byte becomes two hex characters, plus one for the null terminator
    if (hexString == NULL) {
        perror("Memory allocation error");
        exit(EXIT_FAILURE);
    }

    for (size_t i = 0; i < length; i++) {
        // Convert each byte to its two-character hexadecimal representation
        sprintf(&hexString[i * 2], "%02X", arr[i]);
    }

    return hexString;
}

int saveToFile(const char* filename, const unsigned char* buffer, int buffer_length) {
    FILE *f = fopen(filename, "wb");
    if (f == NULL) {
        perror("Failed to open file");
        return -1;
    }

    fwrite(buffer, 1, buffer_length, f);
    fclose(f);

    return 0;
}

#endif //DIPLOMAENCRYPTION_UTILS_H
