// Services/RealtimeTicketService.cs
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using TicketApp.Models;
using TicketApp.Helpers;

namespace TicketApp.Services
{
    /// <summary>
    /// Gerçek zamanlı ticket takibi için servis sınıfı
    /// FileSystemWatcher kullanarak veritabanı değişikliklerini algılar
    /// </summary>
    public class RealtimeTicketService : IDisposable
    {
        #region Events

        /// <summary>
        /// Yeni ticket geldiğinde tetiklenir
        /// </summary>
        public event EventHandler<TicketEventArgs> NewTicketReceived;

        /// <summary>
        /// Ticket güncellendiğinde tetiklenir
        /// </summary>
        public event EventHandler<TicketEventArgs> TicketUpdated;

        /// <summary>
        /// Veritabanı değiştiğinde tetiklenir
        /// </summary>
        public event EventHandler DatabaseChanged;

        #endregion

        #region Fields

        private FileSystemWatcher _dbWatcher;
        private Timer _debounceTimer;
        private DateTime _lastDbUpdate = DateTime.Now;
        private List<Ticket> _previousTickets = new List<Ticket>();
        private bool _isRunning = false;
        private readonly object _lockObject = new object();

        #endregion

        #region Properties

        /// <summary>
        /// Servisin çalışıp çalışmadığı
        /// </summary>
        public bool IsRunning => _isRunning;

        /// <summary>
        /// Debounce süresi (ms)
        /// </summary>
        public int DebounceDelay { get; set; } = 500;

        #endregion

        #region Constructor & Destructor

        public RealtimeTicketService()
        {
            InitializeDebounceTimer();
        }

        ~RealtimeTicketService()
        {
            Dispose(false);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Servisi başlatır
        /// </summary>
        public void Start()
        {
            if (_isRunning) return;

            try
            {
                // İlk ticket listesini al
                _previousTickets = DatabaseHelper.GetAllTickets();

                // FileSystemWatcher'ı başlat
                InitializeFileSystemWatcher();

                _isRunning = true;
                Logger.Log("RealtimeTicketService başlatıldı.");
            }
            catch (Exception ex)
            {
                Logger.Log($"RealtimeTicketService başlatma hatası: {ex.Message}");
                throw new Exception("Gerçek zamanlı takip servisi başlatılamadı.", ex);
            }
        }

        /// <summary>
        /// Servisi durdurur
        /// </summary>
        public void Stop()
        {
            if (!_isRunning) return;

            try
            {
                if (_dbWatcher != null)
                {
                    _dbWatcher.EnableRaisingEvents = false;
                    _dbWatcher.Dispose();
                    _dbWatcher = null;
                }

                _isRunning = false;
                Logger.Log("RealtimeTicketService durduruldu.");
            }
            catch (Exception ex)
            {
                Logger.Log($"RealtimeTicketService durdurma hatası: {ex.Message}");
            }
        }

        /// <summary>
        /// Manuel olarak değişiklikleri kontrol eder
        /// </summary>
        public void CheckForChanges()
        {
            CheckDatabaseChanges();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// FileSystemWatcher'ı başlatır
        /// </summary>
        private void InitializeFileSystemWatcher()
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tickets.db");
            string dbDirectory = Path.GetDirectoryName(dbPath);
            string dbFileName = Path.GetFileName(dbPath);

            _dbWatcher = new FileSystemWatcher(dbDirectory)
            {
                Filter = dbFileName,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size,
                EnableRaisingEvents = true
            };

            _dbWatcher.Changed += OnDatabaseFileChanged;
            _dbWatcher.Created += OnDatabaseFileChanged;
        }

        /// <summary>
        /// Debounce timer'ı başlatır
        /// </summary>
        private void InitializeDebounceTimer()
        {
            _debounceTimer = new Timer();
            _debounceTimer.Interval = DebounceDelay;
            _debounceTimer.Tick += OnDebounceTimerTick;
        }

        /// <summary>
        /// Veritabanı dosyası değiştiğinde
        /// </summary>
        private void OnDatabaseFileChanged(object sender, FileSystemEventArgs e)
        {
            lock (_lockObject)
            {
                // Kendi değişikliklerimizi yoksay
                if (DateTime.Now.Subtract(_lastDbUpdate).TotalMilliseconds < 1000)
                    return;

                // Debounce timer'ı yeniden başlat
                _debounceTimer.Stop();
                _debounceTimer.Start();
            }
        }

        /// <summary>
        /// Debounce timer tetiklendiğinde
        /// </summary>
        private void OnDebounceTimerTick(object sender, EventArgs e)
        {
            _debounceTimer.Stop();
            CheckDatabaseChanges();
        }

        /// <summary>
        /// Veritabanındaki değişiklikleri kontrol eder
        /// </summary>
        private void CheckDatabaseChanges()
        {
            try
            {
                lock (_lockObject)
                {
                    // Güncel ticket listesini al
                    var currentTickets = DatabaseHelper.GetAllTickets();

                    // Yeni ticketları bul
                    var newTickets = currentTickets
                        .Where(ct => !_previousTickets.Any(pt => pt.Id == ct.Id))
                        .ToList();

                    // Güncellenen ticketları bul
                    var updatedTickets = currentTickets
                        .Where(ct => _previousTickets.Any(pt =>
                            pt.Id == ct.Id &&
                            (pt.Status != ct.Status ||
                             pt.AssignedTo != ct.AssignedTo ||
                             pt.IsResolved != ct.IsResolved)))
                        .ToList();

                    // Event'leri tetikle
                    if (newTickets.Any() || updatedTickets.Any())
                    {
                        DatabaseChanged?.Invoke(this, EventArgs.Empty);

                        foreach (var ticket in newTickets)
                        {
                            NewTicketReceived?.Invoke(this, new TicketEventArgs(ticket, TicketAction.Created));
                        }

                        foreach (var ticket in updatedTickets)
                        {
                            TicketUpdated?.Invoke(this, new TicketEventArgs(ticket, TicketAction.Updated));
                        }
                    }

                    // Listeyi güncelle
                    _previousTickets = currentTickets;
                    _lastDbUpdate = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Veritabanı değişiklik kontrolü hatası: {ex.Message}");
            }
        }

        #endregion

        #region IDisposable Implementation

        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                Stop();
                _debounceTimer?.Dispose();
            }

            _disposed = true;
        }

        #endregion
    }

    #region Event Arguments

    /// <summary>
    /// Ticket event argümanları
    /// </summary>
    public class TicketEventArgs : EventArgs
    {
        public Ticket Ticket { get; }
        public TicketAction Action { get; }
        public DateTime Timestamp { get; }

        public TicketEventArgs(Ticket ticket, TicketAction action)
        {
            Ticket = ticket;
            Action = action;
            Timestamp = DateTime.Now;
        }
    }

    /// <summary>
    /// Ticket aksiyonları
    /// </summary>
    public enum TicketAction
    {
        Created,
        Updated,
        Deleted,
        Assigned,
        Resolved
    }

    #endregion
}