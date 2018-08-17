using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Discount.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        public static string BaseAPIUrl
        {
            get
            {
                return AppSettings.GetValueOrDefault("BaseAPIUrl", "http://chml.keells.lk/");
            }
            set
            {
                AppSettings.AddOrUpdateValue("BaseAPIUrl", value);
            }
        }

        public static string Username
        {
            get
            {
                return AppSettings.GetValueOrDefault("Username", string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue("Username", value);
            }
        }

        public static string Password
        {
            get
            {
                return AppSettings.GetValueOrDefault("Password", string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue("Password", value);
            }
        }

        public static string AccessToken
        {
            get
            {
                return AppSettings.GetValueOrDefault("AccessToken", string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue("AccessToken", value);
            }
        }

        public static string ExpiresTime
        {
            get
            {
                return AppSettings.GetValueOrDefault("ExpiresTime", string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue("ExpiresTime", value);
            }
        }
    }
}
