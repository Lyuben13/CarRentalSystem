```markdown
# Car Rental System / Система за наемане на автомобили

A comprehensive console-based application in C# for managing a car rental fleet, demonstrating Object-Oriented Programming (OOP) principles.

Подробно конзолно приложение на C# за управление на автопарк под наем, реализиращо обектно-ориентирани принципи.

---

## Table of Contents / Съдържание
- [Overview / Описание](#overview--описание)
- [Features / Функционалности](#features--функционалности)
- [Architecture / Архитектура](#architecture--архитектура)
- [Data Structures / Структури от данни](#data-structures--структури-от-данни)
- [CSV File Formats / CSV Формати](#csv-file-formats--csv-формати)
- [Installation / Инсталация](#installation--инсталация)
- [Usage / Използване](#usage--използване)
- [Commands / Команди](#commands--команди)
- [Error Handling / Обработка на грешки](#error-handling--обработка-на-грешки)
- [Future Enhancements / Бъдещи подобрения](#future-enhancements--бъдещи-подобрения)
- [License / Лиценз](#license--лиценз)

---

## Overview / Описание

**English**

This Car Rental System is a console application built with .NET 8.0 that allows you to manage cars, customers, and rentals. It showcases key OOP principles—encapsulation, inheritance, polymorphism, and abstraction—while providing features like data persistence via CSV and business intelligence statistics.

**Български**

Системата за наем на автомобили е конзолно приложение, разработено с .NET 8.0, което позволява управление на автомобили, клиенти и наеми. Демонстрира ключови ООП принципи—инкапсулация, наследяване, полиморфизъм и абстракция—и предлага функционалности като запис в CSV и статистики за бизнес анализ.

---

## Features / Функционалности

| Feature / Функция         | Description / Описание                                                      |
|---------------------------|-----------------------------------------------------------------------------|
| Add Car / Добавяне на кола   | Create new car with ID, make, model, year, type, color, license plate, rate. |
| Edit Car / Редакция на кола  | Modify details of existing cars interactively.                              |
| Remove Car / Премахване       | Flag cars as removed from the fleet.                                        |
| List & Search / Списък и търсене | View all, available, or rented cars; search by ID, model, status.           |
| Rent & Return / Наем и връщане | Rent cars to customers, track return, calculate cost.                       |
| Rentals Tracking / Мониторинг | View active, completed, overdue rentals.                                    |
| Statistics / Статистика        | Show fleet and revenue stats, maintenance needs, overdue alerts.            |
| CSV Persistence / CSV запис   | Load/save cars & rentals to CSV files without external libraries.           |

---

## Architecture / Архитектура

**Project Structure**

```

CarRentalSystem/
├── Models/
│   ├── AbstractVehicle.cs    # Abstract base class
│   ├── Car.cs               # Car entity
│   ├── Customer.cs          # Customer entity
│   └── Rental.cs            # Rental transaction
├── Interfaces/
│   ├── IRentable.cs         # Rentable interface
│   ├── ISearchable.cs       # Searchable interface
│   └── IFileOperations.cs   # CSV reader/writer interface
├── Services/
│   ├── CarRentalService.cs  # Business logic
│   ├── CarFileReader.cs     # Read cars CSV
│   ├── CarFileWriter.cs     # Write cars CSV
│   ├── RentalFileReader.cs  # Read rentals CSV
│   └── RentalFileWriter.cs  # Write rentals CSV
└── Program.cs               # Entry point & UI loop

````

---

## Data Structures / Структури от данни

- **List<T>**: Stores cars, customers, rentals.
- **Dictionary<K,V>**: Groups cars by type, revenue by month.
- **IEnumerable<T>**: Enables LINQ queries for filtering and projection.

---

## CSV File Formats / CSV Формати

**Cars CSV**
```csv
Id,Make,Model,Year,Type,Status,CurrentRenter,LicensePlate,Mileage,DailyRate
1,Toyota,Corolla,2019,Sedan,Available,ABC123,50000,50.00
````

**Rentals CSV**

```csv
Id,CarId,CustomerId,StartDate,ExpectedReturn,ActualReturn,DailyRate,TotalCost,Status
1001,1,101,2025-01-15,2025-01-20,2025-01-19,50.00,200.00,Completed
```

---

## Installation / Инсталация

1. **Clone the repository / Клонирай репото**

   ```bash
   git clone https://github.com/Lyuben13/CarRentalSystem.git
   ```
2. **Open in Visual Studio / Отвори в Visual Studio**
3. **Run the project / Стартирай проекта**

   ```bash
   dotnet run
   ```

---

## Usage / Използване

After launching, enter commands at the prompt. Example session:

```bash
=== Welcome to Car Rental System ===
Loaded 10 cars and 3 rentals.

Command> listall
ID: 1 | Toyota Corolla (2019) | Sedan | Available | Rate: $50.00/day
...

Command> rent 1 "Goshko Goshkarq"
Car 1 rented to Goshko Goshkarq.

Command> stats
Total Cars: 10
Available: 9
Rented: 1

Command> exit
Data saved. Goodbye!
```

---

## Commands / Команди

| Command                | Description / Описание                             |
| ---------------------- | -------------------------------------------------- |
| `help`                 | Show all commands / Помощ                          |
| `listall`              | List all cars / Всички коли                        |
| `listavailable`        | List available cars / Свободни коли                |
| `listrented`           | List rented cars / Наети коли                      |
| `listoverdue`          | List overdue rentals / Просрочени наеми            |
| `listmaintenance`      | Cars needing maintenance / Коли за обслужване      |
| `add`                  | Add a new car / Добави кола                        |
| `addcustomer`          | Add a customer / Добави клиент                     |
| `edit <id>`            | Edit car details / Редактира кола                  |
| `remove <id>`          | Remove car / Премахни кола                         |
| `rent <id> <name>`     | Rent car to customer / Дай кола под наем на клиент |
| `return <id>`          | Return a car / Върни кола                          |
| `search id <id>`       | Search car by ID / Търси по ID                     |
| `search model <text>`  | Search by model / Търси по модел                   |
| `search status <stat>` | Search by status / Търси по статус                 |
| `stats`                | Show fleet statistics / Статистика                 |
| `revenue`              | Show revenue data / Приходи                        |
| `save`                 | Save to CSV / Запази в CSV                         |
| `exit`                 | Save and exit / Изход                              |

---

## Error Handling / Обработка на грешки

* **Input Validation / Валидация**: Ensures correct formats and ranges.
* **File I/O / Файлови операции**: Graceful handling of read/write errors.
* **Business Logic / Бизнес логика**: Proper exception management.

---

## Future Enhancements / Бъдещи подобрения

* Database integration (SQL Server, SQLite)
* Web API for remote access
* GUI using WPF or WinForms
* Advanced reporting and analytics
* Customer loyalty program
* Automated maintenance scheduling
* Payment processing integration

---

## License / Лиценз

MIT © 2025 Lyuben Andreev
