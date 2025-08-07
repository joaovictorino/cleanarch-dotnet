# Criar solução
dotnet new sln -n SistemaBancario

# Criar projeto de dominio
dotnet new classlib -n SistemaBancario.Dominio
dotnet sln add SistemaBancario.Dominio
dotnet new webapi -n SistemaBancario.Api
dotnet sln add SistemaBancario.Api
dotnet new classlib -n SistemaBancario.Infraestrutura
dotnet sln add SistemaBancario.Infraestrutura
dotnet new classlib -n SistemaBancario.Aplicacao
dotnet sln add SistemaBancario.Aplicacao

# Adicionar referências em cada projeto
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
docker-compose up -d

# Verificar se está rodando
docker ps

# Conectar no MySQL (opcional)
docker exec -it banking_mysql mysql -u root -p
