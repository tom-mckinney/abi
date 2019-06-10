using System;
using System.Collections.Generic;
using System.Text;

namespace Abi.Services
{
    public interface ITrackingService
    {
        void CreateEncounterAsync();
    }

    public class TrackingService : ITrackingService
    {
        public void CreateEncounterAsync()
        {
            throw new NotImplementedException();
        }
    }
}
