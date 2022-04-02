using KhdoumMobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KhdoumMobile.Interfaces
{
    public interface IStateService
    {
        Task<IEnumerable<State>> GetStates(int CityId);
    }
}
