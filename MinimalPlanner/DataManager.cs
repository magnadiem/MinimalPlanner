using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace MinimalPlanner
{
    /// <summary>
    /// Прослойка для записи в БД
    /// </summary>
    public class DataManager
    {
        private static DataManager? _instance;
        private DBContext _db;

        /// <summary>
        /// По совместительству хранит в себе BindingList для остальных
        /// </summary>
        private BindingList<Entry>? _bl;

        private static DataManager Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new DataManager();
                return _instance;
            }
        }

        private DataManager()
        {
            _db = new DBContext();
        }

        private static void Save()
        {
            Instance._db.SaveChanges();
        }

        public static BindingList<Entry> GetBindingList()
        {
            if (Instance._bl is null)
            {
                Instance._db.Entries.Load();
                Instance._bl = Instance._db.Entries.Local.ToBindingList();
            }
            return Instance._bl;
        }

        public static void Remove(int id)
        {
            Instance._db.Entries.Remove(
                Instance._db.Entries.First(e => e.Id == id));
            Save();
        }

        public static void Update(int id, Entry entry)
        {
            var current = Instance._db.Entries.First(e => e.Id == id);
            current.Description = entry.Description;
            current.TimeOfEvent = entry.TimeOfEvent;
            Save();
        }

        public static void Add(Entry entry)
        {
            Instance._db.Entries.Add(entry);
            Save();
        }
    }
}