using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mika
{
    public partial class Context
    {
        internal const int ElementsCapacity = 4096;
        internal const int KeyPressesCapacity = 128;
        internal const int CommandsCapacity = 1024;
        internal const int StackCapacity = 64;
        internal const int ScrollableCapacity = 64;
        internal const int ScrollableIdSeed = 2147483580;

        public uint Hover;
        public uint Focus;
        public uint Active;
        internal uint PrevHover;
        internal uint PrevFocus;
        internal uint PrevActive;
        public uint ScrollTarget;

        internal readonly Stack<LayoutState> LayoutStack;
        internal readonly Stack<ContainerState> ContainerStack;
        internal readonly Stack<ClippingState> ClippingStack;
        internal readonly Stack<uint> ScrollStack;
        internal readonly Dictionary<uint, ScrollState> ScrollStates;
        private int _autoIdWidgetIndex;
        private int _autoIdScrollableIndex;

        internal List<DrawCommand> Commands;
        internal Queue<DrawCommand> CommandQueue;

        public Theme Theme { get; set; }

        public int ScreenWidth = 0;
        public int ScreenHeight = 0;
        internal Rectangle FullscreenRect;

        public delegate void MikaEventHandler(EventType type, EventData eventData, object eventValue);
        public event MikaEventHandler Events;
        private readonly List<MikaEventHandler> _handlers;

        public EventData CurrentEventTarget { get; internal set; }
        public string NextEventTargetName { get; internal set; }

        public SpriteBatchPresets SpriteBatchPresets;

        public int ScrollSpeed;

        public MouseState MouseState;
        public MouseState PrevMouseState;

        public Vector2 PrevMousePosition;
        public Vector2 MousePosition => new Vector2(MouseState.X, MouseState.Y);

        public KeyboardState KeyboardState;
        public KeyboardState PrevKeyboardState;

        public GamePadState GamePadState;
        public GamePadState PrevGamePadState;

        public Context(
            GraphicsDevice graphicsDevice,
            SpriteFontBase defaultFont)
        {
            Hover = Hash.Empty;
            Focus = Hash.Empty;
            Active = Hash.Empty;
            PrevHover = Hash.Empty;
            PrevFocus = Hash.Empty;
            PrevActive = Hash.Empty;
            ScrollTarget = Hash.Empty;

            LayoutStack = new Stack<LayoutState>(StackCapacity);
            ContainerStack = new Stack<ContainerState>(StackCapacity);
            ClippingStack = new Stack<ClippingState>(StackCapacity);
            ScrollStack = new Stack<uint>(ScrollableCapacity);
            ScrollStates = new Dictionary<uint, ScrollState>(ScrollableCapacity);

            Commands = new List<DrawCommand>(CommandsCapacity);
            CommandQueue = new Queue<DrawCommand>(CommandsCapacity);

            Theme = new Theme();

            ScreenWidth = 0;
            ScreenHeight = 0;

            _handlers = new List<MikaEventHandler>();

            NextEventTargetName = "";

            SpriteBatchPresets = new SpriteBatchPresets
            {
                BlendState = BlendState.NonPremultiplied,
                SamplerState = SamplerState.LinearClamp,
                DepthStencilState = DepthStencilState.None,
                RasterizerState = RasterizerState.CullNone,
                Effect = null,
                Matrix = Matrix.Identity
            };
            SpriteBatchPresets.RasterizerState.ScissorTestEnable = true;

            ScrollSpeed = 40;

            var dot = new Texture2D(graphicsDevice, 1, 1);
            dot.SetData(new Color[] { Color.White });
            DotTexture = dot;
            DefaultFont = defaultFont;
        }

        public void Begin(
            int screenWidth,
            int screenHeight,
            LayoutType layoutType = LayoutType.Vertical,
            int layoutSpacing = default)
        {
            Hover = Hash.Empty;
            Focus = Hash.Empty;
            Active = Hash.Empty;

            LayoutStack.Clear();
            ContainerStack.Clear();
            ClippingStack.Clear();
            ScrollStack.Clear();

            _autoIdWidgetIndex = 0;
            _autoIdScrollableIndex = ScrollableIdSeed;

            Commands.Clear();

            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;
            FullscreenRect = new Rectangle(0, 0, screenWidth, screenHeight);

            Layout(layoutType, LayoutSizingMode.Fixed, new Point(ScreenWidth, ScreenHeight), layoutSpacing);
        }

        public void End()
        {
            CloseLayout();

            if (LayoutStack.Count > 0)
                throw new Exception("Some layout is not closed!");

            if (ContainerStack.Count > 0)
                throw new Exception("Some container is not closed! Might be a panel, button layout, or something.");

            PrevHover = Hover;
            PrevFocus = Focus;
            PrevActive = Active;

            PrevMousePosition = MousePosition;

            var sorted = Commands.OrderBy(c => c.ZIndex).ToList();
            foreach (var command in sorted) CommandQueue.Enqueue(command);

            Commands.Clear();
        }

        public void Update()
        {
            PrevMouseState = MouseState;
            MouseState = Mouse.GetState();

            PrevKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();

            PrevGamePadState = GamePadState;
            GamePadState = GamePad.GetState(PlayerIndex.One);
        }

        public void Clean()
        {
            foreach (var handler in _handlers) Events -= handler;
        }

        public void Render(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                SpriteBatchPresets.BlendState,
                SpriteBatchPresets.SamplerState,
                SpriteBatchPresets.DepthStencilState,
                SpriteBatchPresets.RasterizerState,
                SpriteBatchPresets.Effect,
                SpriteBatchPresets.Matrix);

            while (TryDequeueDrawCommand(out var command))
            {
                if (command.Hidden) continue;

                var pos = command.Position;
                var size = command.Size;
                var color = command.Color;
                var opacity = command.Opacity;

                var rect = Utils.RectFromPosAndSize(pos, size);
                var posVec = Utils.PointToVec2(pos);

                if (command.Hover && command.HoverColor != DefaultValues.Color) color = command.HoverColor;
                if (command.Focus && command.FocusColor != DefaultValues.Color) color = command.FocusColor;
                if (command.Active && command.ActiveColor != DefaultValues.Color) color = command.ActiveColor;

                var finalColor = new Color(color.R, color.G, color.B, Utils.LerpInt(0, 255, opacity));

                switch (command.Type)
                {
                    case DrawCommandType.Texture:
                        spriteBatch.Draw(command.Texture, rect, command.SourceRect, finalColor);
                        break;
                    case DrawCommandType.String:
                        spriteBatch.DrawString(command.Font, command.Text, posVec, finalColor, rotation: command.Rotation, origin: Utils.PointToVec2(command.Origin));
                        break;
                    case DrawCommandType.SetClipping:
                        graphicsDevice.RasterizerState.ScissorTestEnable = true;
                        graphicsDevice.ScissorRectangle = command.ClippingRect;
                        break;
                    case DrawCommandType.ResetClipping:
                        graphicsDevice.ScissorRectangle = FullscreenRect;
                        break;
                    case DrawCommandType.RTL:
                        command.RTL.Draw(spriteBatch, posVec, finalColor);
                        break;
                    default: break;
                }
            }

            spriteBatch.End();
        }

        public void AddEventHandler(Action<EventType, EventData, object> action)
        {
            var handler = new MikaEventHandler(action);
            _handlers.Add(handler);
            Events += handler;
        }

        public uint GetId(string id)
        {
            return Hash.Of(id);
        }

        public uint GetId(int id)
        {
            return Hash.Of(id);
        }

        public uint GetId()
        {
            return Hash.Of(_autoIdWidgetIndex++);
        }

        public bool IsHot(uint id)
        {
            return Hover == id || Focus == id;
        }

        public bool TryDequeueDrawCommand(out DrawCommand command)
        {
            if (CommandQueue.Count == 0)
            {
                command = default;
                return false;
            }

            command = CommandQueue.Dequeue();
            return true;
        }

        public void SetNextEventTargetName(string nextEventTargetName)
        {
            NextEventTargetName = nextEventTargetName;
        }

        internal bool MouseLeftJustPressed()
        {
            return PrevMouseState.LeftButton == ButtonState.Released && MouseState.LeftButton == ButtonState.Pressed;
        }

        internal bool MouseRightJustPressed()
        {
            return PrevMouseState.RightButton == ButtonState.Released && MouseState.RightButton == ButtonState.Pressed;
        }

        internal bool MouseMiddleJustPressed()
        {
            return PrevMouseState.MiddleButton == ButtonState.Released && MouseState.MiddleButton == ButtonState.Pressed;
        }

        internal bool MouseLeftJustReleased()
        {
            return PrevMouseState.LeftButton == ButtonState.Pressed && MouseState.LeftButton == ButtonState.Released;
        }

        internal bool MouseRightJustReleased()
        {
            return PrevMouseState.RightButton == ButtonState.Pressed && MouseState.RightButton == ButtonState.Released;
        }

        internal bool MouseMiddleJustReleased()
        {
            return PrevMouseState.MiddleButton == ButtonState.Pressed && MouseState.MiddleButton == ButtonState.Released;
        }
    }
}