# mattycontrols-csharp
A wrapper around some windows forms controls.

This library enables me to simply create windows forms applications without using a designer.
It provides easy positioning and scaling for the controls, a small framework to show/hide usercontrols on a form and a
way to store some settings of the application in a `.ini` file.


## Example project

_Program.cs:_

```
using System.Windows.Forms;

namespace ExampleProject
{
    static class Program
    {
        static void Main(string[] args)
        {
            Settings.Get.Load();

            Main main = new Main();

            Application.EnableVisualStyles();
            Application.Run(main);

            Settings.Get.Save();
        }
    }
}
```

_Settings.cs:_

```
using MattyControls;

namespace ExampleProject
{
    class Settings : SettingsSingleton
    {
        protected override string Name => "ExampleProjectSettings";

        public static Settings Get => SettingsSingleton.GetSingleton<Settings>();

        public string SomeText
        {
            get { return this.get("some-string", "default text"); }
            set { this.set("some-string", value); }
        }
    }
}
```

_Main.cs:_

```
using System.Drawing;
using MattyControls;

namespace ExampleProject
{
    class Main : MattyForm
    {
        private static Size MIN_SIZE = new Size(220, 150);
        private static Size DEFAULT_SIZE = new Size(300, 200);

        public Main() : base(MIN_SIZE, DEFAULT_SIZE, Settings.Get)
        {
            this.Text = "Example application";

            this.AddUserControl(new MainControl(), new SettingsControl());
            this.ShowUserControl<MainControl>();
        }
    }
}
```

_MainControl.cs:_

```
using System;
using MattyControls;

namespace ExampleProject
{
    class MainControl : MattyUserControl
    {
        Btn btnSettings;
        Tb tbContent;

        public MainControl()
        {
            this.btnSettings = new Btn("Settings", this);
            this.btnSettings.Click += (o, e) =>
            {
                this.ShowUserControl<SettingsControl>();
            };

            this.tbContent = new Tb(this);
            this.tbContent.Multiline = true;
            this.tbContent.ReadOnly = true;
        }

        public override void OnResize()
        {
            this.btnSettings.PositionBottomRightInside(this);

            this.tbContent.PositionTopLeftInside(this);
            this.tbContent.StretchRightInside(this);
            this.tbContent.StretchDownTo(this.btnSettings);
        }

        public override void OnShow()
        {
            this.tbContent.Text = "This is the main control of the example project." +
                $"{Environment.NewLine}{Environment.NewLine}{Settings.Get.SomeText}";
        }
    }
}
```

_SettingsControl.cs:_

```
using MattyControls;

namespace ExampleProject
{
    class SettingsControl : MattyUserControl
    {
        Btn btnSave, btnCancel;
        Tb tbSomeText;

        public SettingsControl()
        {
            this.tbSomeText = new Tb(this);

            this.btnSave = new Btn("Save", this);
            this.btnSave.Click += (o, e) =>
            {
                Settings.Get.SomeText = this.tbSomeText.Text;
                this.ShowUserControl<MainControl>();
            };

            this.btnCancel = new Btn("Cancel", this);
            this.btnCancel.Click += (o, e) =>
            {
                this.ShowUserControl<MainControl>();
            };
        }

        public override void OnResize()
        {
            this.tbSomeText.PositionTopLeftInside(this);
            this.tbSomeText.AddLabel("Enter some text:");
            this.tbSomeText.StretchRightInside(this);

            this.btnCancel.PositionBottomRightInside(this);
            this.btnSave.PositionLeftOf(this.btnCancel);
        }

        public override void OnShow()
        {
            this.tbSomeText.Text = Settings.Get.SomeText;

            this.tbSomeText.Focus();
        }
    }
}
```
