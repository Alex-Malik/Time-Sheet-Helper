using System.Threading.Tasks;

namespace TimeSheet.Interfaces
{
    public interface ISettingsService<TSettings> : IService where TSettings : ISettings
    {
        void Save(TSettings settings);
        Task SaveAsync(TSettings settings);
        TSettings Load();
        Task<TSettings> LoadAsync();
    }
}
