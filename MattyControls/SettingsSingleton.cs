using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MattyControls
{
    public class SettingsSingleton
    {
        protected virtual string Name => "settings";
        protected virtual string Path => Application.StartupPath + System.IO.Path.DirectorySeparatorChar + Name + ".ini";

        /// <summary>
        /// The settings instance
        /// </summary>
        protected static SettingsSingleton instance;
        /// <summary>
        /// The instance of the settings singleton
        /// </summary>
        public static T GetSingleton<T>() where T : SettingsSingleton, new() {
            return SettingsSingleton.instance == null ? (T)(SettingsSingleton.instance = new T()) : (T)SettingsSingleton.instance;
        }

        /// <summary>
        /// The list with all settings.
        /// </summary>
        protected Dictionary<string, string> hashList;

        protected SettingsSingleton() {
            this.SetDefaults();
        }

        public override string ToString() {
            return this.ToString(null);
        }
        public string ToString(char separator) {
            return this.ToString(null, separator);
        }
        public string ToString(List<string> properties, char separator = ';') {
            return this.ToString(properties, separator, Environment.NewLine);
        }
        public string ToString(List<string> properties, char separator, string endSeperator) {
            // Write all hashlist values with keys in the properties list to a string, write all if the propertieslist is left null
            StringBuilder s = new StringBuilder();
            foreach (var tuple in this.hashList) {
                if (properties == null || properties.Contains(tuple.Key)) {
                    s.Append(tuple.Key + separator + tuple.Value + endSeperator);
                }
            }
            return s.ToString();
        }

        public void FromString(string s, char separator = ';') {
            this.FromString(s, separator, Environment.NewLine);
        }
        public void FromString(string s, char separator, string endSeperator) {
            // Override hashlist with values from s (values not in s will be left intact)
            string[] lines = s.Split(endSeperator.ToCharArray());
            for (int i = 0; i < lines.Length; i++) {
                string[] keyVal = lines[i].Split(separator);
                if (keyVal[0] != "") {
                    this.hashList[keyVal[0]] = keyVal[1];
                }
            }
        }

        /// <summary>
        /// Load the settings from file
        /// </summary>
        /// <returns>Whether there was an error loading</returns>
        public bool Load() {
            bool noError = false;

            // If the file doesnt exist, load the defaults
            if (!File.Exists(this.Path)) {
                return false;
            }

            try {
                using (StreamReader file = new StreamReader(this.Path)) {
                    this.FromString(file.ReadToEnd());
                    noError = true;
                }
            }
            catch {
                return false;
            }

            return noError;
        }

        /// <summary>
        /// Save the settings to file
        /// </summary>
        /// <returns>Whether there was an error saving</returns>
        public bool Save() {
            bool noError = false;

            try {
                using (StreamWriter file = new StreamWriter(this.Path)) {
                    file.WriteLine(this.ToString());
                    noError = true;
                }
            }
            catch {
                return false;
            }

            return noError;
        }

        /// <summary>
        /// Set the default values.
        /// </summary>
        public void SetDefaults() {
            this.hashList = new Dictionary<string, string>();
        }

        public Point Position {
            get { return this.get("position", Point.Empty); }
            set { this.set("position", value); }
        }

        public Size Size {
            get { return this.get("size", Size.Empty); }
            set { this.set("size", value); }
        }

        #region Getters, setters and converters

        // String
        protected string get(string key, string defaultValue) {
            if (!this.hashList.ContainsKey(key)) {
                this.set(key, defaultValue);
            }
            return this.hashList[key];
        }
        protected void set(string key, string value) {
            this.hashList[key] = value;
        }

        // Bool
        protected bool get(string key, bool defaultValue) {
            return bool.Parse(this.get(key, defaultValue.ToString()));
        }
        protected void set(string key, bool value) {
            this.set(key, value.ToString());
        }

        // Int
        protected int get(string key, int defaultValue) {
            return int.Parse(this.get(key, defaultValue.ToString()));
        }
        protected void set(string key, int value) {
            this.set(key, value.ToString());
        }

        // Long
        protected long get(string key, long defaultValue) {
            return long.Parse(this.get(key, defaultValue.ToString()));
        }
        protected void set(string key, long value) {
            this.set(key, value.ToString());
        }

        // Float
        protected float get(string key, float defaultValue) {
            return float.Parse(this.get(key, defaultValue.ToString()));
        }
        protected void set(string key, float value) {
            this.set(key, value.ToString());
        }

        // Double
        protected double get(string key, double defaultValue) {
            return double.Parse(this.get(key, defaultValue.ToString()));
        }
        protected void set(string key, double value) {
            this.set(key, value.ToString());
        }

        // Point
        protected Point get(string key, Point defaultValue) {
            return Str2Vec(this.get(key, Vec2Str(defaultValue)));
        }
        protected void set(string key, Point value) {
            this.set(key, Vec2Str(value));
        }

        // Size
        protected Size get(string key, Size defaultValue) {
            return new Size(this.get(key, new Point(defaultValue)));
        }
        protected void set(string key, Size value) {
            this.set(key, new Point(value));
        }

        // Color
        protected Color get(string key, Color defaultValue) {
            return Str2Color(this.get(key, Color2Str(defaultValue)));
        }
        protected void set(string key, Color value) {
            this.set(key, Color2Str(value));
        }

        // String[], List<string>, IEnumerable<string>, ...
        protected string[] get(string key, string[] defaultValue, string separator = ",") {
            return this.get(key, defaultValue).ToArray();
        }
        protected List<string> get(string key, List<string> defaultValue, string separator = ",") {
            return this.get(key, defaultValue).ToList();
        }
        protected IEnumerable<string> get(string key, IEnumerable<string> defaultValue, string separator = ",") {
            return this.get(key, string.Join(separator, defaultValue)).Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
        }
        protected void set(string key, IEnumerable<string> value, string separator = ",") {
            this.set(key, string.Join(separator, value));
        }

        // Int[], List<int>, IEnumerable<int>, ...
        protected int[] get(string key, int[] defaultValue, string separator = ",") {
            return this.get(key, defaultValue, separator).ToArray();
        }
        protected List<int> get(string key, List<int> defaultValue, string separator = ",") {
            return this.get(key, defaultValue, separator).ToList();
        }
        protected IEnumerable<int> get(string key, IEnumerable<int> defaultValue, string separator = ",") {
            return this.get(key, defaultValue.Select(i => i.ToString())).Select(int.Parse).ToArray();
        }
        protected void set(string key, IEnumerable<int> value, string separator = ",") {
            this.set(key, string.Join<int>(separator, value));
        }

        /// <summary>
        /// Parse an vector2 (with integer values) to a string
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string Vec2Str(Point v, char separator = ',') {
            return ((int)v.X).ToString() + separator + ((int)v.Y).ToString();
        }
        /// <summary>
        /// Parse a string to a vector2 (with integer values)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static Point Str2Vec(string s, char separator = ',') {
            string[] vs = s.Split(separator);
            return new Point(int.Parse(vs[0]), int.Parse(vs[1]));
        }

        /// <summary>
        /// Parse an vector2 (with floating point values) to a string
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string Vec2StrF(Point v, char separator = ',') {
            return v.X.ToString() + separator + v.Y.ToString();
        }
        /// <summary>
        /// Parse a string to a vector2 (with floating point values)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static Point Str2VecF(string s, char separator = ',') {
            string[] vs = s.Split(separator);
            return new Point(int.Parse(vs[0]), int.Parse(vs[1]));
        }

        /// <summary>
        /// Parse a color to a string
        /// </summary>
        /// <param name="c"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string Color2Str(Color c, char separator = ',') {
            return c.A.ToString() + separator + c.R.ToString() + separator + c.G.ToString() + separator + c.B.ToString();
        }

        /// <summary>
        /// Parse a string to a color
        /// </summary>
        /// <param name="s"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static Color Str2Color(string s, char separator = ',') {
            string[] cs = s.Split(separator);
            if (cs.Length == 3) {
                return Color.FromArgb(int.Parse(cs[0]), int.Parse(cs[1]), int.Parse(cs[2]));
            }
            return Color.FromArgb(int.Parse(cs[0]), int.Parse(cs[1]), int.Parse(cs[2]), int.Parse(cs[3]));
        }

        #endregion
    }
}
