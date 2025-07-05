# Car Rental System

## 🇧🇬 Български

**Конзолно приложение на C# за управление на автопарк под наем**

### Описание

Приложението позволява:

* Добавяне, редактиране и премахване на автомобили
* Отдаване и връщане на автомобили
* Търсене по ID, модел и статус
* Запис и четене на данни в CSV файлове

### Инсталация

1. Клонирай репото:

   ```bash
   git clone https://github.com/Lyuben13/CarRentalSystem.git
   ```
2. Отвори решението в Visual Studio.
3. Стартирай проекта (F5).

### Използване

Списък с основни команди:

```
Help            – справка
ListAll         – всички коли
ListAvailable   – свободни коли
Add             – добавяне на кола
Edit <CarId>    – редакция на кола
Remove <CarId>  – маркиране за премахване
Rent <CarId> <Name>   – отдаване
Return <CarId>        – връщане
Search id <id>        – търсене по ID
Search model <text>   – търсене по модел
Search status <stat>  – търсене по статус
Save            – запазване в CSV
Exit            – запазване и изход
```

### Структура

```
Interfaces/     IRentable, ISearchable
Models/         AbstractVehicle, Car, Customer, Rental
Services/       CarFileReader/Writer, RentalFileReader/Writer, CarRentalService
Program.cs      Основна логика и меню
```

### Лиценз

MIT © 2025 Любен Андреев

---

## 🇺🇸 English

**Console application in C# for managing a car rental fleet**

### Description

The application supports:

* Adding, editing, and removing cars
* Renting and returning cars
* Searching by ID, model, and status
* Reading and writing data to CSV files

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/Lyuben13/CarRentalSystem.git
   ```
2. Open the solution in Visual Studio.
3. Run the project (F5).

### Usage

List of main commands:

```
Help            – show help
ListAll         – list all cars
ListAvailable   – list available cars
Add             – add a new car
Edit <CarId>    – edit an existing car
Remove <CarId>  – flag a car for removal
Rent <CarId> <Name>   – rent a car to a customer
Return <CarId>        – return a rented car
Search id <id>        – search by ID
Search model <text>   – search by model
Search status <stat>  – search by status
Save            – save data to CSV
Exit            – save and exit
```

### Structure

```
Interfaces/     IRentable, ISearchable
Models/         AbstractVehicle, Car, Customer, Rental
Services/       CarFileReader/Writer, RentalFileReader/Writer, CarRentalService
Program.cs      Core logic and menu
```

### License

MIT © 2025 Lyuben Andreev
