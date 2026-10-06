namespace Library.Domain.Book;

// Value object because no identity, identified by its value and immutable
public enum BookStatus
{
    Available,
    OnLoan
}