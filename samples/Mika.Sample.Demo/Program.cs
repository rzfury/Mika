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

        private readonly Context mika;

        private Texture2D testSprite1;

        private int screenWidth = 1600;
        private int screenHeight = 900;

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _graphics.PreferredBackBufferWidth = screenWidth;
            _graphics.PreferredBackBufferHeight = screenHeight;
            _graphics.ApplyChanges();

            mika = new();
            mika.RegisterEvent((eventType, target) =>
            {
                if (eventType == EventType.OnClick && target.Name == "Hello1")
                    Console.WriteLine("Hello 1 Click!");
            });

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var dot = new Texture2D(GraphicsDevice, 1, 1);
            dot.SetData([Color.White]);
            Context.DotTexture = dot;

            _fontSystem = new FontSystem();
            _fontSystem.AddFont(File.ReadAllBytes(@"Content/Lato-Regular.ttf"));

            testSprite1 = Content.Load<Texture2D>(@"Untitled.png");
        }

        protected override void UnloadContent() { }

        bool ExitApp = false;
        int paddingX = 8;
        int paddingY = 8;
        int borderX = 8;
        int borderY = 8;
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            KeyboardStateHelper.UpdateState();

            if (KeyboardStateHelper.GetKeyDown(Microsoft.Xna.Framework.Input.Keys.H))
                paddingX = Math.Max(0, paddingX - 1);

            if (KeyboardStateHelper.GetKeyDown(Microsoft.Xna.Framework.Input.Keys.J))
                paddingX++;

            if (KeyboardStateHelper.GetKeyDown(Microsoft.Xna.Framework.Input.Keys.N))
                paddingY = Math.Max(0, paddingY - 1);

            if (KeyboardStateHelper.GetKeyDown(Microsoft.Xna.Framework.Input.Keys.M))
                paddingY++;

            if (KeyboardStateHelper.GetKeyDown(Microsoft.Xna.Framework.Input.Keys.K))
                borderX = Math.Max(0, borderX - 1);

            if (KeyboardStateHelper.GetKeyDown(Microsoft.Xna.Framework.Input.Keys.L))
                borderX++;

            if (KeyboardStateHelper.GetKeyDown(Microsoft.Xna.Framework.Input.Keys.OemComma))
                borderY = Math.Max(0, borderY - 1);

            if (KeyboardStateHelper.GetKeyDown(Microsoft.Xna.Framework.Input.Keys.OemPeriod))
                borderY++;

            mika.Update();

            if (ExitApp)
            {
                Exit();
                return;
            }
        }

        protected override void Draw(GameTime gameTime)
        {

            mika.Begin(screenWidth, screenHeight, LayoutType.Absolute);

            mika.Panel(
                LayoutType.Vertical,
                style: new Style
                {
                    Color = Color.White,
                    Border = Edges.All(8),
                    BorderColor = Color.Gray,
                    Padding = Edges.All(8),
                    Spacing = 8
                });
            {
                mika.Button("Hello!!!", _fontSystem.GetFont(26),
                    new Point(256, 0),
                    style: new Style
                    {
                        Color = Color.Blue,
                        Padding = Edges.All(4),
                        Border = Edges.All(4),
                        BorderColor = Color.DarkBlue,
                        TextAlign = TextAlignment.Left,
                        TextColor = Color.White
                    },
                    eventData: EventData.Create("Hello1"));
                mika.Button("Hello!!!", _fontSystem.GetFont(26),
                    new Point(256, 0),
                    style: new Style
                    {
                        Color = Color.Blue,
                        Padding = Edges.All(4),
                        Border = Edges.All(4),
                        BorderColor = Color.DarkBlue,
                        TextAlign = TextAlignment.Center,
                        TextColor = Color.White
                    });
                mika.Button("Hello!!!", _fontSystem.GetFont(26),
                    new Point(256, 0),
                    style: new Style
                    {
                        Color = Color.Blue,
                        Padding = Edges.All(4),
                        Border = Edges.All(4),
                        BorderColor = Color.DarkBlue,
                        TextAlign = TextAlignment.Right,
                        TextColor = Color.White
                    });

                mika.Panel(
                    LayoutType.Vertical,
                    style: new Style
                    {
                        Color = Color.White,
                        Border = Edges.All(8),
                        BorderColor = Color.Gray,
                        Padding = Edges.All(8),
                        Spacing = 8
                    });
                {
                    mika.SolidRect(new Point(128, 32), new Style { Color = Color.Red });
                    mika.Sprite(testSprite1, new Point(testSprite1.Width, testSprite1.Height), new Style { Color = Color.White });
                    mika.SolidRect(new Point(128, 32), new Style { Color = Color.Red });

                    mika.ClosePanel();
                }

                mika.SolidRect(new Point(128, 32), new Style { Color = Color.Red });
                mika.SolidRect(new Point(128, 32), new Style { Color = Color.Red });

                mika.ClosePanel();
            }

            mika.SetCursorPos(screenWidth - testSprite1.Width, 0);
            mika.Sprite(testSprite1, new Point(testSprite1.Width, testSprite1.Height), new Style { Color = Color.White });

            mika.End();

            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            Mika.BuiltIn.Renderer.Render(_spriteBatch, mika);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }

    static void Main(string[] args)
    {
        using var game = new Game();
        game.Run();
    }
}
