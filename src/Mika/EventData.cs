namespace Mika
{
    public struct EventData
    {
        public string Name;
        public string NavigateLeftTarget;
        public string NavigateRightTarget;
        public string NavigateUpTarget;
        public string NavigateDownTarget;
        public bool DetectLeftMouse;
        public bool DetectRightMouse;
        public bool DetectMiddleMouse;

        public static EventData Create(
            string name = "",
            string navigateLeftTarget = "",
            string navigateRightTarget = "",
            string navigateUpTarget = "",
            string navigateDownTarget = "",
            bool detectLeftMouse = true,
            bool detectRightMouse = false,
            bool detectMiddleMouse = false
            )
        {
            return new EventData
            {
                Name = name,
                NavigateLeftTarget = navigateLeftTarget,
                NavigateRightTarget = navigateRightTarget,
                NavigateUpTarget = navigateUpTarget,
                NavigateDownTarget = navigateDownTarget,
                DetectLeftMouse = detectLeftMouse,
                DetectRightMouse = detectRightMouse,
                DetectMiddleMouse = detectMiddleMouse
            };
        }
    }
}
