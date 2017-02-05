using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace TimeSheet.Services.Settings
{
    using Imps;
    using Interfaces;
    using System.Reflection;

    // TODO: Implementation of ISettingsService should implement explicetly
    // different instances of the ISettingsService<T>.

    internal class SettingsService : ISettingsService
    {
        public SettingsService()
        {
        }

        #region ISettingsService Support
        public void Save<T>(T settings) where T : class, ISettings
        {
            // Load application settings.
            AppSettings appset = LoadAppSettings();

            // Load or create settings file.
            Settings currentSettings = LoadSettings(appset.SettingsFilePath);

            // Check if settings type supported or throw an exception.
            if (currentSettings is T)
            {
                // Map properties of type T to the current settings object.
                MapProperties(currentSettings, settings);

                // Save settings object to the file.
                SaveOrCreate(currentSettings, appset.SettingsFilePath);
            }
            else
                throw new NotSupportedException(typeof(T).ToString());
        }

        public async Task SaveAsync<T>(T settings) where T : class, ISettings
        {
            // Load application settings.
            AppSettings appset = await LoadAppSettingsAsync();

            // Load or create settings file.
            Settings currentSettings = await LoadSettingsAsync(appset.SettingsFilePath);

            // Check if settings type supported or throw an exception.
            if (currentSettings is T)
            {
                // Map properties of type T to the current settings object.
                await MapPropertiesAsync(currentSettings, settings);

                // Save settings object to the file.
                await SaveOrCreateAsync(currentSettings, appset.SettingsFilePath);
            }
            else
                throw new NotSupportedException(typeof(T).ToString());
        }

        public T Load<T>() where T : class, ISettings
        {
            // Load application settings.
            AppSettings appset = LoadAppSettings();

            // Load or create settings file.
            Settings settings  = LoadSettings(appset.SettingsFilePath);

            // Check if settings type supported or throw an exception.
            if (settings is T) return settings as T;
            else
                throw new NotSupportedException(typeof(T).ToString());
        }

        public async Task<T> LoadAsync<T>() where T : class, ISettings
        {
            // Load application settings.
            AppSettings appset = await LoadAppSettingsAsync();

            // Load or create settings file.
            Settings settings  = await LoadSettingsAsync(appset.SettingsFilePath);

            // Check if settings type supported or throw an exception.
            if (settings is T) return settings as T;
            else
                throw new NotSupportedException(typeof(T).ToString());
        }
        #endregion

        #region Private Methods
        private T LoadOrCreate<T>(String filePath) where T : class
        {
            // Create directory if it not exists.
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            T result = null;

            // Read json data from file and convert it to the settings instance.
            using (var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read))
            using (var reader = new StreamReader(stream))
            {
                String json = reader.ReadToEnd();
                result = JsonConvert.DeserializeObject<T>(json);
            }

            return result;
        }

        private async Task<T> LoadOrCreateAsync<T>(String filePath) where T : class
        {
            // Create directory if it not exists.
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            T result = null;

            // Read json data from file and convert it to the settings instance.
            using (var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read))
            using (var reader = new StreamReader(stream))
            {
                String json = await reader.ReadToEndAsync();
                result = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(json));
            }

            return result ?? default(T);
        }

        private Settings LoadSettings(String filePath)
        {
            return LoadOrCreate<Settings>(filePath) ?? new Settings();
        }

        private Task<Settings> LoadSettingsAsync(String filePath)
        {
            return LoadOrCreateAsync<Settings>(filePath) ?? Task.FromResult(new Settings());
        }

        private AppSettings LoadAppSettings()
        {
            AppSettings settings = LoadOrCreate<AppSettings>($"{Environment.CurrentDirectory}\\app.json");
            if (settings == null)
            {
                String defSettingsFilePath = String.Empty;
                defSettingsFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                defSettingsFilePath = Path.Combine(defSettingsFilePath, "Time Sheet Helper/timesheet.json");

                settings = new AppSettings(defSettingsFilePath);
            }
            return settings;
        }

        private async Task<AppSettings> LoadAppSettingsAsync()
        {
            AppSettings settings = await LoadOrCreateAsync<AppSettings>($"{Environment.CurrentDirectory}\\app.json");
            if (settings == null)
            {
                String defSettingsFilePath = String.Empty;
                defSettingsFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                defSettingsFilePath = Path.Combine(defSettingsFilePath, "Time Sheet Helper/timesheet.json");

                settings = new AppSettings(defSettingsFilePath);
            }
            return settings;
        }
        
        private void MapProperties<T>(Settings currentSettings, T settings) where T : class, ISettings
        {
            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                property.SetValue(currentSettings, property.GetValue(settings));
            }
        }

        private Task MapPropertiesAsync<T>(Settings currentSettings, T settings) where T : class, ISettings
        {
            return Task.Factory.StartNew(() => 
            {
                foreach (PropertyInfo property in typeof(T).GetProperties())
                {
                    property.SetValue(currentSettings, property.GetValue(settings));
                }
            });
        }

        private void SaveOrCreate(Settings settings, String filePath)
        {
            // Create directory if it not exists.
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            
            using (var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
            using (var writer = new StreamWriter(stream))
            {
                String json = JsonConvert.SerializeObject(settings);
                writer.WriteLine(json);
            }
        }

        private async Task SaveOrCreateAsync(Settings settings, String filePath)
        {
            // Create directory if it not exists.
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            using (var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
            using (var writer = new StreamWriter(stream))
            {
                String json = await Task.Factory
                    .StartNew(() => JsonConvert.SerializeObject(settings));
                await writer.WriteLineAsync(json);
            }
        }
        #endregion
    }
}
