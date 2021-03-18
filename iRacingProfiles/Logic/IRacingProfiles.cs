using iRacingProfiles.Components;
using iRacingProfiles.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace iRacingProfiles.Logic {
    class IRacingProfiles {
        public Config Config { get; set; }

        #region Singleton
        private IRacingProfiles() {
            LoadConf();
        }
        private static IRacingProfiles instance = null;
        public static IRacingProfiles Instance {
            get {
                if (instance == null) {
                    instance = new IRacingProfiles();
                }
                return instance;
            }
        }
        #endregion

        #region Configuration
        private void LoadConf() {
            Config = FileUtils.ReadFromXmlFile<Config>(Constants.GetAppConfRoute());
            if (Config == null) {
                Config = new Config {
                    Profiles = new List<Profile>(),
                    IRacingFolder = Constants.GetDefaultIracingRoute()
                };
            }
        }

        private void SaveConf() {
            FileUtils.CreateDirectory(Constants.GetAppRoute());
            FileUtils.WriteToXmlFile<Config>(Constants.GetAppConfRoute(), Config);
        }
        #endregion


        #region Profiles
        public void AddProfiles(Profile profile) {
            CheckProfile(profile);
            SaveProfile(profile);
            Config.Profiles.Add(profile);
            SaveConf();
        }

        private void SaveProfile(Profile profile) {
            string dirRoute = Path.Combine(Constants.GetAppRoute(), profile.Name);
            FileUtils.CreateDirectory(dirRoute);
            string finalRoute = Path.Combine(dirRoute, Constants.IRACING_CONF_NAME);
            FileUtils.CopyFile(profile.Content, finalRoute);
            profile.Content = finalRoute;
        }

        public void DeleteProfile(Profile profile) {
            string dirRoute = Path.Combine(Constants.GetAppRoute(), profile.Name);
            FileUtils.DeleteDirectory(dirRoute);
            Config.Profiles.Remove(profile);
            SaveConf();
        }

        public void CheckProfile(Profile profile) {
            if (String.IsNullOrEmpty(profile.Name)) {
                throw new iRacingProfileException("exception_profile_empty_name");
            }
            if (GetProfilesFromName(profile.Name) != null) {
                throw new iRacingProfileException("exception_profile_exists");
            }
            if (Regex.IsMatch(profile.Name, @"[\.\\\/]")) {
                throw new iRacingProfileException("exception_profile_name");
            }
        }
        
        public void LoadProfile(Profile profile) {
            string finalRoute = Path.Combine(Config.IRacingFolder, Constants.IRACING_CONF_NAME);
            FileUtils.CopyFile(profile.Content, finalRoute);
        }

        public List<Profile> GetProfiles() {
            return new List<Profile>(Config.Profiles);
        }

        public Profile GetProfilesFromName(string name) {
            return Config.Profiles.Where(x => x.Name == name).FirstOrDefault();
        }

        public void SelectProfile(Profile profile) {
            string finalRoute = Path.Combine(Config.IRacingFolder, Constants.IRACING_CONF_NAME);
            FileUtils.CopyFile(profile.Content, finalRoute);
            profile.Content = finalRoute;
        }
        #endregion


        #region IRacingRoute
        public void SetIRacingRoute(string path) {
            FileUtils.CheckDirectory(path);
            Config.IRacingFolder = path;
            SaveConf();
        }
        #endregion
        
        public void SetLocalization(string local) {
            Config.localization = local;
            SaveConf();
        }
    }
}
