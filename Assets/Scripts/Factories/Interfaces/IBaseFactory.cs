using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Factories.Interfaces
{
    public interface IBaseFactory
    {
        public T Create<T>(bool cached = true);
        public T CreateNotCached<T>();
    }
}
