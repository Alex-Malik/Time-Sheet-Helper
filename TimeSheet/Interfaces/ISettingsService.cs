using System.Threading.Tasks;

namespace TimeSheet.Interfaces
{
    public interface ISettingsService : IService
    {
        // Saving methods.
        void Save<T>(T settings)      where T : class, ISettings;
        Task SaveAsync<T>(T settings) where T : class, ISettings;

        // Loading methods.
        T       Load<T>()      where T : class, ISettings;
        Task<T> LoadAsync<T>() where T : class, ISettings;
    }
}
