using System.Threading.Tasks;

namespace TimeSheet.Interfaces
{
    public interface IService
    {

    }

    public interface IService<TSettings> where TSettings : ISettings
    {
        TSettings LoadSettings();
        Task<TSettings> LoadSettingsAsync();
        void SaveSettings(TSettings settings);
        Task SaveSettingsAsync(TSettings settings);
    }
}
