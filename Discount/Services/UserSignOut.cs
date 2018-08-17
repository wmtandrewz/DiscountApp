using System;
using Xamarin.Forms;

namespace Discount.Services
{
    public class UserSignOut
    {
        public void SignOut()
        {
            MessagingCenter.Send<UserSignOut>(this, "signout");
        }
    }
}
