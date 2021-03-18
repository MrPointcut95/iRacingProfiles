using System;
using System.IO;

namespace iRacingProfiles.Logic {
    static class Constants {
        public const string APP_DIR_NAME = "iRacingProfiles";
        public const string APP_CONF_NAME = "iracingprofiles.conf";
        public const string IRACING_CONF_NAME = "controls.cfg";

        public static string GetAppRoute() {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), APP_DIR_NAME);
        }

        public static string GetAppConfRoute() {
            return Path.Combine(GetAppRoute(), APP_CONF_NAME);
        }

        public static string GetDefaultIracingRoute() {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "iRacing");
        }
    }
}
