# Sistema de Gestão de Consultas UVV

Aplicação Web em **ASP.NET Core MVC (C#)** para gerenciamento de usuários e registro de consultas médicas/profissionais, utilizando **Entity Framework Core (Code First)**, autenticação por cookie e separação de responsabilidades (Models / Views / Controllers).

## Participantes
- Lucas Rodrigues Amorim

## Tecnologias
- ASP.NET Core 8.0 (MVC)
- Entity Framework Core 8 (Code First + Migrations)
- SQL Server
- Autenticação por Cookie (`Microsoft.AspNetCore.Authentication.Cookies`)
- Bootstrap 5

## Estrutura do projeto
```
SistemaGestaoConsultasUVV/
 ├─ Controllers/        (AccountController, ConsultasController, HomeController)
 ├─ Models/             (Usuario, Consulta)
 ├─ Models/ViewModels/  (LoginViewModel, RegisterViewModel)
 ├─ Data/                (AppDbContext)
 ├─ Views/               (Account, Consultas, Home, Shared)
 ├─ Migrations/          (gerado pelo EF Core)
 ├─ Program.cs
 └─ appsettings.json
```

## Como configurar e rodar o projeto

### 1. Pré-requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server LocalDB (já vem com o Visual Studio) ou uma instância SQL Server acessível
- (Opcional) Visual Studio 2022 ou VS Code

### 2. Clonar o repositório
```bash
git clone https://github.com/Lucaamorim/Sistema-de-Gest-o-de-Consultas-UVV.git
cd Sistema-de-Gest-o-de-Consultas-UVV/SistemaGestaoConsultasUVV
```

### 3. Configurar a Connection String
No arquivo `appsettings.json`, ajuste a string de conexão conforme seu ambiente:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SistemaGestaoConsultasUVV;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

Se estiver usando o SQL Server Express/instância nomeada, troque `(localdb)\mssqllocaldb` pelo nome do seu servidor (ex.: `.\SQLEXPRESS`).

### 4. Restaurar pacotes
```bash
dotnet restore
```

### 5. Criar as Migrations e o Banco de Dados (Code First)
Este projeto usa a abordagem **Code First**: as classes em `Models/` definem o esquema, e o EF Core gera o banco através de Migrations.

```bash
dotnet tool install --global dotnet-ef   # apenas se ainda não tiver o dotnet-ef instalado
dotnet ef migrations add InitialCreate
dotnet ef database update
```

O comando `dotnet ef database update` cria o banco `SistemaGestaoConsultasUVV` no SQL Server configurado, já com as tabelas `Usuarios` e `Consultas` e o relacionamento entre elas.

### 6. Executar a aplicação
```bash
dotnet run
```
Acesse `https://localhost:<porta>` exibida no terminal (ex.: `https://localhost:7000`).

## Funcionalidades
- **Cadastro de Usuário** (`/Account/Register`) — cria uma nova conta (Nome, E-mail, Senha). A senha é armazenada com hash (`PasswordHasher`), nunca em texto puro.
- **Login** (`/Account/Login`) — autentica o usuário via cookie de autenticação.
- **Gestão de Consultas** (`/Consultas`, protegida por `[Authorize]`) — o usuário logado pode:
  - Cadastrar uma nova consulta (Especialidade, Data/Hora, Descrição);
  - Listar apenas as suas próprias consultas;
  - Editar e excluir suas consultas.
- **Logout** (`/Account/Logout`).

## Segurança
- Rotas de consulta protegidas com o atributo `[Authorize]` no `ConsultasController`.
- Pipeline de middleware configurado em `Program.cs` com `app.UseAuthentication()` **antes** de `app.UseAuthorization()`.
- Validação de entradas no servidor com Data Annotations (`[Required]`, `[EmailAddress]`, `[StringLength]`, `[Compare]`).
- Senhas nunca armazenadas em texto puro (uso de `PasswordHasher<Usuario>`).
- Cada usuário só visualiza/edita/exclui as próprias consultas (filtro por `UsuarioId` do usuário autenticado).

## Vídeo demonstrativo
🎥 Link do vídeo (cadastro, login e registro de consulta em funcionamento): **[INSERIR LINK DO LOOM/YOUTUBE AQUI]**

## Testando com Swagger/Postman
Como este é um projeto MVC (Views + Controllers) e não uma Web API pura, o Swagger não é habilitado por padrão. Para testar fluxos isoladamente, recomenda-se usar o Postman apontando para as rotas dos controllers, ou testar diretamente pela interface web.
