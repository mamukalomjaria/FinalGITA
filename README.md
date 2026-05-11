# 🖥️ C# Console Projects

A collection of four console-based C# applications demonstrating core programming concepts including arithmetic operations, game logic, file I/O (CSV, XML, JSON), input validation, and the Single Responsibility Principle.

---

## 📋 Table of Contents

- [Project 1 – Console Calculator](#project-1--console-calculator)
- [Project 2 – Number Guessing Game](#project-2--number-guessing-game)
- [Project 3 – Hangman](#project-3--hangman)
- [Project 4 – ATM Operations](#project-4--atm-operations)
- [General Requirements](#general-requirements)

---

## Project 1 – Console Calculator

A simple interactive console calculator that supports the four basic arithmetic operations.

### Features

- Supports `+`, `-`, `*`, `/` operations
- Accepts two numeric inputs from the user
- Input validation with meaningful error messages for invalid entries
- Handles division by zero gracefully
- Runs continuously — does not exit after a single operation

### Usage

```
Enter first number: 10
Choose operation (+, -, *, /): *
Enter second number: 5
Result: 50
```

---

## Project 2 – Number Guessing Game

A guessing game where the program generates a random number and the player tries to guess it within 10 attempts.

### Features

- **Difficulty levels** selectable at game start:
  | Level  | Range  |
  |--------|--------|
  | Easy   | 1 – 15  |
  | Medium | 1 – 25 |
  | Hard   | 1 – 50 |

- Player receives **Higher / Lower** hints after each guess
- Maximum of **10 attempts** per round
- Win condition: correct guess within 10 attempts
- Lose condition: all 10 attempts exhausted

### Leaderboard

- Game history is persisted in **CSV format**
- Stores player name and highest score
- Displays **Top 10 players** and their scores
- Players can improve their personal records

### CSV Structure

```
Name,HighScore,GamesPlayed
Alice,95,12
Bob,80,7
```

---

## Project 3 – Hangman

A classic word-guessing game using a predefined word list.

### Word List

```csharp
List<string> words = new List<string>
{
    "apple", "banana", "orange", "grape", "kiwi",
    "strawberry", "pineapple", "blueberry", "peach", "watermelon"
};
```

### Features

- Program selects a **random word** from the list
- Player has **6 attempts** to reveal letters
- Correct guesses reveal letter positions in the word
- After using letter guesses, the player submits the **full word**
- Win conditions:
  - All letters revealed within 6 guesses, **or**
  - Correct full word submitted
- Lose conditions:
  - All 6 guesses used on letters **not in the word**, **or**
  - Incorrect full word submitted

### Leaderboard

- Game history is persisted in **XML format**
- Stores player name and highest score
- Displays **Top 10 players** and their scores

### XML Structure

```xml
<Leaderboard>
  <Player>
    <Name>Alice</Name>
    <HighScore>120</HighScore>
  </Player>
</Leaderboard>
```

---

## Project 4 – ATM Operations

A console application simulating basic ATM functionality with user authentication and operation logging.

### Features

- **Authentication**: Users must log in with a registered ID number and 4-digit PIN
- **Operations**:
  - Check balance
  - Deposit funds
  - Withdraw funds
  - View transaction history
- All operations are **logged to a JSON file** with timestamps and user details

### User Model

| Field          | Description                                      |
|----------------|--------------------------------------------------|
| `Id`           | Auto-incremented unique identifier               |
| `FirstName`    | User's first name                                |
| `LastName`     | User's last name                                 |
| `IdNumber`     | Unique national ID number                        |
| `PIN`          | Auto-generated unique 4-digit password           |
| `Balance`      | Current account balance                          |

### Log Examples (JSON)

```json
{ "user": "Nika Chkhartishvili", "action": "Checked balance", "date": "22.02.2024" }
{ "user": "Nika Chkhartishvili", "action": "Deposited 100 GEL", "date": "22.02.2024", "balance": 500 }
{ "user": "Nika Chkhartishvili", "action": "Withdrew 50 GEL", "date": "22.02.2024", "balance": 450 }
```

### Data Persistence

- Registered users stored in a **JSON file**
- Transaction logs stored in a **JSON file**

---

## General Requirements

All four projects adhere to the following mandatory rules:

1. **No compilation errors** — any project with errors receives 0 points
2. **Exception handling** — all exceptions must be properly caught and handled; no unhandled crashes
3. **Single Responsibility Principle (SRP)** — each component is responsible for one concern only; business logic is separated from technical/infrastructure logic

---

## 🛠️ Tech Stack

- **Language**: C# (.NET)
- **Interface**: Console Application
- **Data Formats**: CSV, XML, JSON
- **Paradigm**: Object-Oriented Programming with SRP

---

## 📁 Project Structure (Recommended)

```
/
├── Calculator/
│   ├── Calculator.cs          # Core arithmetic logic
│   └── Program.cs             # Entry point & UI loop
├── NumberGuessingGame/
│   ├── GameEngine.cs          # Game logic
│   ├── LeaderboardService.cs  # CSV read/write
│   └── Program.cs
├── Hangman/
│   ├── GameEngine.cs          # Word/guess logic
│   ├── LeaderboardService.cs  # XML read/write
│   └── Program.cs
└── ATM/
    ├── AuthService.cs         # Authentication logic
    ├── AtmService.cs          # ATM operations
    ├── LoggingService.cs      # JSON logging
    ├── UserRepository.cs      # User data persistence
    └── Program.cs
```
