using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheet.Shared
{
    using Interfaces;

    public class ControlsRouter : IControlsRouter
    {
        #region Singleton Implementation
        public static readonly ControlsRouter Instance = new ControlsRouter();
        private ControlsRouter() { }
        #endregion Singleton Implementation

        private readonly Dictionary<Type, List<IControl>> _controls = new Dictionary<Type, List<IControl>>();

        public void Register<T>(T control) where T: class, IControl
        {
            Type type = typeof(T);
            if (!_controls.ContainsKey(type))
                 _controls.Add(type, new List<IControl>());
            _controls[type].Add(control);
        }

        public void Unregister<T>(T control) where T : class, IControl
        {
            throw new NotImplementedException();
        }

        public T Get<T>() where T : class, IControl
        {
            Type type = typeof(T);
            if (_controls.ContainsKey(type))
                return _controls[type].Last() as T;
            else
                return null;
        }
    }
}
