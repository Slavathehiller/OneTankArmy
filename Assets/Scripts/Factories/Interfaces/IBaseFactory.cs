using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Interfaces
{
    public interface IBaseFactory
    {
        public T Create<T>(bool cached = true);
        public T CreateNotCached<T>();
    }
}
