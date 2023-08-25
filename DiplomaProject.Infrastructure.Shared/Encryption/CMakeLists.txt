cmake_minimum_required(VERSION 3.26)
project(Encryption C)

set(CMAKE_C_STANDARD 23)

find_package(OpenSSL REQUIRED)
include_directories(${OPENSSL_INCLUDE_DIR})

# Add the source files for the shared library
add_library(Encryption SHARED
    aes/aes.h
    aes/aes.c
    rsa/rsa.c
    rsa/rsa.h
    shared/mainFunctions.c
    shared/mainFunctions.h
    utils.h
)

# Set the output directory for the shared library
set_target_properties(Encryption PROPERTIES
    LIBRARY_OUTPUT_DIRECTORY ..
)

target_link_libraries(Encryption PUBLIC ${OPENSSL_LIBRARIES})