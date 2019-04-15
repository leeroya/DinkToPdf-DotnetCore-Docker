# DinkToPdf dotnet core Docker

## This is a example of dotnet core 2.2 in a Docker container using DinkToPdf Library

This is by no means the best project layout or use of object and resources but is intented
to showcase the idea of using the library DinkToPdf and how to run it in a Docker container.

## Developement

1. Clone repo.
2. dotnet restore restore 
3. dotnet build
4. dotnet run 


## Docker 

    docker build -t pdfdemo:1 .

Then after built and tagged.

    docker run -p 89:80 pdfdemo:1

Then open a browser and navigate to http://localhost:89/api/values

and the generated file will be downloaded.


