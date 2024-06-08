namespace WebApplication1.Data
{
    public class DbContext
    {
        private DbContextOptions<SubscriptionContext> options;

        public DbContext(DbContextOptions<SubscriptionContext> options)
        {
            this.options = options;
        }
    }
}