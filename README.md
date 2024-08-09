# FitLog

The project was generated using the [Clean.Architecture.Solution.Template](https://github.com/jasontaylordev/FitLog) version 8.0.5.

## Build

Run `dotnet build -tl` to build the solution.

## Run

To run the web application:

```bash
cd .\src\Web\
dotnet watch run
```

Navigate to https://localhost:5001. The application will automatically reload if you change any of the source files.

## Code Styles & Formatting

The template includes [EditorConfig](https://editorconfig.org/) support to help maintain consistent coding styles for multiple developers working on the same project across various editors and IDEs. The **.editorconfig** file defines the coding styles applicable to this solution.

## Code Scaffolding

The template includes support to scaffold new commands and queries.

Start in the `.\src\Application\` folder.

Create a new command:

```
dotnet new ca-usecase --name CreateTodoList --feature-name TodoLists --usecase-type command --return-type int
```

Create a new query:

```
dotnet new ca-usecase -n GetTodos -fn TodoLists -ut query -rt TodosVm
```

If you encounter the error *"No templates or subcommands found matching: 'ca-usecase'."*, install the template and try again:

```bash
dotnet new install Clean.Architecture.Solution.Template::8.0.5
```

## Test

The solution contains unit, integration, functional, and acceptance tests.

To run the unit, integration, and functional tests (excluding acceptance tests):
```bash
dotnet test --filter "FullyQualifiedName!~AcceptanceTests"
```

To run the acceptance tests, first start the application:

```bash
cd .\src\Web\
dotnet run
```

Then, in a new console, run the tests:
```bash
cd .\src\Web\
dotnet test
```
## Project Configuration Guide
This guide will help you set up the necessary authentication and email settings for your application. Make sure to replace the default values with your own credentials before running the project.

1. Authentication Settings
The project supports authentication via Google and Facebook. To configure these, you need to obtain the appropriate ClientId, ClientSecret, AppId, and AppSecret from the respective developer platforms.

### Google Authentication
Visit the Google Cloud Console.
Create a new project or select an existing one.
Navigate to Credentials under APIs & Services.
Create OAuth 2.0 Client IDs and generate your ClientId and ClientSecret.
Replace the default values in the appsettings.json file as shown below:

```json
"Authentication": {
  "Google": {
    "ClientId": "YOUR_GOOGLE_CLIENT_ID",
    "ClientSecret": "YOUR_GOOGLE_CLIENT_SECRET"
  }
}
```
### Facebook Authentication
Go to the Facebook for Developers page.
Create a new app or select an existing one.
Navigate to Settings > Basic and find your AppId and AppSecret.
Replace the default values in the appsettings.json file as shown below:

```json
"Authentication": {
  "Facebook": {
    "AppId": "YOUR_FACEBOOK_APP_ID",
    "AppSecret": "YOUR_FACEBOOK_APP_SECRET"
  }
}
```
2. Email Settings
The application also requires email settings to send out emails via SMTP. You can use any SMTP service provider. Below is an example of how to configure these settings using Gmail.
Gmail Configuration
Ensure that "Less secure app access" is enabled on your Google account (if you're using Gmail).
Replace the values in the appsettings.json file as shown below:

```json
"EmailSettings": {
  "FromEmail": "YOUR_EMAIL_ADDRESS",
  "SmtpHost": "smtp.gmail.com",
  "SmtpPort": "587",
  "SmtpUser": "YOUR_EMAIL_ADDRESS",
  "SmtpPass": "YOUR_EMAIL_APP_PASSWORD"
}
```

## Database
The solution uses EntityFramework to generate a database code-first

To run the solution on your computer, first get the connection string to your SQL Server server and extract the connection string. Then replace the connection string in the Infrastructure project
Navigate to Infrastructure>Data>ApplicationDbContext
Uncomment the OnConfiguring method and replace the connection string with your own.
After that, use command line to generate the database and initial data

```bash
cd .\src\Infrastructure
dotnet ef database update
```

Then, to use the database in the project, navigate to the appsettings file in the Web project
And replace the connection string in this section with your own

```json
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=JOSEPHPHAM;Database=FitLogDatabase;Trust Server Certificate=True;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
```




## Help
To learn more about the template go to the [project website](https://github.com/jasontaylordev/CleanArchitecture). Here you can find additional guidance, request new features, report a bug, and discuss the template with other users.
