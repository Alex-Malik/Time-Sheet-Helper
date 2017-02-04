using System.Threading.Tasks;

namespace TimeSheet.Interfaces
{
    public interface IInsertService : IService<IInsertSettings>
    {
        void Save(IData data);
        Task SaveAsync(IData data);
    }
}
