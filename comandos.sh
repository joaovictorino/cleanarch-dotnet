# Criar solução
dotnet new sln -n SistemaBancario

# Criar projeto de dominio
dotnet new classlib -n SistemaBancario.Dominio
dotnet sln add SistemaBancario.Dominio

# Criar projeto de aplicacao
dotnet new classlib -n SistemaBancario.Aplicacao
dotnet sln add SistemaBancario.Aplicacao

# Adicionar referência no projeto de aplicacao
dotnet add SistemaBancario.Aplicacao reference SistemaBancario.Dominio

# Criar projeto de infraestrutura
dotnet new classlib -n SistemaBancario.Infraestrutura
dotnet sln add SistemaBancario.Infraestrutura

# Adicionar referências no projeto de infraestrutura
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.0
dotnet add package Pomelo.EntityFrameworkCore.MySql --version 8.0.0
dotnet add SistemaBancario.Infraestrutura reference SistemaBancario.Dominio

# Criar projeto de API
dotnet new webapi -n SistemaBancario.Api
dotnet sln add SistemaBancario.Api

# Adicionar referências no projeto de API
dotnet add SistemaBancario.Api reference SistemaBancario.Aplicacao
dotnet add SistemaBancario.Api reference SistemaBancario.Infraestrutura
dotnet add package Swashbuckle.AspNetCore --version 6.5.0
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.0

# Instalar dependências nuget
dotnet restore

# Compilar solução
dotnet build

# Execute projeto API
dotnet run --project SistemaBancario.Api

# Migração com EF
# Instalar ferramenta EF (se não tiver)
dotnet tool install --global dotnet-ef

# Criar primeira migration
dotnet ef migrations add CriacaoInicial --project SistemaBancario.Infraestrutura --startup-project SistemaBancario.Api

# Aplicar migration ao banco
dotnet ef database update --project SistemaBancario.Infraestrutura --startup-project SistemaBancario.Api

# Docker e Docker Compose
# Iniciar o MySQL
docker compose up -d

# Verificar se está rodando
docker ps

# Conectar no MySQL (opcional)
docker exec -it banking_mysql mysql -u root -p
