
dotnet new tool-manifest

dotnet tool install NSwag.ConsoleCore --version 13.10.9

curl -o swagger.json http://localhost:5199/swagger/v1/swagger.json

dotnet nswag swagger2csclient /input:swagger.json /classname:TodoService /namespace:TodoReader /output:TodoService.cs

