using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace TimeSheet.Services.Settings
{
    using Imps;
    using Interfaces;

    // TODO: Implementation of ISettingsService should implement explicetly
    // different instances of the ISettingsService<T>.

    internal class SettingsService : 
        ISettingsService<IApplicationSettings>,
        ISettingsService<IInsertSettings>,
        ISettingsService<ISheetsSettings>
    {
        public SettingsService()
        {
        }

        #region IApplicationSettings Support
        IApplicationSettings ISettingsService<IApplicationSettings>.Load() => LoadAppSettings();
        Task<IApplicationSettings> ISettingsService<IApplicationSettings>.LoadAsync()
        {
            throw new NotImplementedException();
        }
        void ISettingsService<IApplicationSettings>.Save(IApplicationSettings settings) => SaveAppSettings(settings as ApplicationSettings);
        Task ISettingsService<IApplicationSettings>.SaveAsync(IApplicationSettings settings)
        {
            throw new NotImplementedException();
        }
        #endregion IApplicationSettings Support

        #region IInsertSettings Support
        IInsertSettings ISettingsService<IInsertSettings>.Load() => Load();
        async Task<IInsertSettings> ISettingsService<IInsertSettings>.LoadAsync() => await LoadAsync();
        void ISettingsService<IInsertSettings>.Save(IInsertSettings settings) => Save(settings as Settings);
        async Task ISettingsService<IInsertSettings>.SaveAsync(IInsertSettings settings) => await SaveAsync(settings as Settings);
        #endregion IInsertSettings Support

        #region ISheetsSettings Support
        ISheetsSettings ISettingsService<ISheetsSettings>.Load() => Load();
        async Task<ISheetsSettings> ISettingsService<ISheetsSettings>.LoadAsync() => await LoadAsync();
        void ISettingsService<ISheetsSettings>.Save(ISheetsSettings settings) => Save(settings as Settings);
        async Task ISettingsService<ISheetsSettings>.SaveAsync(ISheetsSettings settings) => await SaveAsync(settings as Settings);
        #endregion ISheetsSettings Support

        #region Private Methods
        private Settings Save(Settings settings = null)
        {
            if (settings == null)
                settings = new Settings();

            // Load application settings.
            ApplicationSettings app = LoadAppSettings();
            using (var stream = new FileStream(app.SettingsFilePath, FileMode.Create, FileAccess.Write))
            using (var writer = new StreamWriter(stream))
            {
                String json = JsonConvert.SerializeObject(settings);
                writer.WriteLine(json);
            }

            return settings;
        }

        private async Task<Settings> SaveAsync(Settings settings = null)
        {
            if (settings == null)
                settings = new Settings();

            // Load application settings.
            ApplicationSettings app = LoadAppSettings();
            using (var stream = new FileStream(app.SettingsFilePath, FileMode.Create, FileAccess.Write))
            using (var writer = new StreamWriter(stream))
            {
                String json = await Task.Factory
                    .StartNew(() => JsonConvert.SerializeObject(settings));
                await writer.WriteLineAsync(json);
            }

            return settings;
        }

        private Settings Load()
        {
            // Load application settings.
            ApplicationSettings app = LoadAppSettings();

            // Create directory in case when it's not exists.
            Directory.CreateDirectory(Path.GetDirectoryName(app.SettingsFilePath));

            String json = String.Empty;
            Settings settings = null;

            // Read json data from file and convert it to the settings instance.
            using (var stream = new FileStream(app.SettingsFilePath, FileMode.OpenOrCreate, FileAccess.Read))
            using (var reader = new StreamReader(stream))
            {
                json = reader.ReadToEnd();
                settings = JsonConvert.DeserializeObject<Settings>(json);
            }

            if (settings == null)
                return Save();
            else
                return settings;
        }

        private async Task<Settings> LoadAsync()
        {
            // Load application settings.
            ApplicationSettings app = LoadAppSettings();

            // Create directory in case when it's not exists.
            Directory.CreateDirectory(Path.GetDirectoryName(app.SettingsFilePath));

            String json = String.Empty;
            Settings settings = null;

            // Read json data from file and convert it to the settings instance.
            using (var stream = new FileStream(app.SettingsFilePath, FileMode.OpenOrCreate, FileAccess.Read))
            using (var reader = new StreamReader(stream))
            {
                json = await reader.ReadToEndAsync();
                settings = await Task.Factory
                    .StartNew(() => JsonConvert.DeserializeObject<Settings>(json));
            }

            if (settings == null)
                return await SaveAsync();
            else
                return settings;
        }

        private void SaveAppSettings(ApplicationSettings settings = null)
        {
            if (settings == null)
            {
                String defSettingsFilePath = String.Empty;
                defSettingsFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                defSettingsFilePath = Path.Combine(defSettingsFilePath, "Time Sheet Helper/timesheet.json");
                settings = new ApplicationSettings
                {
                    SettingsFilePath = defSettingsFilePath
                };
            }

            // Build path to application settings file.
            String path = $"{Environment.CurrentDirectory}\\app.json";

            // Read json data from file and convert it to the settings instance.
            using (var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            using (var writer = new StreamWriter(stream))
            {
                String json = JsonConvert.SerializeObject(settings);
                writer.WriteLine(json);
            }
        }

        private ApplicationSettings LoadAppSettings()
        {
            // Build path to application settings file.
            String path = $"{Environment.CurrentDirectory}\\app.json";

            if (!File.Exists(path)) SaveAppSettings();

            // Read json data from file and convert it to the settings instance.
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(stream))
            {
                String json = reader.ReadToEnd();
                ApplicationSettings settings =
                    JsonConvert.DeserializeObject<ApplicationSettings>(json);

                return settings;
            }
        }
        #endregion
    }
}
