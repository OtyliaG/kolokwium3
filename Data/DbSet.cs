namespace WebApplication1.Data
{
    public class DbSet<T>
    {
        internal Task FindAsync(object idClient)
        {
            throw new NotImplementedException();
        }

        internal object FirstOrDefault(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }

        internal object ToList()
        {
            throw new NotImplementedException();
        }
    }
}