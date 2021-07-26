using System.Collections.Generic;
using System.ServiceModel;
using kn.web.core.Soap;

namespace kn.web.core.Soap
{
   [ServiceContract]
   public interface IService
   {
      [OperationContract]
      List<Models.Event> GetGPSEvents(Filter filter);
  }
}