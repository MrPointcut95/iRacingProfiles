using System;
using System.Collections.Generic;

namespace iRacingProfiles.Components {
    public class Config {

        public string IRacingFolder { get; set; }

        public string localization { get; set; }

        public List<Profile> Profiles { get; set; }

        public bool CheckUpdates { get; set; }
        
    }
}
