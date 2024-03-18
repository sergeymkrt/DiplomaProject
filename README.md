# DiplomaProject
University diploma project

Secure File Sharing Platform.
Platform that allows businesses (b2b) or individuals (b2c) to securely share files with end-to-end encryption.

## How to run
Our backend is written in C# , .NET 7 , so we have to have the dotnet 7.0 sdk installed in our system </br>
Download and install .NET [here](https://dotnet.microsoft.com/en-us/download) </br>
Also, keep in mind that at the start of application, Entity Framework migrations are running, so please supply the correct database connection string before running the application </br>
To run the project navigate to DiplomaProject.WebApi and run command `dotnet run`

# Features
## Authentication and Authorization
Using JWT tokens, Bearer Scheme </br>
Token is signed using Elliptic Curve Digital Signature Algorithm (nistP521) </br>
Token is transported by Http Only Cookies, so client side doesn't handle the token send/receive functionality.

## Role Based Auth
- All user apis are secured with JWT authorization
- APIs can be restricted to only 1 or more Roles using Authorization Attribute , existing roles are Administrator and User

## Encryption
Encryption is handled in several stages.

### The file transmission from client to backend is handled by SSL.
- [ ] Generating a checksum on client side with a specific secret that is present only on client side and backend side, sending it along the file
- [ ] Verifying the checksum on backend side

###  Backend to Storage
- [X] Generating RSA 2048 Keys (1 pair) for all files
- [X] For Each file generating random AES 128-bit key
- [X] Encrypting the file with AES
- [X] Encrypting the AES key with RSA and Concat(file, key)
- [X] Sending the file via SSL to the secure storage (AWS S3 Bucket, Azure blob)

## Data Backup
Data Backup will be implemented on database side, using Microsoft Sql Server features. </br>
We will have snapshots which are saved every day and exported to another storage.

## Docker Support
We have docker support, docker-compose and dockerfile. </br>
In Docker-compose, we have azure-sql-edge as sql server </br>
Azure SQL Edge was picked because it is the only T-SQL server that can be deployed on arm64 based CPUs.
