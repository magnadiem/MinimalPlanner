using System.ComponentModel;
using System.Timers;

namespace MinimalPlanner
{
    /// <summary>
    /// Свой таймер для привязки конкретного ивента к конкретному инстансу таймера
    /// </summary>
    public class EntryTimer : System.Timers.Timer
    {
        public Entry? Data;
    }

    /// <summary>
    /// Класс для управления уведомлениями по истечению таймера
    /// </summary>
    public class NotificationManager
    {
        private static NotificationManager? _instance;
        
        /// <summary>
        /// callback на таймер
        /// </summary>
        private ElapsedEventHandler? _handler;

        /// <summary>
        /// Ссылка на BindingList датасорс
        /// </summary>
        private BindingList<Entry>? _entries;

        /// <summary>
        /// Словарь таймеров {id ивента:таймер ивента} , 
        /// </summary>
        private Dictionary<int, EntryTimer> _timers;

        /// <summary>
        /// 5 минут в мс
        /// </summary>
        private const double notificationThreshold = 300000;

        private static NotificationManager Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new NotificationManager();
                return _instance;
            }
        }

        /// <summary>
        /// callback на удаление таймера из словаря
        /// </summary>
        private static ElapsedEventHandler ElapsedHandler = (sender, e) =>
        {
            RemoveTimer(((EntryTimer)sender).Data.Id);
        };

        private NotificationManager()
        {
            _timers = new Dictionary<int, EntryTimer>();
        }

        /// <summary>
        /// Вызывает callback, передавая в него ивент
        /// </summary>
        /// <param name="handler"></param>
        public static void RegisterHandler(Action<Entry> handler)
        {
            Instance._handler = (sender, e) => handler(((EntryTimer)sender).Data);
        }

        public static void RegisterDataSource(BindingList<Entry> bl)
        {
            Instance._entries = bl;
        }

        private static void AddTimer(Entry entry, double timeLeft)
        {
            var timer = new EntryTimer
            {
                Interval = timeLeft > notificationThreshold ? timeLeft - notificationThreshold : 500,
                Data = entry,
                AutoReset = false,
                Enabled = true
            }; 
            timer.Elapsed += Instance._handler;
            timer.Elapsed += ElapsedHandler;
            timer.Start();
            Instance._timers.Add(entry.Id, timer);
        }

        private static void EditTimer(Entry e)
        {
            if (Instance._timers.ContainsKey(e.Id))
                Instance._timers[e.Id].Interval = DateDiff(e.TimeOfEvent);
            else
                AddDate(e);
        }

        private static void RemoveTimer(int entryId)
        {
            if (Instance._timers.ContainsKey(entryId))
            {
                Instance._timers[entryId].Stop();
                Instance._timers.Remove(entryId);
            }
        }

        /// <summary>
        /// Вешает таймеры из БД на первом запуске
        /// </summary>
        public static void ParseDate()
        {
            var now = DateTime.Now;
            foreach (var e in Instance._entries)
            {
                var dt = e.TimeOfEvent - now;
                if (dt.TotalMinutes >= 0)
                {
                    AddTimer(e, dt.TotalMilliseconds);
                }
            }
        }

        public static void AddDate(Entry e)
        {
            AddTimer(e, DateDiff(e.TimeOfEvent));
        }

        public static void EditDate(Entry e)
        {
            if (DateDiff(e.TimeOfEvent) > 0)
                EditTimer(e);
        }

        public static void RemoveDate(Entry e)
        {
            RemoveTimer(e.Id);
        }

        private static double DateDiff(DateTime dt)
        {
            return (dt - DateTime.Now).TotalMilliseconds;
        }
    }
}
