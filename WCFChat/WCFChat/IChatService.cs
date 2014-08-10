using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFChat
{
    [ServiceContract(SessionMode = SessionMode.Required, 
        CallbackContract = typeof(IChatCallback))]
    public interface IChatService
    {
        [OperationContract]
        bool Connect(Person p);
    }
}
