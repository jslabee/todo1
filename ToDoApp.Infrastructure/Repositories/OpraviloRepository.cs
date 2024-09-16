using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Interfaces;
using ToDoApp.Infrastructure.Data;

namespace ToDoApp.Infrastructure.Repositories
{
    public class OpraviloRepository(ToDoContext context) : IOpraviloRepository
    {
        public async Task<IEnumerable<Opravilo?>> GetAllOpravilaAsync()
        {
            var sql = "SELECT * FROM Opravila";
            return await context.Opravila.FromSqlRaw(sql).ToListAsync();
        }

        public async Task<Opravilo?> GetOpraviloByIdAsync(int id)
        {
            var sql = "SELECT * FROM Opravila WHERE Id = @Id";
            var parameter = new SqlParameter("@Id", id);

            return await context.Opravila.FromSqlRaw(sql, parameter).FirstOrDefaultAsync();
        }
        public async Task<int> AddOpraviloAsync(Opravilo opravilo)
        {
       
            var pId = new SqlParameter("@Id", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            // SQL poizvedba, ki vstavi zapis in uporabi SCOPE_IDENTITY() za zajem ID-ja
            var sql = @"
            INSERT INTO Opravila (Naslov, Opis, DatumUstvarjanja, Opravljeno) 
            VALUES (@Naslov, @Opis, @DatumUstvarjanja, @Opravljeno);
            SET @Id = SCOPE_IDENTITY();";

            
            object[] parameters =
            [
                new SqlParameter("@Naslov", opravilo.Naslov),
                new SqlParameter("@Opis", opravilo.Opis),
                new SqlParameter("@DatumUstvarjanja", opravilo.DatumUstvarjanja),
                new SqlParameter("@Opravljeno", opravilo.Opravljeno),
                pId
            ];

       
            await context.Database.ExecuteSqlRawAsync(sql, parameters);
            

            return (int)pId.Value;
        }


        public async Task UpdateOpraviloAsync(Opravilo opravilo)
        {
            var sql = "UPDATE Opravila SET Naslov = @Naslov, Opis = @Opis, DatumUstvarjanja = @DatumUstvarjanja, Opravljeno = @Opravljeno WHERE Id = @Id";

            object[] parameters =
            [
                new SqlParameter("@Naslov", opravilo.Naslov),
                new SqlParameter("@Opis", opravilo.Opis),
                new SqlParameter("@DatumUstvarjanja", opravilo.DatumUstvarjanja),
                new SqlParameter("@Opravljeno", opravilo.Opravljeno),
                new SqlParameter("@Id", opravilo.Id)
            ];

            await context.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public async Task DeleteOpraviloAsync(int id)
        {
            var sql = "DELETE FROM Opravila WHERE Id = @Id";
            var parameter = new SqlParameter("@Id", id);

            var result = await context.Database.ExecuteSqlRawAsync(sql, parameter);

            if (result == 0)
            {
                throw new KeyNotFoundException($"Opravilo z ID {id} ne obstaja.");
            }
        }
    }
}
