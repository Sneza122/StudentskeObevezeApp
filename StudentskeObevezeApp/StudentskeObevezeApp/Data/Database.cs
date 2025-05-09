using SQLite;
using StudentskeObevezeApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentskeObevezeApp.Data
{
    public class Database
    {
        readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Zadaci>().Wait();  // Tvoja model klasa
            _database.CreateTableAsync<Beleska>().Wait();
            _database.CreateTableAsync<Ispit>().Wait();
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
            return _database.DeleteAsync(item);  // Ovo je ispravna metoda

        }
        public Task<int> DeleteAllItemsAsync()
        {
            return _database.DeleteAllAsync<Zadaci>();
        }

        public Task<List<Beleska>> GetBeleskeAsync()
        {
            return _database.Table<Beleska>().OrderByDescending(b => b.Datum).ToListAsync();
        }

        public Task<int> SacuvajBeleskuAsync(Beleska beleska)
        {
            if (beleska.Id != 0)
                return _database.UpdateAsync(beleska);
            else
                return _database.InsertAsync(beleska);
        }

        public Task<int> ObrisiBeleskuAsync(Beleska beleska)
        {
            return _database.DeleteAsync(beleska);
        }

        public Task<List<Ispit>> GetIspitiAsync()
        {
            return _database.Table<Ispit>().OrderByDescending(i => i.DatumIspita).ToListAsync();
        }

        public Task<int> SacuvajIspitAsync(Ispit ispit)
        {
            if (ispit.Id != 0)
                return _database.UpdateAsync(ispit);
            else
                return _database.InsertAsync(ispit);
        }

        public Task<int> ObrisiIspitAsync(Ispit ispit)
        {
            return _database.DeleteAsync(ispit);
        }


    }
}
