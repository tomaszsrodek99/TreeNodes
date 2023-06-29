# TreeNodes
 Struktura drzewiasta do zarządzania folderami
 
## Uruchamianie aplikacji
Aby uruchomić tę aplikację, postępuj zgodnie z poniższymi krokami:

1. Pobierz pliki źródłowe aplikacji i umieść je w jednym folderze na swoim komputerze.
2. Otwórz plik solucji (plik o rozszerzeniu .sln) w programie Visual Studio.
3. Przejdź do Konsoli Menedżera Pakietów (Package Manager Console) w programie Visual Studio. Możesz to zrobić, wybierając menu "Tools" (Narzędzia) > "NuGet Package Manager" (Menedżer pakietów NuGet) > "Package Manager Console" (Konsola Menedżera Pakietów).
4. W Konsoli Menedżera Pakietów wpisz następujące polecenie i naciśnij Enter, aby zainstalować wymagane paczki NuGet: dotnet restore

Jeśli wciąż brakuje jakiegoś pakietu, użyj poniższej listy paczek i zainstaluj je ręcznie za pomocą Konsoli Menedżera Pakietów:
1. AutoMapper
2. AutoMapper.Extensions.Microsoft.DependencyInjection
3. FluentValidation
4. FluentValidation.DependencyInjectionExtensions
5. Microsoft.EntityFrameworkCore (wersja 5.0.17)
6. Microsoft.EntityFrameworkCore.Design (wersja 5.0.17)
7. Microsoft.EntityFrameworkCore.SqlServer (wersja 5.0.17)
8. Microsoft.EntityFrameworkCore.Tools (wersja 5.0.17)

Na przykład, aby zainstalować paczkę AutoMapper, wpisz w Konsoli Menedżera Pakietów: Install-Package AutoMapper. Możesz to zrobić również, wybierając menu "Tools" (Narzędzia) > "NuGet Package Manager" (Menedżer pakietów NuGet) > (Zarządzaj pakietami NuGet).

5. Otwórz plik appsettings.json i zmień nazwę bazy danych na odpowiednią nazwę lub utwórz nową bazę danych o nazwie "TreeNodes" na serwerze SQL Server.
6. Wykonaj migrację bazy danych, wpisując nastepujące polecenia w Konsoli Menedżera Pakietów: Add-Migration Init oraz Update-Database.
7. Skompiluj projekt aby upewnić się, że nie ma żadnych błędów.

Po wykonaniu tych kroków, aplikacja powinna być gotowa do uruchomienia. Możesz ją uruchomić, klikając przycisk "Start" lub używając skrótu klawiaturowego F5.
