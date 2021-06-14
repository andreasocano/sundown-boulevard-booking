# Getting started

## Dependencies

* **SQL Server**: to store data.
* **IIS 10.0 Express**: to run website.
* **.NET Framework 4.8**: to run website.
* **.NET Core 3.1**: to run API.
* **Entity Framework Core**: to access data in the database.

## Configure the solution

Before running the solution, go through the following checklist:


* Consider adding sensitive information to a key vault and applying them to the solution upon deployment.
* Create an SQL database and a user.
* In [`SundownBoulevard.Booking.API/appSettings.json`](../SundownBoulevard.Booking.API/appSettings.json):
	- Add the connection string to your newly created database.
	- Take note of the website domain and change it if running the website on a different domain.
	- Insert SMTP settings.
* In [`SundownBoulevard.Booking.Website/Web.config`](../SundownBoulevard.Booking.Website/Web.config), check that the domains under `<appSettings>` correspond to the ones your would expect. Especially the API domain.

## First time you run the solution

Ensure that both the website and the API are started.
- Right-click the solution and select *Set Startup Projects...*.
- Select *Multiple startup projects*.
- Select *Start* or *Start without debugging* in the *Action* column for the website and API project.
- Click buttons *Apply* then *OK*.

### API

In [`SundownBoulevard.Booking.API/Startup.cs`](../SundownBoulevard.Booking.API/Startup.cs), configurations are applied to the application. Database migrations are also run.

**NOTE!** Currently this is also where the *Table* table is seeded with data, until a better solution has been found.

## Changes to database schemas

This solution uses a code first approach. If you make changes to the entity models

- Go to Package Management Console
- Set `SundownBoulevard.Booking.DAL` as the default project
- Run `Add-Migration <name-of-your-migration>`

The next time you run the solution (or just the API), changes will be applied to the database.

## Quirks

The website uses `HttpClient` to call the API. On local development environments it threw a certificate error on a machine where a *localhost* certificate wasn't added the trusted certificate store.
A workaround has been implemented in the form of a `HttpClientHandler` that allows the request to be sent in the event of a certificate error.
This should only be used on local development environments.

## Common issues & resolutions

## Q. System.IO.DirectoryNotFoundException: Could not find a part of the path 'my-solution-path\SundownBoulevard.Booking.Website\bin\roslyn\csc.exe'.

I just cloned down the solution but upon running it I get the above error.

Go to Package Management Console.

- Set `SundownBoulevard.Booking.Website` as the default project
- Run `Update-Package Microsoft.CodeDom.Providers.DotNetCompilerPlatform -r` as per [this answer on stackoverflow](https://stackoverflow.com/questions/32780315/could-not-find-a-part-of-the-path-bin-roslyn-csc-exe)