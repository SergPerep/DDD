namespace Library.Domain.Book;

// The repository interface must live in the domain layer
// The repository implementation in the infrastructure layer
// The repository is a mechanism for getting and storing aggregate roots from DB, in-memory or other form of storage.

public interface IBookRepository
{
    Book GetById(Guid id);
    void Add(Book book);
    void Save(Book book);
}