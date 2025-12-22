using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mika.DevTest;

internal class Program
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private FontSystem _fontSystem;

        private Context mika;
        private Icons.IconCollection icons;

        private Texture2D testSprite1;

        private int screenWidth = 1600;
        private int screenHeight = 900;

        bool checkbox = false;

        Color bg = new Color(55, 69, 97);

        List<string> messages = [];

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = screenWidth;
            _graphics.PreferredBackBufferHeight = screenHeight;
            _graphics.ApplyChanges();

            IsMouseVisible = true;

            base.Initialize();
        }

        int sliderValue = 0;

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _fontSystem = new FontSystem();
            _fontSystem.AddFont(File.ReadAllBytes(@"Content/Lato-Regular.ttf"));

            testSprite1 = Content.Load<Texture2D>(@"Untitled.png");

            mika = new(GraphicsDevice, _fontSystem.GetFont(24));

            mika.AddEventHandler((type, target, _) =>
            {
                if (type != EventType.OnClick) return;
                if (target.Name != "Hello1") return;

                Console.WriteLine("Hello Clicked!");
                messages.Add("Hello Clicked!");
            });
            mika.AddEventHandler((type, target, value) =>
            {
                if (type != EventType.OnChange) return;
                if (target.Name != "sliderValue") return;

                sliderValue = (int)value;
            });
            mika.AddEventHandler((type, target, value) =>
            {
                if (type != EventType.OnClick) return;
                if (target.Name != "sliderValue") return;

                var msg = $"Slider value is now {sliderValue}";
                Console.WriteLine(msg);
                messages.Add(msg);
            });
            mika.AddEventHandler((type, target, value) =>
            {
                if (type != EventType.OnClick) return;
                if (target.Name != "youClickedMe") return;

                Console.WriteLine("You clicked the clickable icon! ☺");
                messages.Add("You clicked the clickable icon! ☺");
            });
            mika.AddEventHandler((type, target, @checked) =>
            {
                if (type != EventType.OnChange) return;
                if (target.Name != "testCheckbox") return;

                checkbox = !(bool)@checked;

                Console.WriteLine("Checkbox Toggled!");
                messages.Add("Checkbox Toggled!");
            });
            mika.AddEventHandler((type, target, _) =>
            {
                if (type != EventType.OnClick) return;
                if (target.Name != "clearMessages") return;

                messages.Clear();
            });

            icons = new(Content);
            icons.LoadIcons(@"icons.png", @"icons.csv");
        }

        protected override void UnloadContent()
        {
            mika.Clean();
        }

        bool ExitApp = false;
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            mika.Update();

            if (ExitApp)
            {
                Exit();
                return;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            mika.Begin(screenWidth, screenHeight, LayoutType.Horizontal, layoutSpacing: 8);
            { // Using block-scoped code is not required. Just for readability.

                mika.AddCursorPos(new Point(20, 20));
                mika.Panel(LayoutType.Vertical, new Point(320, 0), Style.New().WithPadding(8).WithOpacity(0.5f));
                {
                    mika.Text("Demo");

                    mika.Divider();

                    mika.Text("Button:");
                    mika.SameLine(150);
                    mika.Button("Hello!!!", Style.ButtonDefault, EventData.New("Hello1"));

                    mika.Text("Slider:");
                    mika.SameLine(150);
                    mika.Slider(sliderValue, 0, 100, EventData.New("sliderValue"));

                    mika.Text("Sprite:");
                    mika.SameLine(150);
                    var icon = icons.GetIcon("smile");
                    mika.Sprite(icons.TextureAtlas, icon.SourceRect, icon.Size);

                    mika.Text("Clickable Sprite:");
                    mika.SameLine(150);
                    mika.ButtonLayout(LayoutType.Horizontal, EventData.New("youClickedMe"));
                    {
                        mika.Sprite(icons.TextureAtlas, icon.SourceRect, icon.Size);
                        mika.CloseButtonLayout();
                    }

                    mika.Text("Checkbox:");
                    mika.SameLine(150);
                    mika.Checkbox("Test Checkbox", checkbox, eventData: EventData.New("testCheckbox"));

                    mika.Button("Clear Messages", Style.ButtonDefault, EventData.New("clearMessages"));

                    mika.ClosePanel();
                }

                mika.AddCursorPos(new Point(12, 0));
                mika.Panel(LayoutType.Vertical, LayoutSizingMode.Fixed, new Point(300, 300), Style.New().WithPadding(8));
                {
                    mika.ScrollView();
                    {
                        foreach (var message in messages)
                            mika.Text(message);

                        mika.CloseScrollView();
                    }

                    mika.ClosePanel();
                }

                mika.End();
            }

            GraphicsDevice.Clear(bg);

            mika.Render(GraphicsDevice, _spriteBatch);

            base.Draw(gameTime);
        }
    }

    static void Main(string[] args)
    {
        using var game = new Game();
        game.Run();
    }
}
