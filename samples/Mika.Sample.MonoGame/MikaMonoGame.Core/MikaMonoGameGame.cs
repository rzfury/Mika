using System;
using System.IO;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mika;

namespace MikaMonoGame.Core
{
    public class MikaMonoGameGame : Game
    {
        // Resources for drawing.
        private GraphicsDeviceManager graphicsDeviceManager;
        private SpriteBatch _spriteBatch;
        private FontSystem _fontSystem;

        Context mika;
        Texture2D testSprite1;

        private int screenWidth = 1600;
        private int screenHeight = 900;

        public MikaMonoGameGame()
        {
            graphicsDeviceManager = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            graphicsDeviceManager.PreferredBackBufferWidth = screenWidth;
            graphicsDeviceManager.PreferredBackBufferHeight = screenHeight;
            graphicsDeviceManager.ApplyChanges();

            mika = new Context();
            mika.RegisterEvent((eventType, target) =>
            {
                if (eventType == EventType.OnClick && target.Name == "Hello1")
                    Console.WriteLine("Hello 1 Click!");
            });

        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var dot = new Texture2D(GraphicsDevice, 1, 1);
            dot.SetData([Color.White]);
            Context.DotTexture = dot;

            _fontSystem = new FontSystem();
            _fontSystem.AddFont(File.ReadAllBytes(@"Content/Fonts/Roboto-Bold.ttf"));
            testSprite1 = Content.Load<Texture2D>(@"splash.png");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
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
                    mika.Sprite(testSprite1, new Point(testSprite1.Width / 3, testSprite1.Height / 3), new Style { Color = Color.White });
                    mika.SolidRect(new Point(128, 32), new Style { Color = Color.Red });

                    mika.ClosePanel();
                }

                mika.SolidRect(new Point(128, 32), new Style { Color = Color.Red });
                mika.SolidRect(new Point(128, 32), new Style { Color = Color.Red });

                mika.ClosePanel();
            }

            mika.SetCursorPos(screenWidth - (testSprite1.Width / 3), 0);
            mika.Sprite(testSprite1, new Point(testSprite1.Width / 3, testSprite1.Height / 3), new Style { Color = Color.White });

            mika.End();

            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            Mika.BuiltIn.Renderer.Render(_spriteBatch, mika);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}