# _Sweet and Savory Treats: User Authentication_

#### By _**Israel Padilla**_

#### _Delve into the delightful world of Sweet and Savory Treats with user authentication. This project showcases many-to-many relationships between treats and flavors, providing a seamless experience for logged-in users to manage, while also allowing non-logged-in users to explore the scrumptious world of baked treats and flavors._

## Technologies Used

* Visual Studio Code v. 1.83.1
* C#
* ASP.NET Core
* ASP.NET Identity
* Razor Markup
* Entity Framework Core

## Setup / Installation Requirements / Configuration

* Open your preferred Terminal/Command Line
* Navigate to your desktop by running the command: `cd ~/Desktop`
* Clone the repository by executing: `git clone https://github.com/izzy503/SweetSavoryTreats.git`
* Open the cloned folder on your desktop using Visual Studio Code

## Adding Required Packages

* In your Terminal, navigate to the "SweetSavoryTreats" folder with the command: `cd SweetSavoryTreats`
* Ensure that the .csproj file contains all the necessary packages.
* Restore the packages by running the following command: `dotnet restore`

## Setting Up appsettings.json

* In your Terminal, navigate to the "SweetSavoryTreats" project directory.
* Create the appsettings.json file using the command: `touch appsettings.json`
* Open the appsettings.json file in your code editor, and include the following configuration:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;database=[YOUR DATABASE NAME];uid=[YOUR USER ID];pwd=[YOUR PASSWORD];"
  } 
}
```
* Save the file.

## Creating the Database

* Install the required tools in your Terminal:
```
$ dotnet tool install --global dotnet-ef --version 6.0.0
```
```
$ dotnet add package Microsoft.EntityFrameworkCore.Design -v 6.0.0
```
```
$ dotnet ef migrations add Initial
```
```
$ dotnet ef database update
```
* The database is now successfully migrated and the program is fully operational.

## Running the Program

* In your Terminal, navigate to the "SweetSavoryTreats" folder
* Start the program locally with the following command:
```
$ dotnet run
```
* The program is now running on your localhost.

## Important Notes

* Logged-in users can create, edit, or delete treats and flavors.
* Non-logged-in users can explore a list of treats and flavors on the home page and view details for each treat and flavor.

## Known Bugs

* No known bugs.

## [License](https://mit-license.org/)

_For any issues or questions related to the project, please feel free to contact me at_ [ipadilla2280@gmail.com]

Copyright (c) _2023_ _Israel Padilla_