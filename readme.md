Para inicializar a tabela de recuperação de senha, acesse a pasta DAL atavés do terminal e digite os seguintes comandos:
1 - dotnet ef migrations add first --context PasswordChangeContext
2 - dotnet ef database update --context PasswordChangeContext
