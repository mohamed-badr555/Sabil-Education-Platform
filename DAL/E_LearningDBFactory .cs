using DAL.DB_Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DAL
{
    public class E_LearningDBFactory : IDesignTimeDbContextFactory<E_LearningDB>
    {
        public E_LearningDB CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<E_LearningDB>();
            optionsBuilder.UseSqlServer("SERVER = LOCALHOST\\SQLEXPRESS; DATABASE = E-Learning Website; INTEGRATED SECURiTY = TRUE; Trust Server Certificate = TRUE;");

            return new E_LearningDB(optionsBuilder.Options);
        }
    }
}
