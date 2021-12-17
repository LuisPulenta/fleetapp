using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Fleet_App.Common.Helpers
{
    public static class Settings
    {
        private const string _user = "User";
        private const string _remote = "Remote";
        private const string _cable = "Cable";
        private const string _tasa = "Tasa";
        private const string _modem = "Modem";
        private const string _isRemembered = "IsRemembered";
        private const string _ingreso = "Ingreso";
        private static readonly bool _boolDefault = false;

        private static readonly string _settingsDefault = string.Empty;
        private static string _dtv = "Dtv";
        private static string _tlc = "Tlc";
        private static string _prisma = "Prisma";


        private static ISettings AppSettings => CrossSettings.Current;

        public static string User2
        {
            get => AppSettings.GetValueOrDefault(_user, _settingsDefault);
            set => AppSettings.AddOrUpdateValue(_user, value);
        }
        public static string Remote
        {
            get => AppSettings.GetValueOrDefault(_remote, _settingsDefault);
            set => AppSettings.AddOrUpdateValue(_remote, value);
        }

        public static string Cable
        {
            get => AppSettings.GetValueOrDefault(_cable, _settingsDefault);
            set => AppSettings.AddOrUpdateValue(_cable, value);
        }
        public static string Dtv
        {
            get => AppSettings.GetValueOrDefault(_dtv, _settingsDefault);
            set => AppSettings.AddOrUpdateValue(_dtv, value);
        }
        public static string Tasa
        {
            get => AppSettings.GetValueOrDefault(_tasa, _settingsDefault);
            set => AppSettings.AddOrUpdateValue(_tasa, value);
        }
        public static string Modem
        {
            get => AppSettings.GetValueOrDefault(_modem, _settingsDefault);
            set => AppSettings.AddOrUpdateValue(_modem, value);
        }

        public static bool IsRemembered
        {
            get => AppSettings.GetValueOrDefault(_isRemembered, _boolDefault);
            set => AppSettings.AddOrUpdateValue(_isRemembered, value);
        }

        public static string Ingreso
        {
            get => AppSettings.GetValueOrDefault(_ingreso, _settingsDefault);
            set => AppSettings.AddOrUpdateValue(_ingreso, value);
        }
        public static string Tlc
        {
            get => AppSettings.GetValueOrDefault(_tlc, _settingsDefault);
            set => AppSettings.AddOrUpdateValue(_tlc, value);
        }
        public static string Prisma
        {
            get => AppSettings.GetValueOrDefault(_prisma, _settingsDefault);
            set => AppSettings.AddOrUpdateValue(_prisma, value);
        }
    }
}