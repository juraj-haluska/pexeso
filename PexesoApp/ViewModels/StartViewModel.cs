using Caliburn.Micro;
using Action = System.Action;

namespace PexesoApp.ViewModels
{
    public class StartViewModel : Screen
    {
        public Action Register { get; set; }

        public Action Login { get; set; }

        public void OnRegister()
        {
            Register?.Invoke();
        }

        public void OnLogin()
        {
            Login?.Invoke();
        }
    }
}