using FireSharp.Config;
using FireSharp.Interfaces;
namespace HocWeb.App_Start
{
    public class FirebaseContext
    {
        public IFirebaseClient Client;
        public FirebaseContext()
        {
            IFirebaseConfig conf = new FirebaseConfig
            {
                AuthSecret = "L5fwqNMqRzKyuo1MF0B0OYEuQ6zN9Hq6Maz618Nv",
                BasePath = "https://doan-d2374.firebaseio.com/"
            };
            Client = new FireSharp.FirebaseClient(conf);
        }        
    }
}