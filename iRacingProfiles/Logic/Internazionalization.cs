using iRacingProfiles.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace iRacingProfiles.Logic {
    class Internazionalization {

        private ResourceManager myManager;

        #region Singleton
        private static Internazionalization instance = null;

        public static Internazionalization Instance {
            get {
                if (instance == null) {
                    instance = new Internazionalization();
                }
                return instance;
            }
        }

        private Internazionalization() {
            myManager = new ResourceManager(typeof(Resources));
        }
        #endregion

        public string getValueFromKey(string key) {
            string result = myManager.GetString(key);
            if(result == null) {
                result = key;
            }
            return result;
        }
    }
}
