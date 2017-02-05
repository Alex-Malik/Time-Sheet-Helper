using System.Threading.Tasks;

namespace TimeSheet.Interfaces
{
    public interface IService
    {

    }

    public interface IService<T> where T : ISettings
    {
        T       LoadSettings();
        Task<T> LoadSettingsAsync();

        void SaveSettings(T settings);
        Task SaveSettingsAsync(T settings);
    }
}
