# SQL Challenge

This repository contains solutions for the SQL Challenge tasks, focusing on data manipulation and SQL operations.

## Prerequisites

- **.NET SDK**: Ensure you have the .NET SDK installed (version 9.0). Download it from [dotnet.microsoft.com](https://dotnet.microsoft.com/download).
- **Visual Studio Code**: Recommended IDE with the C# extension installed.
- **EPPlus**: A .NET library for creating and manipulating Excel files. It will be installed via NuGet.

## Installation

### 1. Clone the Repository
```bash
git clone <repository-url>
cd sql_challenge
```

### 2. Install Dependencies
- Open the project folder in VS Code.
- Open the terminal in VS Code (Ctrl+`).
- Install the EPPlus package:
  ```bash
  dotnet add package EPPlus
  ```
- Restore dependencies:
  ```bash
  dotnet restore
  ```

### 3. Build the Project
```bash
dotnet build
```

## Running the Program

### Task A: Data Manipulation
The C# console application processes a CSV file and generates an Excel file.

1. **Prepare Input File**:
   - Create a file named `input.csv` in the project directory with the following format (example below).
   - The CSV should have a header row and data rows with columns: `entity_id`, `entity_first_name`, `entity_middle_name`, `entity_last_name`, `entity_dob`, `is_master`, `address`, `entity_gender`.

   **Example `input.csv`:**
   ```
   entity_id,entity_first_name,entity_middle_name,entity_last_name,entity_dob,is_master,address,entity_gender
   1001,John,,Smith,1990-05-15,1,"123 Main Street, Wellington, New Zealand",Male
   1002,Mary,Jane,Doe,1985-12-22,0,"456 Oak Avenue, Auckland, New Zealand",Female
   1003,Paul,,Smith,1988-02-18,1,"789 Pine Road, Christchurch, New Zealand",Male
   1004,Sarah,Ann,Johnson,1992-07-30,0,"321 Elm Street, Dunedin, New Zealand",Female
   1005,David,,Brown,1980-03-10,1,"654 Cedar Lane, Hamilton, New Zealand",Male
   1006,Emily,,Davis,1995-11-25,,,"789 Birch Court, Tauranga, New Zealand",
   1007,Michael,James,Wilson,1983-09-14,0,"147 Maple Drive, Palmerston North, New Zealand",Male
   ```

2. **Run the Application**:
   - Execute the program with the input CSV file as an argument:
     ```bash
     dotnet run -- input.csv
     ```
   - This will generate an `output.xlsx` file in the same directory.

### Task B: SQL Update
The SQL script updates the `AnnualReviewDate` in the `Profiles` table.
The statement for the solution to Task B is located in the file TaskB.sql
