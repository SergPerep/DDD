## Goal

Build a library management domain in C# using DDD. Focus on the Domain layer first. No database, API, or UI. Everything runs through unit tests.

## Business Description

A library lends books to registered members. A member can borrow several books at once, but only within limits. Each loan has a due date. Late returns produce a fine. The library may own multiple copies of the same title.

## Ubiquitous Language

Use these exact terms in your code:


- Book: a physical copy owned by the library (not the title in general).
- ISBN: identifies a title. Many books can share one ISBN.
- Member: a registered person who can borrow.
- Loan: a record of one member borrowing one book.
- Loan period: the start date and due date of a loan.
- Fine: money owed for a late return.

## Business rules (invariants)

### Book

- A book has a valid ISBN.
- A book is either Available or OnLoan.
- A book on loan cannot be loaned again.

### Member

1. A member has a name and an email (valid format).
1. A suspended member cannot borrow.
1. A member can have at most 5 active loans.
1. A member with unpaid fines above 10.00 cannot borrow.

### Loan

1. A loan lasts 14 days by default.
1. A loan can be returned only once.
1. A loan can be extended once, by 7 days, and only if not already overdue.
1. A late return produces a fine of 0.50 per day overdue, capped at 20.00.

## Building blocks to implement

### Value objects (use record, validate in the constructor)

- `Isbn`
- `Email`
- `Money` (amount + currency)
- `LoanPeriod` (start, due date; method to check overdue and days late)

### Aggregates

- `Book`
- `Member`
- `Loan`

### Domain events

- `BookBorrowed`
- `BookReturned`
- `LoanExtended`
- `FineIssued`


### Common (Shared Base Code)

- `Entity` base class (Id, equality by Id)
- Domain event list (`Raise`, `DomainEvents`, `ClearEvents`)

### Repository Interfaces (in Domain)

- `IBookRepository`
- `IMemberRepository`
- `ILoanRepository`

### Application Layer (Use Cases)

- `RegisterMember`
- `AddBook`
- `BorrowBook`
- `ReturnBook`
- `ExtendLoan`
- `PayFine`

### Infrastructure

- In-memory repositories (Dictionary-based)
- A simple event dispatcher that calls registered handlers

## Event Reactions (Handlers)

| Event | Reaction |
|---|---|
| `BookBorrowed` | Mark the book as `OnLoan` |
| `BookReturned` | Mark the book as `Available`; if late, issue a fine to the member |
| `FineIssued` | Add the fine to the member's unpaid fines |

## Design Decision to Explore

The rules "max 5 active loans" and "no borrowing with high fines" involve both `Member` and `Loan`. Build it **two ways** and compare:

- **Version A:** `Loan` is a separate aggregate. The member's active loan count is tracked via events or passed in by the handler.
- **Version B:** `Loan` lives inside the `Member` aggregate. The rule is enforced directly.

Write down what each approach makes easier or harder.

## Tests to Write

- Each invariant above has at least one passing and one failing test.
- Value objects: equal by value, reject invalid input.
- Handlers: `BorrowBook` succeeds for a valid case, fails for each violated rule.
- Event flow: returning a late book results in a fine on the member.

## Suggested Build Order

- [ ] 1. `Isbn`, `Email`, `Money`, `LoanPeriod` with tests
- [ ] 2. `Book` aggregate with tests
- [ ] 3. `Member` aggregate with tests
- [ ] 4. `Loan` aggregate with tests
- [ ] 5. Domain events and the `Common` base classes
- [ ] 6. Repository interfaces and in-memory implementations
- [ ] 7. Application handlers and the event dispatcher
- [ ] 8. Event handlers (fines, book status)
- [ ] 9. Redo the design with Version B and compare

## Stretch Goals

- **Reservations:** a member can reserve a borrowed book; when it's returned, the first reservation is notified.
- **Membership types:** different loan rules for Student, Standard, and Premium.
- **EF Core:** replace the in-memory repositories.

## Done When

All invariants are covered by passing tests, and a full **borrow → return late → fine** flow works end to end in a test, with no database.
