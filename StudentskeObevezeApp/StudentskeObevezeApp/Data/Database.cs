using SQLite;
using StudentskeObevezeApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentskeObevezeApp.Data
{
    public class Database // ← OVDE JE BILA PROMENA (umesto internal)
    {
        readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Zadaci>().Wait();  // Tvoja model klasa
        }

        public Task<List<Zadaci>> GetItemsAsync()
        {
            return _database.Table<Zadaci>().ToListAsync();
        }

        public Task<int> SaveItemAsync(Zadaci item)
        {
            if (item.Id != 0)
                return _database.UpdateAsync(item);
            else
                return _database.InsertAsync(item);
        }

        public Task<int> DeleteItemAsync(Zadaci item)
        {
            return _database.DeleteAsync(item);
        }
    }
}
