namespace Entities.Exceptions
{
    public sealed class BookNotFoundException : NotFoundException // sealed kalıtılması mümkün değil.
    {
        public BookNotFoundException(int id) : base($"The book with id: {id} could not found.") //string gönderebiliyoruz.
        {

        }
    }
}
