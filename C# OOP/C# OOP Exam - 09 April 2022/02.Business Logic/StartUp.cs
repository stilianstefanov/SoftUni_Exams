﻿namespace Formula1
{
    using Core;
    using Core.Contracts;
    public class StartUp
    {
        public static void Main()
        {
            IEngine engine = new Engine();
            engine.Run();
        }
    }
}
