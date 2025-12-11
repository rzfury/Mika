using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Mika
{
    public partial class Context
    {
        internal const int ElementsCapacity = 4096;
        internal const int KeyPressesCapacity = 128;
        internal const int CommandsCapacity = 1024;
        internal const int LayoutStackCapacity = 32;

        public uint Hover = Hash.Empty;
        public uint Focus = Hash.Empty;
        public uint Active = Hash.Empty;
        internal uint PrevHover = Hash.Empty;
        internal uint PrevFocus = Hash.Empty;
        internal uint PrevActive = Hash.Empty;
        public uint ScrollTarget = Hash.Empty;

        internal readonly Stack<uint> Ids = new Stack<uint>(ElementsCapacity);
        internal readonly Stack<uint> IdStack = new Stack<uint>(ElementsCapacity);
        internal readonly Stack<LayoutState> LayoutStack = new Stack<LayoutState>(32);
        internal readonly Stack<ContainerState> ContainerStack = new Stack<ContainerState>(32);
        private int _autoIdWidgetIndex = 0;

        internal List<DrawCommand> Commands = new List<DrawCommand>(CommandsCapacity);
        internal Queue<DrawCommand> CommandQueue = new Queue<DrawCommand>(CommandsCapacity);

        internal Caches Caches = new Caches();

        public int ScreenWidth = 0;
        public int ScreenHeight = 0;

        public delegate void MikaEventHandler(EventType type, EventData eventData);
        public event MikaEventHandler Events;

        public EventData CurrentEventTarget { get; internal set; } = default;
        public string NextEventTargetName { get; internal set; } = "";

        public MouseState MouseState = default;
        public MouseState PrevMouseState = default;

        public Vector2 MousePosition => new Vector2(MouseState.X, MouseState.Y);

        public KeyboardState KeyboardState = default;
        public KeyboardState PrevKeyboardState = default;

        public GamePadState GamePadState = default;
        public GamePadState PrevGamePadState = default;

        public void Begin(
            int screenWidth,
            int screenHeight,
            LayoutType layoutType = LayoutType.Vertical,
            Point layoutSize = default,
            Point layoutMaxSize = default,
            int layoutSpacing = default)
        {
            Hover = Hash.Empty;
            Focus = Hash.Empty;
            Active = Hash.Empty;

            Ids.Clear();
            IdStack.Clear();
            LayoutStack.Clear();

            Commands.Clear();

            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;

            Layout(layoutType, layoutSize, layoutMaxSize, layoutSpacing);
        }

        public void End()
        {
            CloseLayout();

            if (LayoutStack.Count > 0)
                throw new Exception("Some layout is not closed!");

            //if (IdStack.Count > 0)
            //    throw new Exception("Some layout is not closed!");

            PrevHover = Hover;
            PrevFocus = Focus;
            PrevActive = Active;

            foreach (var command in Commands)
            {
                var copy = command;
                copy.CurrentZIndex = Math.Max(0, command.CurrentZIndex);
                CommandQueue.Enqueue(copy);
            }

            Commands.Clear();

            _autoIdWidgetIndex = 0;
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

        public void RegisterEvent(Action<EventType, EventData> handler)
        {
            Events += new MikaEventHandler(handler);
        }

        public uint GetId(string id)
        {
            var hash = Hash.Of(id);

            if (IdStack.Count > 0)
            {
                var parent = IdStack.Peek();
                hash = Hash.Combine(parent, hash);
            }

            return hash;
        }

        public uint GetId()
        {
            return GetId((_autoIdWidgetIndex++).ToString());
        }

        public bool IsHot(uint id)
        {
            return Hover == id || Focus == id;
        }

        public bool TryDequeueCommand(out DrawCommand command)
        {
            DrawCommand tempCommand;

            if (CommandQueue.Count == 0)
            {
                command = default;
                return false;
            }

            while (true)
            {
                tempCommand = CommandQueue.Dequeue();

                if (tempCommand.CurrentZIndex < 0)
                    break;

                tempCommand.CurrentZIndex--;
                CommandQueue.Enqueue(tempCommand);
            }

            command = tempCommand;

            return true;
        }

        public void SetNextEventTargetName(string nextEventTargetName)
        {
            NextEventTargetName = nextEventTargetName;
        }

        private static Texture2D _dotTexture = null;
        /// <summary>
        /// A texture used for solid color and default texture.
        /// </summary>
        public static Texture2D DotTexture
        {
            get { return _dotTexture; }
            set { _dotTexture = value; }
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